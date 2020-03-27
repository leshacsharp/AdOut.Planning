using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Collections.Generic;
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
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly ITimeLineHelper _timeLineHelper;

        public PlanManager(
            IPlanRepository planRepository,
            IAdRepository adRepository,
            IPlanAdPointRepository planAdPointRepository,
            IPlanAdRepository planAdRepository,
            IScheduleRepository scheduleRepository,
            IScheduleValidatorFactory scheduleValidatorFactory,
            ITimeLineHelper timeLineHelper) 
            : base(planRepository)
        {
            _planRepository = planRepository;
            _adRepository = adRepository;
            _planAdPointRepository = planAdPointRepository;
            _planAdRepository = planAdRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _timeLineHelper = timeLineHelper;
        }


        public async Task<ValidationResult<string>> ValidatePlanExtension(int planId, DateTime newEndDate)
        {
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            var adPointsIds = await _planAdPointRepository.GetAdPointsIds(planId);
            var adPointsPlans = await _planRepository.GetByAdPoints(adPointsIds.ToArray(), plan.EndDateTime, newEndDate);

            var existingAdsPeriods = new List<AdPeriod>();
            foreach (var apPlan in adPointsPlans)
            {
                foreach (var schedule in apPlan.Schedules)
                {
                    var schdeduleAdsPeriods = _timeLineHelper.GetScheduleTimeLine(schedule, apPlan.AdsTimePlaying);
                    existingAdsPeriods.AddRange(schdeduleAdsPeriods);
                }
            }

            var scheduleTimeIntersectionValidator = _scheduleValidatorFactory.CreateChainOfValidators(ValidatorType.ScheduleIntersectionTime);
            var planSchedules = await _scheduleRepository.GetByPlanAsync(planId);

            var validationContext = new ScheduleValidationContext()
            {
                ExistingAdsPeriods = existingAdsPeriods,
                Plan = new PlanValidation() { Type = plan.Type }
            };

            foreach (var schedule in planSchedules)
            {
                var scheduleAdsPeriods = _timeLineHelper.GetScheduleTimeLine(schedule, plan.AdsTimePlaying);

                validationContext.Schedule = schedule;
                validationContext.NewAdsPeriods = scheduleAdsPeriods;

                scheduleTimeIntersectionValidator.Validate(validationContext);
            }

            var validationResult = new ValidationResult<string>()
            {
                Errors = validationContext.Errors
            };

            return validationResult;
        }

        public async Task ExtendPlan(int planId, DateTime newEndDate)
        {

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

        public async Task UpdateAsync(UpdatePlanModel updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }

            var plan = await _planRepository.GetByIdAsync(updateModel.PlanId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={updateModel.PlanId} was not found");
            }

            plan.Title = updateModel.Title;

            Update(plan);
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
                throw new ObjectNotFoundException($"PlanAd with id=(planId={planId},adId={adId}) was not found");
            }
         
            var planAdsCount = await _planAdRepository.Read(pa => pa.PlanId == planId).CountAsync();
            if (planAdsCount == 1)
            {
                throw new BadRequestException($"Plan cannot exist without ads. Plan with id={planId} has one ad");
            }

            _planAdRepository.Delete(planAd);
        }

        public async Task UpdateAdAsync(int planId, int adId, int order)
        {
            var planAd = await _planAdRepository.GetByIdAsync(planId, adId);
            if (planAd == null)
            {
                throw new ObjectNotFoundException($"Plan Ad with id=(planId={planId},adId={adId}) was not found");
            }

            planAd.Order = order;

            _planAdRepository.Update(planAd);
        }
    }
}
