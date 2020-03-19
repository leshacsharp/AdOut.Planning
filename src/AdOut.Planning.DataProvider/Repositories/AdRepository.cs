using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Enum;
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
            var query = from ad in Context.Ads.Where(predicate)
                        orderby ad.AddedDate descending
                        select new AdListDto()
                        {
                            Id = ad.Id,
                            Title = ad.Title,
                            ContentType = ad.ContentType,
                            Status = ad.Status,
                            PreviewPath = ad.PreviewPath
                        };

            return query.ToListAsync();
        }

        public Task<Ad> GetByIdAsync(int adId)
        {
            var query = from a in Context.Ads
                        where a.Id == adId
                        select a;

            return query.SingleOrDefaultAsync();
        }

        public async Task<AdDto> GetDtoByIdAsync(int adId)
        {
            var query = from a in Context.Ads

                        join pa in Context.PlanAds on a.Id equals pa.AdId into planAdsJoin
                        from pa in planAdsJoin.DefaultIfEmpty()

                        join p in Context.Plans on pa.PlanId equals p.Id into plansJoin
                        from p in plansJoin.DefaultIfEmpty()

                        join pap in Context.PlanAdPoints on p.Id equals pap.PlanId into planAdPointsJoin
                        from pap in planAdPointsJoin.DefaultIfEmpty()

                        join ap in Context.AdPoints on pap.AdPointId equals ap.Id into adPointsJoin
                        from ap in adPointsJoin.DefaultIfEmpty()

                        where a.Id == adId

                        select new
                        {
                            a.Id,
                            a.Title,
                            a.Path,
                            a.ContentType,
                            a.Status,
                            a.AddedDate,
                            a.ConfirmationDate,

                            PlanId = p != null ? p.Id : (int?)null,
                            PlanTitle = p != null ? p.Title : null,
                            PlanStatus = p != null ? p.Status : PlanStatus.Off,

                            AdPointId = ap != null ? ap.Id : (int?)null,
                            AdPointLocation = ap != null ? ap.Location : null
                        };

            var ads = await query.ToListAsync();

            var result = from a in ads
                         group a by a.Id into aGroup

                         let ad = aGroup.FirstOrDefault()
                         select new AdDto()
                         {
                             Id = aGroup.Key,
                             Title = ad.Title,
                             Path = ad.Path,
                             ContentType = ad.ContentType,
                             Status = ad.Status,
                             AddedDate = ad.AddedDate,
                             ConfirmationDate = ad.ConfirmationDate,

                             Plans = aGroup.Where(a => a.PlanId != null).Select(a => new AdPlanDto()
                             {
                                 Id = (int)a.PlanId,
                                 Status = a.PlanStatus,
                                 Title = a.PlanTitle
                             }),

                             AdPoints = aGroup.Where(a => a.AdPointId != null).Select(a => new AdAdPointDto()
                             {
                                 Id = (int)a.AdPointId,
                                 Location = a.AdPointLocation
                             })
                         };

            return result.SingleOrDefault();
        }
    }
}
