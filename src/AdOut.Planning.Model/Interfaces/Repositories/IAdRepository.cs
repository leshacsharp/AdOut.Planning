using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IAdRepository : IBaseRepository<Ad>
    {
        Task<Ad> FindByIdAsync(int adId);
        Task<List<AdDto>> GetAds(Expression<Func<Ad, bool>> predicate);
    }
}
