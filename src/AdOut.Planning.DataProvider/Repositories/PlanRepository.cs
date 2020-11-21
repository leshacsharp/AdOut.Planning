﻿using AdOut.Planning.Model.Database;
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

        public Task<List<AdPointPlanDto>> GetByAdPointAsync(string adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var query = Context.Plans.Where(p => p.PlanAdPoints.Any(ap => ap.AdPointId == adPointId) &&
                                                 p.StartDateTime >= dateFrom &&
                                                 p.EndDateTime <= dateTo)
                               .Select(p => new AdPointPlanDto()
                               {
                                   Id = p.Id,
                                   Title = p.Title,
                                   Schedules = p.Schedules.Select(s => new ScheduleDto() 
                                   {
                                       StartTime = s.StartTime,
                                       EndTime = s.EndTime,
                                       BreakTime = s.BreakTime,
                                       PlayTime = s.PlayTime,
                                       DayOfWeek = s.DayOfWeek,
                                       Date = s.Date
                                   })
                               });

            return query.ToListAsync();
        }

        public Task<PlanDto> GetDtoByIdAsync(string planId)
        { 
            var query = Context.Plans.Where(p => p.Id == planId)
                               .Select(p => new PlanDto()
                                {
                                    Title = p.Title,
                                    UserId = p.UserId,
                                    Type = p.Type,
                                    Status = p.Status,
                                    StartDateTime = p.StartDateTime,
                                    EndDateTime = p.EndDateTime,
                                    Schedules = p.Schedules.Select(s => new ScheduleDto()
                                    {
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
