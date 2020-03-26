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

        public async Task<ValidationResult<string>> ValidateScheduleWithTempPlanAsync(TempScheduleValidationModel scheduleModel)
        {
            if (scheduleModel == null)
            {
                throw new ArgumentNullException(nameof(scheduleModel));
            }

            var adPointsValidations = await _adPointRepository.GetAdPointsValidationAsync(scheduleModel.AdPointIds, scheduleModel.Plan.StartDateTime, scheduleModel.Plan.EndDateTime);
            var existingsAdsPeriods = GetAdPointsAdsPeriods(adPointsValidations);

            foreach (var schedule in scheduleModel.Plan.Schedules)
            {
                var tempAdPeriods = GetScheduleTimeLine(schedule, scheduleModel.Plan.AdsTimePlaying);
                existingsAdsPeriods.AddRange(tempAdPeriods);
            }

            var newAdsPeriods = GetScheduleTimeLine(scheduleModel.Schedule, scheduleModel.Plan.AdsTimePlaying);

            var validationContext = new ScheduleValidationContext()
            {
                Schedule = scheduleModel.Schedule,
                Plan = scheduleModel.Plan,
                AdPointsValidations = adPointsValidations,
                ExistingAdsPeriods = existingsAdsPeriods,
                NewAdsPeriods = newAdsPeriods
            };

            var scheduleValidator = _scheduleValidatorFactory.CreateValidator();
            scheduleValidator.Validate(validationContext);

            var validationResult = new ValidationResult<string>()
            {
                Errors = validationContext.Errors
            };

            return validationResult;
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

            var scheduleDto = new ScheduleDto()
            {
                StartTime = scheduleModel.StartTime,
                EndTime = scheduleModel.EndTime,
                BreakTime = scheduleModel.BreakTime,
                DayOfWeek = scheduleModel.DayOfWeek,
                Date = scheduleModel.Date
            };
            
            var adPointsIds = await _planRepository.GetAdPointsIds(plan.Id);
            var adPointsValidations = await _adPointRepository.GetAdPointsValidationAsync(adPointsIds.ToArray(), plan.StartDateTime, plan.EndDateTime);

            var existingsAdsPeriods = GetAdPointsAdsPeriods(adPointsValidations);
            var newAdsPeriods = GetScheduleTimeLine(scheduleDto, plan.AdsTimePlaying);

            var validationContext = new ScheduleValidationContext()
            {
                Schedule = scheduleDto,
                AdPointsValidations = adPointsValidations,
                ExistingAdsPeriods = existingsAdsPeriods,
                NewAdsPeriods = newAdsPeriods
            };

            var scheduleValidator = _scheduleValidatorFactory.CreateValidator();
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
            var adPointPlans = await _planRepository.GetByAdPoint(adPointId, dateFrom, dateTo);

            foreach (var plan in adPointPlans)
            {
                var planTimeLine = new PlanTimeLine() { PlanId = plan.Id };     
                foreach (var schedule in plan.Schedules)
                {
                    var scheduleAdPeriods = GetScheduleTimeLine(schedule, plan.AdsTimePlaying);
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
                throw new BadRequestException(ScheduleValidationMessages.ScheduleTimeIsIncreased);
            }

            Update(schedule);
        }


        private List<AdPeriod> GetAdPointsAdsPeriods(List<AdPointValidation> adPointsValidations)
        {
            var adsPeriods = new List<AdPeriod>();

            foreach (var adPoint in adPointsValidations)
            {
                foreach (var plan in adPoint.Plans)
                {
                    foreach (var schedule in plan.Schedules)
                    {
                        var schdeduleAdsPeriods = GetScheduleTimeLine(schedule, plan.AdsTimePlaying);
                        adsPeriods.AddRange(schdeduleAdsPeriods);
                    }
                }
            }

            return adsPeriods;
        }

        private List<AdPeriod> GetScheduleTimeLine(ScheduleDto schedule, TimeSpan adsTimePlaying)
        {
            var adsPeriods = new List<AdPeriod>();

            AdPeriod currentAdPeriod = null;
            var adTimeWithBreak = adsTimePlaying + schedule.BreakTime;

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

            return adsPeriods;
        }  
    }
}
