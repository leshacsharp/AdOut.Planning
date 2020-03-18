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

        public Task<List<AdDto>> GetAds(Expression<Func<Ad, bool>> predicate)
        {
            var query = from ad in Table.Where(predicate)
                        orderby ad.AddedDate descending
                        select new AdDto()
                        {
                            Id = ad.Id,
                            Title = ad.Title,
                            ContentType = ad.ContentType,
                            Status = ad.Status,
                            PreviewPath = ad.PreviewPath
                        };

            return query.ToListAsync();
        }

        public Task<Ad> FindByIdAsync(int adId)
        {
            return Table.FindAsync(adId).AsTask();
        }
    }
}
