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
        private readonly IAdPointRepository _adPointRepository;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly IScheduleTimeHelperProvider _scheduleTimeHelperProvider;
        private readonly ITimeLineHelper _timeLineHelper;

        public ScheduleManager(
            IScheduleRepository scheduleRepository,
            IPlanRepository planRepository,
            IAdPointRepository adPointRepository,
            IScheduleValidatorFactory scheduleValidatorFactory,
            IScheduleTimeHelperProvider scheduleTimeHelperProvider,
            ITimeLineHelper timeLineHelper)
            : base(scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
            _planRepository = planRepository;
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

            //todo: make something with that!!!
            var scheduleDto = new ScheduleDto()
            {
                StartTime = scheduleModel.StartTime,
                EndTime = scheduleModel.EndTime,
                PlayTime = scheduleModel.PlayTime,
                BreakTime = scheduleModel.BreakTime,
                DayOfWeek = scheduleModel.DayOfWeek,
                Date = scheduleModel.Date
            };

            var adPointsValidations = await _adPointRepository.GetAdPointsValidationAsync(plan.Id, plan.StartDateTime, plan.EndDateTime);
            var adPointTimes = new List<AdPointTime>();

            foreach (var adPoint in adPointsValidations)
            {
                var adPointTime = new AdPointTime()
                {
                    Location = adPoint.Location,
                    StartWorkingTime = adPoint.StartWorkingTime,
                    EndWorkingTime = adPoint.EndWorkingTime,
                    DaysOff = adPoint.DaysOff.ToList()
                };

                foreach (var app in adPoint.Plans)
                {
                    foreach (var s in app.Schedules)
                    {
                        //todo: think about filtering ads periods by dates!
                        var schdeduleAdsPeriod = _timeLineHelper.GetScheduleTimeLine(s, app.StartDateTime, app.EndDateTime);
                        adPointTime.AdPeriods.Add(schdeduleAdsPeriod);
                    }
                }
       
                adPointTimes.Add(adPointTime);
            }

            var newAdPeriod = _timeLineHelper.GetScheduleTimeLine(scheduleDto, plan.StartDateTime, plan.EndDateTime);

            var validationContext = new ScheduleValidationContext()
            {
                Schedule = scheduleDto,
                AdPoints = adPointTimes,
                NewAdPeriod = newAdPeriod,
                Plan = new SchedulePlan()
                {
                    Type = plan.Type,
                    StartDateTime = plan.StartDateTime,
                    EndDateTime = plan.EndDateTime
                }
            };

            var chainOfValidator = _scheduleValidatorFactory.CreateChainOfAllValidators();
            chainOfValidator.Validate(validationContext);

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

            var scheduleInfo = await _scheduleRepository.GetScheduleInfoAsync(updateModel.ScheduleId);
            var scheduleTimeHelper = _scheduleTimeHelperProvider.CreateScheduleTimeHelper(scheduleInfo.PlanType);

            var timeOfAdsShowingBeforeUpdating = scheduleTimeHelper.GetTimeOfAdsShowing(scheduleInfo);

            scheduleInfo.ScheduleStartTime = updateModel.StartTime;
            scheduleInfo.ScheduleEndTime = updateModel.EndTime;
            scheduleInfo.AdBreakTime = updateModel.BreakTime;
            scheduleInfo.ScheduleDayOfWeek = updateModel.DayOfWeek;
            scheduleInfo.ScheduleDate = updateModel.Date;

            var timeOfAdsShowingAfterUpdating = scheduleTimeHelper.GetTimeOfAdsShowing(scheduleInfo);

            if (timeOfAdsShowingAfterUpdating > timeOfAdsShowingBeforeUpdating)
            {
                throw new BadRequestException(ValidationMessages.Schedule.TimeIsIncreased);
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
