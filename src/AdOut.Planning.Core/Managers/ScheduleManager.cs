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
        private readonly IAdPointRepository _adPointRepository;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly IScheduleTimeHelperProvider _scheduleTimeHelperProvider;

        public ScheduleManager(
            IScheduleRepository scheduleRepository,
            IPlanRepository planRepository,
            IAdPointRepository adPointRepository,
            IScheduleValidatorFactory scheduleValidatorFactory,
            IScheduleTimeHelperProvider scheduleTimeHelperProvider)
            : base(scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
            _planRepository = planRepository;
            _adPointRepository = adPointRepository;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _scheduleTimeHelperProvider = scheduleTimeHelperProvider;
        }

        public async Task<ValidationResult<string>> ValidateScheduleWithTempPlanAsync(ScheduleWithPlanValidationModel scheduleModel)
        {
            if (scheduleModel == null)
            {
                throw new ArgumentNullException(nameof(scheduleModel));
            }

            var validationContext = new ScheduleValidationContext()
            {
                Schedule = scheduleModel.Schedule,
                Plan = scheduleModel.TempPlan
            };

            var adsPeriods = new List<AdPeriod>();
            var adPointsValidations = await _adPointRepository.GetAdPointsValidationAsync(scheduleModel.AdPointIds, scheduleModel.TempPlan.StartDateTime, scheduleModel.TempPlan.EndDateTime);

            foreach (var adPoint in adPointsValidations)
            {
                foreach (var plan in adPoint.Plans)
                {
                    var planAdsPeriods = GenerateTimeLine(plan.Schedules, plan.AdsTimePlaying);
                    adsPeriods.AddRange(planAdsPeriods);
                }
            }

            var tempAdPeriods = GenerateTimeLine(scheduleModel.TempPlan.Schedules, scheduleModel.TempPlan.AdsTimePlaying);
            adsPeriods.AddRange(tempAdPeriods);

            validationContext.AdPointValidations = adPointsValidations;
            validationContext.AdPointAdsPeriods = adsPeriods;

            var scheduleValidator = _scheduleValidatorFactory.CreateValidator();
            scheduleValidator.Validate(validationContext);

            var validationResult = new ValidationResult<string>()
            {
                Errors = validationContext.Errors
            };

            return validationResult;
        }

        //public async Task<ValidationResult<string>> ValidateScheduleAsync(ScheduleValidationModel scheduleModel)
        //{
        //    if (scheduleModel == null)
        //    {
        //        throw new ArgumentNullException(nameof(scheduleModel));
        //    }

        //    var plan = await _planRepository.GetByIdAsync(scheduleModel.PlanId);
        //    plan.
        //    if (plan == null)
        //    {
        //        throw new ObjectNotFoundException($"Plan with id={scheduleModel.PlanId} was not found");
        //    }

        //    var validationContext = new ScheduleValidationContext()
        //    {
        //        Schedule = scheduleModel.Schedule,
        //        Plan = scheduleModel.TempPlan
        //    };

        //    var adsPeriods = new List<AdPeriod>();
        //    var adPointsValidations = await _adPointRepository.GetAdPointsValidationAsync(scheduleModel.AdPointIds, scheduleModel.TempPlan.StartDateTime, scheduleModel.TempPlan.EndDateTime);

        //    foreach (var adPoint in adPointsValidations)
        //    {
        //        foreach (var plan in adPoint.Plans)
        //        {
        //            var planAdsPeriods = GenerateTimeLine(plan.Schedules, plan.AdsTimePlaying);
        //            adsPeriods.AddRange(planAdsPeriods);
        //        }
        //    }


        //    validationContext.AdPointValidations = adPointsValidations;
        //    validationContext.AdPointAdsPeriods = adsPeriods;

        //    var scheduleValidator = _scheduleValidatorFactory.CreateValidator();
        //    scheduleValidator.Validate(validationContext);

        //    var validationResult = new ValidationResult<string>()
        //    {
        //        Errors = validationContext.Errors
        //    };

        //    return validationResult;
        //}

        public async Task CreateAsync(CreateScheduleModel createModel)
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
                PlanId = plan.Id
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
                throw new BadRequestException(ScheduleValidationMessages.ScheduleTimeIsIncreased);
            }

            Update(schedule);
        }

        
        //todo: make unit tests
        private List<AdPeriod> GenerateTimeLine(IEnumerable<ScheduleValidation> schedules, TimeSpan adsTimePlaying)
        {
            var adsPeriods = new List<AdPeriod>();

            foreach (var schedule in schedules)
            {
                AdPeriod currentAdPeriod = null;
                var adTimeWithBreak = schedule.BreakTime + adsTimePlaying;

                while (currentAdPeriod.EndTime + adTimeWithBreak <= schedule.EndTime)
                {
                    var adStartTime = TimeSpan.Zero;
                    if (currentAdPeriod == null)
                    {
                        adStartTime = schedule.StartTime;
                    }
                    else
                    {
                        adStartTime = currentAdPeriod.EndTime.Add(schedule.BreakTime);
                    }

                    var adEndTime = adStartTime.Add(adsTimePlaying);
                    var adPeriod = new AdPeriod()
                    {
                        StartTime = adStartTime,
                        EndTime = adEndTime,
                        Date = schedule.Date,
                        DayOfWeek = schedule.DayOfWeek
                    };

                    currentAdPeriod = adPeriod;
                    adsPeriods.Add(adPeriod);
                }
            }

            return adsPeriods;
        }
    }
}
