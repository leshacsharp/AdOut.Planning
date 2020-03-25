using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;

namespace AdOut.Planning.Core.Managers
{
    public class PlanManager : BaseManager<Plan>, IPlanManager
    {
        private readonly IPlanRepository _planRepository;
        private readonly IPlanAdPointRepository _planAdPointRepository;
        private readonly IPlanAdRepository _planAdRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public PlanManager(
            IPlanRepository planRepository,
            IPlanAdPointRepository planAdPointRepository,
            IPlanAdRepository planAdRepository,
            IScheduleRepository scheduleRepository) 
            : base(planRepository)
        {
            _planRepository = planRepository;
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

            foreach (var adId in createModel.AdsIds)
            {
                var planAd = new PlanAd()
                {
                    AdId = adId,
                    Plan = plan
                };

                _planAdRepository.Create(planAd);
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
    }
}
