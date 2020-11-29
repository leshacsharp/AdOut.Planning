using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Managers
{
    public class ScheduleManager : BaseManager<Model.Database.Schedule>, IScheduleManager
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly IScheduleTimeHelperProvider _scheduleTimeHelperProvider;

        public ScheduleManager(
            IScheduleRepository scheduleRepository,
            IPlanRepository planRepository,
            IScheduleValidatorFactory scheduleValidatorFactory,
            IScheduleTimeHelperProvider scheduleTimeHelperProvider)
            : base(scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
            _planRepository = planRepository;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _scheduleTimeHelperProvider = scheduleTimeHelperProvider;
        }


        public async Task<ValidationResult<string>> ValidateScheduleAsync(ScheduleModel scheduleModel)
        {
            //todo: refactoring and variables naming

            if (scheduleModel == null)
            {
                throw new ArgumentNullException(nameof(scheduleModel));
            }

            var plan = await _planRepository.GetPlanScheduleValidationAsync(scheduleModel.PlanId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={scheduleModel.PlanId} was not found");
            }

            var planValidations = await _planRepository.GetPlanValidationsAsync(scheduleModel.PlanId, plan.StartDateTime, plan.EndDateTime);
            var existingSchedulePeriods = new List<SchedulePeriod>();

            foreach (var p in planValidations)
            {
                foreach (var s in p.Schedules)
                {
                    var timeHelper = _scheduleTimeHelperProvider.CreateScheduleTimeHelper(s.Type);
                    var existingScheduleTime = MapToScheduleTime(p, s);
                    var existingSchedulePeriod = timeHelper.GetScheduleTimeLine(existingScheduleTime);
                    existingSchedulePeriods.Add(existingSchedulePeriod);
                }
            }

            var scheduleTimeHelper = _scheduleTimeHelperProvider.CreateScheduleTimeHelper(scheduleModel.Type);
            var newScheduleTime = MapToScheduleTime(plan, scheduleModel);
            var newSchedulePeriod = scheduleTimeHelper.GetScheduleTimeLine(newScheduleTime);

            var validationContext = new ScheduleValidationContext()
            {
                ScheduleStartTime = scheduleModel.StartTime,
                ScheduleEndTime = scheduleModel.EndTime,
                AdPlayTime = scheduleModel.PlayTime,
                AdBreakTime = scheduleModel.BreakTime,
                ScheduleDayOfWeek = scheduleModel.DayOfWeek,
                ScheduleDate = scheduleModel.Date,
                ScheduleType = scheduleModel.Type,
                PlanStartDateTime = plan.StartDateTime,
                PlanEndDateTime = plan.EndDateTime,
                AdPoints = plan.AdPoints.ToList(),
                ExistingSchedulePeriods = existingSchedulePeriods,
                NewSchedulePeriod = newSchedulePeriod
            };

            var chainOfValidators = _scheduleValidatorFactory.CreateChainOfAllValidators();
            chainOfValidators.Validate(validationContext);

            var validationResult = new ValidationResult<string>()
            {
                Errors = validationContext.Errors
            };

            return validationResult;
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

            var scheduleTime = await _scheduleRepository.GetScheduleTimeAsync(updateModel.ScheduleId);
            var scheduleTimeHelper = _scheduleTimeHelperProvider.CreateScheduleTimeHelper(scheduleTime.ScheduleType);

            var timeOfAdsShowingBeforeUpdating = scheduleTimeHelper.GetTimeOfAdsShowing(scheduleTime);

            scheduleTime.ScheduleStartTime = updateModel.StartTime;
            scheduleTime.ScheduleEndTime = updateModel.EndTime;
            scheduleTime.AdBreakTime = updateModel.BreakTime;
            scheduleTime.ScheduleDayOfWeek = updateModel.DayOfWeek;
            scheduleTime.ScheduleDate = updateModel.Date;

            var timeOfAdsShowingAfterUpdating = scheduleTimeHelper.GetTimeOfAdsShowing(scheduleTime);

            if (timeOfAdsShowingAfterUpdating > timeOfAdsShowingBeforeUpdating)
            {
                throw new BadRequestException(ValidationMessages.Schedule.TimeIncreased);
            }

            schedule.StartTime = updateModel.StartTime;
            schedule.EndTime = updateModel.EndTime;
            schedule.BreakTime = updateModel.BreakTime;
            schedule.DayOfWeek = updateModel.DayOfWeek;
            schedule.Date = updateModel.Date;

            Update(schedule);
        }

        //todo: create mapper for these entities
        private ScheduleTime MapToScheduleTime(PlanValidation plan, ScheduleDto schedule)
        {
            var adPointsDaysOff = plan.AdPointsDaysOff.Distinct();

            return new ScheduleTime()
            {
                AdPointsDaysOff = adPointsDaysOff,
                PlanStartDateTime = plan.StartDateTime,
                PlanEndDateTime = plan.EndDateTime,
                ScheduleType = schedule.Type,
                ScheduleStartTime = schedule.StartTime,
                ScheduleEndTime = schedule.EndTime,
                AdPlayTime = schedule.PlayTime,
                AdBreakTime = schedule.BreakTime,
                ScheduleDayOfWeek = schedule.DayOfWeek,
                ScheduleDate = schedule.Date
            };
        }

        private ScheduleTime MapToScheduleTime(SchedulePlanValidation plan, ScheduleModel schedule)
        {
            var adPointsDaysOff = plan.AdPoints.SelectMany(ap => ap.DaysOff).Distinct();

            return new ScheduleTime()
            {
                AdPointsDaysOff = adPointsDaysOff,
                PlanStartDateTime = plan.StartDateTime,
                PlanEndDateTime = plan.EndDateTime,
                ScheduleType = schedule.Type,
                ScheduleStartTime = schedule.StartTime,
                ScheduleEndTime = schedule.EndTime,
                AdPlayTime = schedule.PlayTime,
                AdBreakTime = schedule.BreakTime,
                ScheduleDayOfWeek = schedule.DayOfWeek,
                ScheduleDate = schedule.Date
            };
        }
    }
}
