using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class AdRepository : BaseRepository<Ad>, IAdRepository
    {
        public AdRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<List<AdListDto>> GetAds(Expression<Func<Ad, bool>> predicate)
        {
            var query = Context.Ads.Where(predicate)
                               .OrderByDescending(a => a.AddedDate)
                               .Select(a => new AdListDto()
                               {
                                   Id = a.Id,
                                   Title = a.Title,
                                   ContentType = a.ContentType,
                                   Status = a.Status,
                                   PreviewPath = a.PreviewPath
                               });

            return query.ToListAsync();
        }

        public Task<AdDto> GetDtoByIdAsync(string adId)
        {
            var query = Context.Ads.Where(a => a.Id == adId)
                               .Select(a => new AdDto()
                               {
                                   UserId = a.UserId,
                                   Title = a.Title,
                                   Path = a.Path,
                                   ContentType = a.ContentType,
                                   Status = a.Status,
                                   AddedDate = a.AddedDate,
                                   ConfirmationDate = a.ConfirmationDate,
                                   Plans = a.PlanAds.Select(pa => new AdPlanDto()
                                   {
                                       Id = pa.Plan.Id,
                                       Title = pa.Plan.Title,
                                       Status = pa.Plan.Status
                                   })
                               });

            return query.SingleOrDefaultAsync();
        }
    }
}
