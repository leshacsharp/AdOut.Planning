using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Managers
{
    public class PlanManager : BaseManager<Plan>, IPlanManager
    {
        private readonly IPlanRepository _planRepository;
        private readonly IAdRepository _adRepository;
        private readonly IPlanAdPointRepository _planAdPointRepository;
        private readonly IPlanAdRepository _planAdRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public PlanManager(
            IPlanRepository planRepository,
            IAdRepository adRepository,
            IPlanAdPointRepository planAdPointRepository,
            IPlanAdRepository planAdRepository,
            IScheduleRepository scheduleRepository) 
            : base(planRepository)
        {
            _planRepository = planRepository;
            _adRepository = adRepository;
            _planAdPointRepository = planAdPointRepository;
            _planAdRepository = planAdRepository;
            _scheduleRepository = scheduleRepository;
        }

        public void Create(CreatePlanModel createModel, string userId)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }

            var plan = new Plan()
            {
                UserId = userId,
                Title = createModel.Title,
                Type = createModel.Type,
                StartDateTime = createModel.StartDateTime,
                EndDateTime = createModel.EndDateTime,
                AdsTimePlaying = createModel.AdsTimePlaying
            };

            Create(plan);

            foreach (var adPointId in createModel.AdPointsIds)
            {
                var planAdPoint = new PlanAdPoint()
                {
                    AdPointId = adPointId,
                    Plan = plan
                };

                _planAdPointRepository.Create(planAdPoint);
            }

            foreach (var planAd in createModel.PlanAds)
            {
                var planAdEntity = new PlanAd()
                {
                    AdId = planAd.AdId,
                    Order = planAd.Order,
                    Plan = plan
                };

                _planAdRepository.Create(planAdEntity);
            }

            foreach (var scheduleDto in createModel.Schedules)
            {
                var scheduleEntity = new Model.Database.Schedule()
                {
                    StartTime = scheduleDto.StartTime,
                    EndTime = scheduleDto.EndTime,
                    BreakTime = scheduleDto.BreakTime,
                    Date = scheduleDto.Date,
                    DayOfWeek = scheduleDto.DayOfWeek,
                    Plan = plan
                };

                _scheduleRepository.Create(scheduleEntity);
            }
        }

        public async Task AddAdAsync(int planId, int adId, int order)
        {
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            var ad = await _adRepository.GetByIdAsync(adId);
            if (ad == null)
            {
                throw new ObjectNotFoundException($"Ad with id={adId} was not found");
            }

            var planHasAdWithSameOrder = await _planAdRepository.Read(pa => pa.PlanId == planId && pa.Order == order).AnyAsync();
            if (planHasAdWithSameOrder)
            {
                throw new BadRequestException($"Plan with id={planId} has ad with order={order}");
            }

            var planAd = new PlanAd()
            {
                Plan = plan,
                Ad = ad,
                Order = order
            };

            _planAdRepository.Create(planAd);
        }

        public async Task DeleteAdAsync(int planId, int adId)
        {
            var planAd = await _planAdRepository.GetByIdAsync(planId, adId);
            if (planAd == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }
         
            var countPlanAds = await _planAdRepository.Read(pa => pa.PlanId == planId).CountAsync();
            if (countPlanAds == 1)
            {
                throw new BadRequestException($"Plan cannot exist without ads. Plan with id={planId} has one ad");
            }

            _planAdRepository.Delete(planAd);
        }
    }
}
