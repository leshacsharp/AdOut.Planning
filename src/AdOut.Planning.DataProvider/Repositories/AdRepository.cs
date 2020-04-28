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
            return Context.Ads.SingleOrDefaultAsync(ad => ad.Id == adId);
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

                            Plan = p != null ? new AdPlanDto()
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Status = p.Status
                            } : null,

                            AdPoint = ap != null ? new AdPointDto()
                            {
                                Id = ap.Id,
                                Location = ap.Location
                            } : null
                        };

            var adItems = await query.ToListAsync();
            var ad = adItems.SingleOrDefault();

            var result = ad != null ? new AdDto()
            {
                Title = ad.Title,
                Path = ad.Path,
                ContentType = ad.ContentType,
                Status = ad.Status,
                AddedDate = ad.AddedDate,
                ConfirmationDate = ad.ConfirmationDate,

                Plans = adItems.Where(a => a.Plan != null).Select(a => a.Plan),
                AdPoints = adItems.Where(a => a.AdPoint != null).Select(a => a.AdPoint)
            } : null;

            return result;
        }
    }
}
