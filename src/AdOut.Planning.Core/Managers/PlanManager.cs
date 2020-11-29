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
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Managers
{
    public class PlanManager : BaseManager<Plan>, IPlanManager
    {
        private readonly IPlanRepository _planRepository;
        private readonly IPlanAdPointRepository _planAdPointRepository;
        private readonly IUserManager _userManager;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly IScheduleTimeHelperProvider _scheduleTimeHelperProvider;

        public PlanManager(
            IPlanRepository planRepository,
            IPlanAdPointRepository planAdPointRepository,
            IUserManager userManager,
            IScheduleValidatorFactory scheduleValidatorFactory,
            IScheduleTimeHelperProvider scheduleTimeHelperProvider) 
            : base(planRepository)
        {
            _planRepository = planRepository;
            _planAdPointRepository = planAdPointRepository;
            _userManager = userManager;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _scheduleTimeHelperProvider = scheduleTimeHelperProvider;
        }

        public async Task<List<PlanPeriod>> GetPlansTimeLines(string adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var planTimeLines = await _planRepository.GetPlanTimeLinesAsync(adPointId, dateFrom, dateTo);
            var plansPeriods = new List<PlanPeriod>();

            foreach (var plan in planTimeLines)
            {
                var planPeriod = new PlanPeriod() { PlanId = plan.Id, PlanTitle = plan.Title };
                foreach (var schedule in plan.Schedules)
                {
                    var timeHelper = _scheduleTimeHelperProvider.CreateScheduleTimeHelper(schedule.Type);
                    var scheduleTime = MapToScheduleTime(plan, schedule);
                    var schedulePeriod = timeHelper.GetScheduleTimeLine(scheduleTime);
                    planPeriod.SchedulePeriods.Add(schedulePeriod);
                }

                plansPeriods.Add(planPeriod);
            }

            return plansPeriods;
        }

        public void Create(CreatePlanModel createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }

            var plan = new Plan()
            {
                UserId = _userManager.GetUserId(),
                Title = createModel.Title,
                StartDateTime = createModel.StartDateTime,
                EndDateTime = createModel.EndDateTime
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

        public async Task DeleteAsync(string planId)
        {
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            Delete(plan);
        }

        public Task<PlanDto> GetDtoByIdAsync(string planId)
        {
            return _planRepository.GetDtoByIdAsync(planId);
        }

        public async Task<Plan> GetByIdAsync(string planId)
        {
            return await _planRepository.GetByIdAsync(planId);
        }

        public async Task<ValidationResult<string>> ValidatePlanExtensionAsync(string planId, DateTime newEndDate)
        {
            var plan = await _planRepository.GetPlanExtensionValidation(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            var validationResult = new ValidationResult<string>();
            if (newEndDate <= plan.EndDateTime)
            {
                validationResult.Errors.Add($"New end date can't be less or equal with current end date");
                return validationResult;
            }

            var planValidations = await _planRepository.GetPlanValidationsAsync(planId, plan.EndDateTime, newEndDate);
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

            var validationContext = new ScheduleValidationContext()
            {
                PlanStartDateTime = plan.StartDateTime,
                PlanEndDateTime = plan.EndDateTime,
                AdPoints = plan.AdPoints.ToList(),
                ExistingSchedulePeriods = existingSchedulePeriods
            };

            var timeIntersectionValidators = _scheduleValidatorFactory.CreateChainOfValidators(ValidatorType.IntersectionTime);
            foreach (var s in plan.Schedules)
            {
                var timeHelper = _scheduleTimeHelperProvider.CreateScheduleTimeHelper(s.Type);
                var scheduleTime = MapToScheduleTime(plan, s);
                var newSchedulePeriod = timeHelper.GetScheduleTimeLine(scheduleTime);
                validationContext.NewSchedulePeriod = newSchedulePeriod;

                timeIntersectionValidators.Validate(validationContext);
            }

            validationResult.Errors.AddRange(validationContext.Errors);
            return validationResult;
        }

        public async Task ExtendPlanAsync(string planId, DateTime newEndDate)
        {
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            plan.EndDateTime = newEndDate;

            Update(plan);
        }

        //todo: create mapper for these entities
        private ScheduleTime MapToScheduleTime(PlanTimeLine plan, ScheduleDto schedule)
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

        private ScheduleTime MapToScheduleTime(PlanExtensionValidation plan, ScheduleDto schedule)
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
