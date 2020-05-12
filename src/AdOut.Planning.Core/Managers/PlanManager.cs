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
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Managers
{
    public class PlanManager : BaseManager<Plan>, IPlanManager
    {
        private readonly IPlanRepository _planRepository;
        private readonly IAdPointRepository _adPointRepository; 
        private readonly IPlanAdPointRepository _planAdPointRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly ITimeLineHelper _timeLineHelper;

        public PlanManager(
            IPlanRepository planRepository,
            IAdPointRepository adPointRepository,
            IPlanAdPointRepository planAdPointRepository,
            IScheduleRepository scheduleRepository,
            IScheduleValidatorFactory scheduleValidatorFactory,
            ITimeLineHelper timeLineHelper) 
            : base(planRepository)
        {
            _planRepository = planRepository;
            _adPointRepository = adPointRepository;
            _planAdPointRepository = planAdPointRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _timeLineHelper = timeLineHelper;
        }

        public async Task CreateAsync(CreatePlanModel createModel, string userId)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }

            var plan = new Plan()
            {
                UserId = userId,
                Title = createModel.Title,
                Type = createModel.Type,
                StartDateTime = createModel.StartDateTime,
                EndDateTime = createModel.EndDateTime,
                AdsTimePlaying = createModel.AdsTimePlaying
            };

            Create(plan);

            foreach (var adPointId in createModel.AdPointsIds)
            {
                var adPoint = await _adPointRepository.GetByIdAsync(adPointId);
                if (adPoint == null)
                {
                    throw new ObjectNotFoundException($"AdPoint with id={adPointId} was not found");
                }

                var planAdPoint = new PlanAdPoint()
                {
                    AdPoint = adPoint,
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

        public async Task DeleteAsync(int planId)
        {
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            Delete(plan);
        }

        public Task<PlanDto> GetDtoByIdAsync(int planId)
        {
            return _planRepository.GetDtoByIdAsync(planId);
        }

        public Task<Plan> GetByIdAsync(int planId)
        {
            return _planRepository.GetByIdAsync(planId);
        }

        public async Task<ValidationResult<string>> ValidatePlanExtensionAsync(int planId, DateTime newEndDate)
        {
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            var validationResult = new ValidationResult<string>();

            if (plan.EndDateTime >= newEndDate)
            {
                validationResult.Errors.Add($"New end date can't be less or equal than current end date");
                return validationResult;
            }

            var adPointsIds = await _planAdPointRepository.GetAdPointsIds(planId);
            var adPointsPlans = await _planRepository.GetByAdPoints(adPointsIds.ToArray(), plan.EndDateTime, newEndDate);

            var existingAdsPeriods = new List<AdPeriod>();
            foreach (var apPlan in adPointsPlans)
            {
                foreach (var schedule in apPlan.Schedules)
                {
                    var schdeduleAdsPeriods = _timeLineHelper.GetScheduleTimeLine(schedule, apPlan.AdsTimePlaying);
                    existingAdsPeriods.AddRange(schdeduleAdsPeriods);
                }
            }

            var scheduleTimeIntersectionValidator = _scheduleValidatorFactory.CreateChainOfValidators(ValidatorType.IntersectionTime);
            var planSchedules = await _scheduleRepository.GetByPlanAsync(planId);

            var validationContext = new ScheduleValidationContext()
            {
                ExistingAdsPeriods = existingAdsPeriods,
                Plan = new SchedulePlan() { Type = plan.Type }
            };

            foreach (var schedule in planSchedules)
            {
                var scheduleAdsPeriods = _timeLineHelper.GetScheduleTimeLine(schedule, plan.AdsTimePlaying);

                validationContext.Schedule = schedule;
                validationContext.NewAdsPeriods = scheduleAdsPeriods;

                scheduleTimeIntersectionValidator.Validate(validationContext);
            }

            validationResult.Errors.AddRange(validationContext.Errors);
            return validationResult;
        }

        public async Task ExtendPlanAsync(int planId, DateTime newEndDate)
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
