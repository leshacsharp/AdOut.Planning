using AdOut.Extensions.Exceptions;
using AdOut.Planning.Core.Mapping;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
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
    public class ScheduleManager : IScheduleManager
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

            var planAdPointsIds = scheduleValidation.AdPoints.Select(ap => ap.Id).ToArray();
            var planTimeLines = await _planRepository.GetPlanTimeLinesAsync(planAdPointsIds, scheduleValidation.PlanStartDateTime, scheduleValidation.PlanEndDateTime);
            var existingSchedulePeriods = new List<SchedulePeriod>();

            foreach (var p in planTimeLines)
            {
                foreach (var s in p.Schedules)
                {
                    var timeService = _scheduleTimeServiceProvider.CreateScheduleTimeService(s.Type);
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

            _scheduleRepository.Create(schedule);
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

            var timeOfAdsShowingAfterUpdating = scheduleTimeService.GetTimeOfAdsShowing(scheduleTime);

            if (timeOfAdsShowingAfterUpdating > timeOfAdsShowingBeforeUpdating)
            {
                //todo: probably need to give this possibility fot extra money
                throw new UnprocessableEntityException(ValidationMessages.Schedule.TimeIncreased);
            }

            schedule.StartTime = updateModel.StartTime;
            schedule.EndTime = updateModel.EndTime;
            schedule.BreakTime = updateModel.BreakTime;

            _scheduleRepository.Update(schedule);
        }

        public async Task<double> CalculateSchedulePriceAsync(ScheduleModel schedule)
        {
            var planPrice = await _planRepository.GetPlanPriceAsync(schedule.PlanId);
            if (planPrice == null)
            {
                throw new ObjectNotFoundException($"Plan with id={schedule.PlanId} was not found");
            }

            var timeService = _scheduleTimeServiceProvider.CreateScheduleTimeService(schedule.Type);
            var scheduleTime = _mapper.MergeInto<ScheduleTime>(planPrice, schedule);
            var schedulePeriod = timeService.GetSchedulePeriod(scheduleTime);

            var adPointsTariffs = planPrice.AdPoints.SelectMany(ap => ap.Tariffs).ToList();
            var schedulePriceForDay = 0d;

            foreach (var timeRange in schedulePeriod.TimeRanges)
            {
                foreach (var tariff in adPointsTariffs)
                {
                    if (timeRange.IsInterescted(tariff.StartTime, tariff.EndTime))
                    {
                        //todo: check the logic of right/left intersection
                        var minutesInTariff = 0d;
                        if (timeRange.IsRightIntersected(tariff.StartTime, tariff.EndTime))
                        {
                            minutesInTariff = (timeRange.End - tariff.StartTime).TotalMinutes;
                        }
                        else if (timeRange.IsLeftIntersected(tariff.StartTime, tariff.EndTime))
                        {
                            minutesInTariff = (tariff.EndTime - timeRange.Start).TotalMinutes;
                        }
                        else
                        {
                            minutesInTariff = (timeRange.End - timeRange.Start).TotalMinutes;
                        }

                        schedulePriceForDay += minutesInTariff * tariff.PriceForMinute;
                    }
                }
            }

            var price = schedulePeriod.Dates.Count * schedulePriceForDay;
            return price;
        }
    }
}
