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

        public async Task<List<PlanTimeLine>> GetPlansTimeLines(string adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var adPointPlans = await _planRepository.GetByAdPoint(adPointId, dateFrom, dateTo);

            var plansTimeLines = new List<PlanTimeLine>();
            foreach (var plan in adPointPlans)
            {
                var planTimeLine = new PlanTimeLine() { PlanId = plan.Id, PlanTitle = plan.Title };
                foreach (var schedule in plan.Schedules)
                {
                    var scheduleAdPeriods = _timeLineHelper.GetScheduleTimeLine(schedule);
                    planTimeLine.AdsPeriods.AddRange(scheduleAdPeriods);
                }

                plansTimeLines.Add(planTimeLine);
            }

            return plansTimeLines;
        }

        public async Task<List<AdPeriod>> GetPlanTimeLine(string planId)
        {
            var planSchedules = await _scheduleRepository.GetByPlanAsync(planId);
            var adPeriods = new List<AdPeriod>();

            foreach (var schedule in planSchedules)
            {
                var scheduleTimeLine = _timeLineHelper.GetScheduleTimeLine(schedule);
                adPeriods.AddRange(scheduleTimeLine);
            }

            return adPeriods;
        }

        //todo: delete userId from arguments
        public void Create(CreatePlanModel createModel, string userId)
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
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            var validationResult = new ValidationResult<string>();

            if (newEndDate <= plan.EndDateTime)
            {
                validationResult.Errors.Add($"New end date can't be less or equal than current end date");
                return validationResult;
            }

            var adPointsValidations = await _adPointRepository.GetAdPointsValidationAsync(planId, plan.EndDateTime, newEndDate);
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

                foreach (var schedule in adPoint.Schedules)
                {
                    var schdeduleAdsPeriods = _timeLineHelper.GetScheduleTimeLine(schedule);
                    adPointTime.AdPeriods.AddRange(schdeduleAdsPeriods);
                }

                adPointTimes.Add(adPointTime);
            }

            var scheduleTimeIntersectionValidator = _scheduleValidatorFactory.CreateChainOfValidators(ValidatorType.IntersectionTime);
            var planSchedules = await _scheduleRepository.GetByPlanAsync(planId);

            var validationContext = new ScheduleValidationContext()
            {
                AdPoints = adPointTimes,
                Plan = new SchedulePlan() { Type = plan.Type }
            };

            foreach (var schedule in planSchedules)
            {
                var scheduleAdsPeriods = _timeLineHelper.GetScheduleTimeLine(schedule);

                validationContext.Schedule = schedule;
                validationContext.NewAdsPeriods = scheduleAdsPeriods;

                scheduleTimeIntersectionValidator.Validate(validationContext);
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
