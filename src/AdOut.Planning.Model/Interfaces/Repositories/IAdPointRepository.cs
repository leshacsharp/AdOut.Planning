using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IAdPointRepository : IBaseRepository<AdPoint>
    {
        Task<List<AdPointValidation>> GetAdPointValidationsAsync(int[] adPointIds, DateTime planStart, DateTime planEnd);
    }
}
