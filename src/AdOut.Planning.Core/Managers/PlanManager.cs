using AdOut.Extensions.Exceptions;
using AdOut.Planning.Core.Mapping;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Enum;
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
    public class PlanManager : IPlanManager
    {
        private readonly IPlanRepository _planRepository;
        private readonly IPlanAdPointRepository _planAdPointRepository;
        private readonly IScheduleValidatorFactory _scheduleValidatorFactory;
        private readonly IScheduleTimeServiceProvider _scheduleTimeServiceProvider;
        private readonly IMapper _mapper;

        public PlanManager(
            IPlanRepository planRepository,
            IPlanAdPointRepository planAdPointRepository,
            IScheduleValidatorFactory scheduleValidatorFactory,
            IScheduleTimeServiceProvider scheduleTimeServiceProvider,
            IMapper mapper) 
        {
            _planRepository = planRepository;
            _planAdPointRepository = planAdPointRepository;
            _scheduleValidatorFactory = scheduleValidatorFactory;
            _scheduleTimeServiceProvider = scheduleTimeServiceProvider;
            _mapper = mapper;
        }

        public async Task<ValidationResult<string>> ValidatePlanExtensionAsync(string planId, DateTime newEndDate)
        {
            var planExtValidation = await _planRepository.GetPlanExtensionValidationAsync(planId);
            if (planExtValidation == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            var validationResult = new ValidationResult<string>();
            if (newEndDate <= planExtValidation.EndDateTime)
            {
                validationResult.Errors.Add($"New end date can't be less or equal than current end date");
                return validationResult;
            }

            var planAdPointsIds = planExtValidation.AdPoints.Select(ap => ap.Id).ToArray();
            var planTimeLines = await _planRepository.GetPlanTimeLinesAsync(planAdPointsIds, planExtValidation.EndDateTime, newEndDate);
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

            _planRepository.Update(plan);
        }

        public void Create(CreatePlanModel createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }

            var plan = new Plan()
            {
                Title = createModel.Title,
                StartDateTime = createModel.StartDateTime,
                EndDateTime = createModel.EndDateTime
            };

            _planRepository.Create(plan);

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

            _planRepository.Update(plan);
        }

        public async Task DeleteAsync(string planId)
        {
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            _planRepository.Delete(plan);
        }

        public Task<PlanDto> GetDtoByIdAsync(string planId)
        {
            return _planRepository.GetDtoByIdAsync(planId);
        }
    }
}
