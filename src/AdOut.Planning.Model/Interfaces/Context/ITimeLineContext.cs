using AdOut.Planning.Model.Database;
using MongoDB.Driver;

namespace AdOut.Planning.Model.Interfaces.Context
{
    public interface ITimeLineContext
    {
        IMongoCollection<PlanTime> Plans { get; }
    }
}
