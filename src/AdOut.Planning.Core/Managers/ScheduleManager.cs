using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Managers
{
    public class ScheduleManager : BaseManager<Model.Database.Schedule>, IScheduleManager
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IPlanAdPointRepository _planAdPointRepository;
        private readonly IAdPointRepository _adPointRepository;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly IScheduleTimeHelperProvider _scheduleTimeHelperProvider;
        private readonly ITimeLineHelper _timeLineHelper;

        public ScheduleManager(
            IScheduleRepository scheduleRepository,
            IPlanRepository planRepository,
            IPlanAdPointRepository planAdPointRepository,
            IAdPointRepository adPointRepository,
            IScheduleValidatorFactory scheduleValidatorFactory,
            IScheduleTimeHelperProvider scheduleTimeHelperProvider,
            ITimeLineHelper timeLineHelper)
            : base(scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
            _planRepository = planRepository;
            _planAdPointRepository = planAdPointRepository;
            _adPointRepository = adPointRepository;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _scheduleTimeHelperProvider = scheduleTimeHelperProvider;
            _timeLineHelper = timeLineHelper;
        }
        public async Task<ValidationResult<string>> ValidateScheduleAsync(ScheduleModel scheduleModel)
        {
            if (scheduleModel == null)
            {
                throw new ArgumentNullException(nameof(scheduleModel));
            }

            var plan = await _planRepository.GetByIdAsync(scheduleModel.PlanId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={scheduleModel.PlanId} was not found");
            }

            //todo: ScheduleModel and ScheduleDto are same (need to do something with that)
            var scheduleDto = new ScheduleDto()
            {
                StartTime = scheduleModel.StartTime,
                EndTime = scheduleModel.EndTime,
                BreakTime = scheduleModel.BreakTime,
                DayOfWeek = scheduleModel.DayOfWeek,
                Date = scheduleModel.Date
            };
            
            //todo: maybe need to invoke 'Read' method (for resuable methods no)
            var adPointsIds = await _planAdPointRepository.GetAdPointsIds(plan.Id);
            ////todo: think about sense of List<AdPointValidation> (the list contains same plans and schedules!!!)
            var adPointsValidations = await _adPointRepository.GetAdPointsValidationAsync(adPointsIds.ToArray(), plan.StartDateTime, plan.EndDateTime);

            var existingsAdsPeriods = new List<AdPeriod>();
            foreach (var adPoint in adPointsValidations)
            {
                foreach (var adPointPlan in adPoint.Plans)
                {
                    foreach (var schedule in adPointPlan.Schedules)
                    {
                        var schdeduleAdsPeriods = _timeLineHelper.GetScheduleTimeLine(schedule, plan.AdsTimePlaying);
                        existingsAdsPeriods.AddRange(schdeduleAdsPeriods);
                    }
                }
            }

            var newAdsPeriods = _timeLineHelper.GetScheduleTimeLine(scheduleDto, plan.AdsTimePlaying);

            var validationContext = new ScheduleValidationContext()
            {
                Schedule = scheduleDto,
                AdPointsValidations = adPointsValidations,
                ExistingAdsPeriods = existingsAdsPeriods,
                NewAdsPeriods = newAdsPeriods,
                Plan = new SchedulePlan()
                {
                    Type = plan.Type,
                    StartDateTime = plan.StartDateTime,
                    EndDateTime = plan.EndDateTime
                }
            };

            var scheduleValidator = _scheduleValidatorFactory.CreateChainOfValidators();
            scheduleValidator.Validate(validationContext);

            var validationResult = new ValidationResult<string>()
            {
                Errors = validationContext.Errors
            };

            return validationResult;
        }

        public async Task<List<PlanTimeLine>> GetPlansTimeLines(int adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var plansTimeLines = new List<PlanTimeLine>();
            var adPointPlans = await _planRepository.GetByAdPoints(new int[] { adPointId }, dateFrom, dateTo);

            foreach (var plan in adPointPlans)
            {
                var planTimeLine = new PlanTimeLine() { PlanId = plan.Id };     
                foreach (var schedule in plan.Schedules)
                {
                    var scheduleAdPeriods = _timeLineHelper.GetScheduleTimeLine(schedule, plan.AdsTimePlaying);
                    planTimeLine.AdsPeriods.AddRange(scheduleAdPeriods);
                }

                plansTimeLines.Add(planTimeLine);
            }

            return plansTimeLines;
        }

        public async Task CreateAsync(ScheduleModel createModel)
        {
            if (createModel == null) 
            {
                throw new ArgumentNullException(nameof(createModel));
            }

            var plan = await _planRepository.GetByIdAsync(createModel.PlanId);
            if (plan == null) 
            {
                throw new ObjectNotFoundException($"Plan with id={createModel.PlanId} was not found");
            }

            var schedule = new Model.Database.Schedule()
            {
                StartTime = createModel.StartTime,
                EndTime = createModel.EndTime,
                BreakTime = createModel.BreakTime,
                Date = createModel.Date,
                DayOfWeek = createModel.DayOfWeek,
                Plan = plan
            };

            Create(schedule);
        }

        public async Task UpdateAsync(UpdateScheduleModel updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }

            var schedule = await _scheduleRepository.GetByIdAsync(updateModel.ScheduleId);
            if (schedule == null)
            {
                throw new ObjectNotFoundException($"Schedule with id={updateModel.ScheduleId} was not found");
            }

            var plan = await _planRepository.GetByIdAsync(schedule.PlanId);
            var scheduleTimeHelper = _scheduleTimeHelperProvider.CreateScheduleTimeHelper(plan.Type);

            var timeOfAdsShowingBeforeUpdating = scheduleTimeHelper.GetTimeOfAdsShowing(plan, schedule);

            schedule.StartTime = updateModel.StartTime;
            schedule.EndTime = updateModel.EndTime;
            schedule.BreakTime = updateModel.BreakTime;
            schedule.DayOfWeek = updateModel.DayOfWeek;
            schedule.Date = updateModel.Date;

            var timeOfAdsShowingAfterUpdating = scheduleTimeHelper.GetTimeOfAdsShowing(plan, schedule);

            if (timeOfAdsShowingAfterUpdating > timeOfAdsShowingBeforeUpdating)
            {
                throw new BadRequestException(ValidationMessages.Schedule.TimeIsIncreased);
            }

            Update(schedule);
        }
    }
}
