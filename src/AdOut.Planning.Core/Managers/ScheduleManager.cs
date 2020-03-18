using AdOut.Planning.Common.Collections;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Managers
{
    public class ScheduleManager : BaseManager<Schedule>, IScheduleManager
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAdPointRepository _adPointRepository;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;

        public ScheduleManager(
            IScheduleRepository scheduleRepository,
            IAdPointRepository adPointRepository,
            IScheduleValidatorFactory scheduleValidatorFactory)
            : base(scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
            _adPointRepository = adPointRepository;
            _scheduleValidatorFactory = scheduleValidatorFactory;
        }

        public async Task<ValidationResult<string>> ValidateScheduleAsync(ScheduleValidationModel scheduleModel)
        {
            if (scheduleModel == null)
            {
                throw new ArgumentNullException(nameof(scheduleModel));
            }

            var validationContext = new ScheduleValidationContext()
            {
                Schedule = scheduleModel.Schedule,
                Plan = scheduleModel.Plan
            };

            var adPointsValidations = await _adPointRepository.GetAdPointsValidationAsync(scheduleModel.AdPointIds, scheduleModel.Plan.StartDateTime, scheduleModel.Plan.EndDateTime);
            var adsPeriods = GenerateTimeLine(adPointsValidations);

            validationContext.AdPointValidations = adPointsValidations;
            validationContext.AdsPeriods = adsPeriods;

            var scheduleValidator = _scheduleValidatorFactory.CreateValidator();
            scheduleValidator.Validate(validationContext);

            var validationResult = new ValidationResult<string>()
            {
                Errors = validationContext.Errors
            };

            return validationResult;
        }

        private List<AdPeriod> GenerateTimeLine(List<AdPointValidation> adPoints)
        {
            var adsPeriods = new List<AdPeriod>();

            foreach (var adPoint in adPoints)
            {
                foreach (var plan in adPoint.Plans)
                {
                    var orderedPlanAds = plan.PlanAds.OrderBy(pa => pa.Order);
                    var ads = new CircleList<PlanAdValidation>(orderedPlanAds);

                    AdPeriod previosAdPeriod = null;
                    foreach (var schedule in plan.Schedules)
                    {
                        var adStartTime = TimeSpan.Zero;
                        if (previosAdPeriod == null)
                        {
                            adStartTime = schedule.StartTime;
                        }
                        else
                        {
                            adStartTime = previosAdPeriod.EndTime.Add(schedule.BreakTime);
                        }

                        var ad = ads.Next();
                        var adTimePlaying = TimeSpan.FromSeconds(ad.TimePlayingSec);
                        var adEndTime = adStartTime.Add(adTimePlaying);

                        var adPeriod = new AdPeriod()
                        {
                            AdPointLocation = adPoint.Location,
                            StartTime = adStartTime,
                            EndTime = adEndTime,
                            Date = schedule.Date,
                            DayOfWeek = schedule.DayOfWeek
                        };

                        previosAdPeriod = adPeriod;
                        adsPeriods.Add(adPeriod);
                    }
                }
            }

            return adsPeriods;
        }
    }
}
