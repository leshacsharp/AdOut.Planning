using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IAdPointRepository : IBaseRepository<AdPoint>
    {
        Task<AdPoint> GetByIdAsync(int adPointId);

        Task<List<AdPointValidation>> GetAdPointsValidationAsync(int[] adPointIds, DateTime planStart, DateTime planEnd);
    }
}
