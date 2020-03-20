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
        Task<Ad> GetByIdAsync(int adId);

        Task<AdDto> GetDtoByIdAsync(int adId);

        Task<List<AdListDto>> GetAds(Expression<Func<Ad, bool>> predicate);

        bool HavePlans(int adId);
    }
}
