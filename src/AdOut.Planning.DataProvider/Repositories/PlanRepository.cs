using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<PlanExtensionValidation> GetPlanExtensionValidationAsync(string planId)
        {
            var query = Context.Plans.Where(p => p.Id == planId)
                                     .Select(p => new PlanExtensionValidation()
                                     {
                                         StartDateTime = p.StartDateTime,
                                         EndDateTime = p.EndDateTime,
                                         Schedules = p.Schedules.Select(s => new ScheduleDto()
                                         {
                                             Type = s.Type,
                                             StartTime = s.StartTime,
                                             EndTime = s.EndTime,
                                             BreakTime = s.BreakTime,
                                             PlayTime = s.PlayTime,
                                             Date = s.Date,
                                             DayOfWeek = s.DayOfWeek
                                         }),
                                         AdPoints = p.PlanAdPoints.Select(pap => new AdPointValidation()
                                         {
                                             Id = pap.AdPointId,
                                             StartWorkingTime = pap.AdPoint.StartWorkingTime,
                                             EndWorkingTime = pap.AdPoint.EndWorkingTime,
                                             DaysOff = pap.AdPoint.DaysOff.Select(doff => doff.DayOfWeek)
                                         })
                                     });

            return query.SingleOrDefaultAsync();
        }

        public Task<ScheduleValidation> GetScheduleValidationAsync(string planId)
        {
            var query = Context.Plans.Where(p => p.Id == planId)
                                     .Select(p => new ScheduleValidation()
                                     {
                                         PlanStartDateTime = p.StartDateTime,
                                         PlanEndDateTime = p.EndDateTime,
                                         AdPoints = p.PlanAdPoints.Select(pap => new AdPointValidation()
                                         {
                                             Id = pap.AdPointId,
                                             StartWorkingTime = pap.AdPoint.StartWorkingTime,
                                             EndWorkingTime = pap.AdPoint.EndWorkingTime,             
                                             DaysOff = pap.AdPoint.DaysOff.Select(doff => doff.DayOfWeek)
                                         })
                                     });

            return query.SingleOrDefaultAsync();
        }

        public Task<List<PlanTimeLine>> GetPlanTimeLinesAsync(string[] adPointIds, DateTime planStart, DateTime planEnd)
        {
            //todo: do I need a disctinct in DaysOff?

            var query = Context.Plans.Where(p => p.PlanAdPoints.Any(pap => adPointIds.Contains(pap.AdPointId)) &&
                                                 planStart < p.EndDateTime &&
                                                 p.StartDateTime < planEnd)
                               .Select(p => new PlanTimeLine()
                               {
                                   Id = p.Id,
                                   StartDateTime = p.StartDateTime,
                                   EndDateTime = p.EndDateTime,
                                   Schedules = p.Schedules.Select(s => new ScheduleDto()
                                   {
                                       Type = s.Type,
                                       StartTime = s.StartTime,
                                       EndTime = s.EndTime,
                                       BreakTime = s.BreakTime,
                                       PlayTime = s.PlayTime,
                                       Date = s.Date,
                                       DayOfWeek = s.DayOfWeek
                                   }),
                                   AdPointsDaysOff = p.PlanAdPoints
                                       .SelectMany(pap => pap.AdPoint.DaysOff.Select(doff => doff.DayOfWeek))
                               });

            return query.ToListAsync();
        }

        public Task<PlanPriceDto> GetPlanPriceAsync(string planId)
        {
            var query = Context.Plans.Where(p => p.Id == planId)
                                     .Select(p => new PlanPriceDto()
                                     {
                                         StartDateTime = p.StartDateTime,
                                         EndDateTime = p.EndDateTime,
                                         AdPoints = p.PlanAdPoints.Select(pap => new AdPointDto() 
                                         {
                                            DaysOff = pap.AdPoint.DaysOff.Select(doff => doff.DayOfWeek),
                                            Tariffs = pap.AdPoint.Tariffs.Select(t => new TariffDto()
                                            {
                                                StartTime = t.StartTime,
                                                EndTime = t.EndTime,
                                                PriceForMinute = t.PriceForMinute
                                            })
                                         })   
                                     });

            return query.SingleOrDefaultAsync();
        }

        public Task<PlanDto> GetDtoByIdAsync(string planId)
        { 
            var query = Context.Plans.Where(p => p.Id == planId)
                               .Select(p => new PlanDto()
                                {
                                    Title = p.Title,
                                    UserId = p.Creator,
                                    Status = p.Status,
                                    StartDateTime = p.StartDateTime,
                                    EndDateTime = p.EndDateTime,
                                    Schedules = p.Schedules.Select(s => new ScheduleDto()
                                    {
                                        Type = s.Type,
                                        StartTime = s.StartTime,
                                        EndTime = s.EndTime,
                                        BreakTime = s.BreakTime,
                                        PlayTime = s.PlayTime,
                                        Date = s.Date,
                                        DayOfWeek = s.DayOfWeek
                                    }),
                                    Ads = p.PlanAds.Select(pa => new AdListDto()
                                    {
                                        Id = pa.Ad.Id,
                                        Title = pa.Ad.Title,
                                        Status = pa.Ad.Status,
                                        ContentType = pa.Ad.ContentType,
                                        PreviewPath = pa.Ad.PreviewPath
                                    }),
                                    AdPoints = p.PlanAdPoints.Select(pap => new AdPointDto()
                                    {
                                        Id = pap.AdPointId,
                                        Location = pap.AdPoint.Location,
                                        DaysOff = pap.AdPoint.DaysOff.Select(doff => doff.DayOfWeek)
                                    })
                               });

            return query.SingleOrDefaultAsync();
        }
    }
}
