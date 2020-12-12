using AdOut.Planning.Core.Mapping;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Services;
using AutoMapper;
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
        private readonly IScheduleTimeServiceProvider _scheduleTimeServiceProvider;
        private readonly IMapper _mapper;

        public ScheduleManager(
            IScheduleRepository scheduleRepository,
            IPlanRepository planRepository,
            IScheduleValidatorFactory scheduleValidatorFactory,
            IScheduleTimeServiceProvider scheduleTimeServiceProvider,
            IMapper mapper)
            : base(scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
            _planRepository = planRepository;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _scheduleTimeServiceProvider = scheduleTimeServiceProvider;
            _mapper = mapper;
        }


        public async Task<ValidationResult<string>> ValidateScheduleAsync(ScheduleModel scheduleModel)
        {
            if (scheduleModel == null)
            {
                throw new ArgumentNullException(nameof(scheduleModel));
            }

            var scheduleValidation = await _planRepository.GetScheduleValidationAsync(scheduleModel.PlanId);
            if (scheduleValidation == null)
            {
                throw new ObjectNotFoundException($"Plan with id={scheduleModel.PlanId} was not found");
            }

            var planTimeLines = await _planRepository.GetPlanTimeLinesAsync(scheduleModel.PlanId, scheduleValidation.PlanStartDateTime, scheduleValidation.PlanEndDateTime);
            var existingSchedulePeriods = new List<SchedulePeriod>();

            foreach (var p in planTimeLines)
            {
                foreach (var s in p.Schedules)
                {
                    var timeService= _scheduleTimeServiceProvider.CreateScheduleTimeService(s.Type);
                    var existingScheduleTime = _mapper.MergeInto<ScheduleTime>(p, s);
                    var existingSchedulePeriod = timeService.GetSchedulePeriod(existingScheduleTime);
                    existingSchedulePeriods.Add(existingSchedulePeriod);
                }
            }

            var scheduleTimeService = _scheduleTimeServiceProvider.CreateScheduleTimeService(scheduleModel.Type);
            var newScheduleTime = _mapper.MergeInto<ScheduleTime>(scheduleValidation, scheduleModel);
            var newSchedulePeriod = scheduleTimeService.GetSchedulePeriod(newScheduleTime);

            var validationContext = new ScheduleValidationContext()
            {
                ScheduleStartTime = scheduleModel.StartTime,
                ScheduleEndTime = scheduleModel.EndTime,
                AdPlayTime = scheduleModel.PlayTime,
                AdBreakTime = scheduleModel.BreakTime,
                ScheduleDayOfWeek = scheduleModel.DayOfWeek,
                ScheduleDate = scheduleModel.Date,
                ScheduleType = scheduleModel.Type,
                PlanStartDateTime = scheduleValidation.PlanStartDateTime,
                PlanEndDateTime = scheduleValidation.PlanEndDateTime,
                AdPoints = scheduleValidation.AdPoints.ToList(),
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
            var scheduleTimeService = _scheduleTimeServiceProvider.CreateScheduleTimeService(scheduleTime.ScheduleType);

            var timeOfAdsShowingBeforeUpdating = scheduleTimeService.GetTimeOfAdsShowing(scheduleTime);

            scheduleTime.ScheduleStartTime = updateModel.StartTime;
            scheduleTime.ScheduleEndTime = updateModel.EndTime;
            scheduleTime.AdBreakTime = updateModel.BreakTime;
            scheduleTime.ScheduleDayOfWeek = updateModel.DayOfWeek;
            scheduleTime.ScheduleDate = updateModel.Date;

            var timeOfAdsShowingAfterUpdating = scheduleTimeService.GetTimeOfAdsShowing(scheduleTime);

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
    }
}
