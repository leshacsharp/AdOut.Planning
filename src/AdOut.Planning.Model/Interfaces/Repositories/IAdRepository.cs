using AdOut.Extensions.Repositories;
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
        Task<AdDto> GetDtoByIdAsync(string adId);
        Task<List<AdListDto>> GetAds(Expression<Func<Ad, bool>> predicate);
    }
}
