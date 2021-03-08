using AdOut.Extensions.Communication;
using AdOut.Extensions.Context;
using AdOut.Extensions.Exceptions;
using AdOut.Planning.Core.Mapping;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.EventHandlers
{
    public class PlanAcceptedConsumer : BaseConsumer<PlanAcceptedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        public PlanAcceptedConsumer(
            IServiceScopeFactory serviceScopeFactory, 
            IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        protected override async Task HandleAsync(PlanAcceptedEvent deliveredEvent)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var planRepository = scope.ServiceProvider.GetRequiredService<IPlanRepository>();
            var planTimeRepository = scope.ServiceProvider.GetRequiredService<IPlanTimeRepository>();
            var scheduleTimeServiceProvider = scope.ServiceProvider.GetRequiredService<IScheduleTimeServiceProvider>();
            var dbContext = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            var commitProvider = scope.ServiceProvider.GetRequiredService<ICommitProvider>();

            var planTimeDto = await planRepository.GetPlanTimeAsync(deliveredEvent.PlanId);
            if (planTimeDto == null)
            {
                throw new ObjectNotFoundException($"Plan with id={deliveredEvent.PlanId} was not found (EventId={deliveredEvent.EventId})");
            }

            var schedulePeriods = new List<SchedulePeriod>();
            foreach (var s in planTimeDto.Schedules)
            {
                var timeService = scheduleTimeServiceProvider.CreateScheduleTimeService(s.Type);
                var scheduleTime = _mapper.MergeInto<ScheduleTime>(planTimeDto, s);
                var schedulePeriod = timeService.GetSchedulePeriod(scheduleTime);
                schedulePeriods.Add(schedulePeriod);
            }

            var planTime = _mapper.Map<PlanTime>(planTimeDto);
            planTime.Schedules = schedulePeriods;
            planTimeRepository.Create(planTime);

            var planEntity = _mapper.Map<Plan>(planTimeDto);
            dbContext.Attach(planEntity);
            planEntity.Status = Planning.Model.Enum.PlanStatus.Accepted;
            planRepository.Update(planEntity);
            await commitProvider.SaveChangesAsync();
        }
    }
}
