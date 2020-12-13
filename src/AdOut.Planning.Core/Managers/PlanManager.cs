using AdOut.Planning.Core.Mapping;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Services;
using AutoMapper;
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
        private readonly IUserService _userManager;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly IScheduleTimeServiceProvider _scheduleTimeServiceProvider;
        private readonly IMapper _mapper;

        public PlanManager(
            IPlanRepository planRepository,
            IPlanAdPointRepository planAdPointRepository,
            IUserService userManager,
            IScheduleValidatorFactory scheduleValidatorFactory,
            IScheduleTimeServiceProvider scheduleTimeServiceProvider,
            IMapper mapper) 
            : base(planRepository)
        {
            _planRepository = planRepository;
            _planAdPointRepository = planAdPointRepository;
            _userManager = userManager;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _scheduleTimeServiceProvider = scheduleTimeServiceProvider;
            _mapper = mapper;
        }

        public async Task<List<PlanPeriod>> GetPlansTimeLines(string adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var planTimeLines = await _planRepository.GetPlanTimeLinesAsync(adPointId, dateFrom, dateTo);
            var plansPeriods = new List<PlanPeriod>();

            foreach (var plan in planTimeLines)
            {
                var planPeriod = new PlanPeriod() { PlanId = plan.Id };
                foreach (var schedule in plan.Schedules)
                {
                    var timeService = _scheduleTimeServiceProvider.CreateScheduleTimeService(schedule.Type);
                    var scheduleTime = _mapper.MergeInto<ScheduleTime>(plan, schedule);
                    var schedulePeriod = timeService.GetSchedulePeriod(scheduleTime);
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
            var planExtValidation = await _planRepository.GetPlanExtensionValidation(planId);
            if (planExtValidation == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            var validationResult = new ValidationResult<string>();
            if (newEndDate <= planExtValidation.EndDateTime)
            {
                validationResult.Errors.Add($"New end date can't be less or equal with current end date");
                return validationResult;
            }

            var planTimeLines = await _planRepository.GetPlanTimeLinesAsync(planId, planExtValidation.EndDateTime, newEndDate);
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

            var validationContext = new ScheduleValidationContext()
            {
                PlanStartDateTime = planExtValidation.StartDateTime,
                PlanEndDateTime = planExtValidation.EndDateTime,
                AdPoints = planExtValidation.AdPoints.ToList(),
                ExistingSchedulePeriods = existingSchedulePeriods
            };

            var intersectionValidators = _scheduleValidatorFactory.CreateChainOfValidators(ValidatorType.IntersectionTime);
            foreach (var s in planExtValidation.Schedules)
            {
                var timeService = _scheduleTimeServiceProvider.CreateScheduleTimeService(s.Type);
                var newScheduleTime = _mapper.MergeInto<ScheduleTime>(planExtValidation, s);
                var newSchedulePeriod = timeService.GetSchedulePeriod(newScheduleTime);
                validationContext.NewSchedulePeriod = newSchedulePeriod;

                intersectionValidators.Validate(validationContext);
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
    }
}
