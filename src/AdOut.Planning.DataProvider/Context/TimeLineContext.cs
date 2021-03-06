using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using MongoDB.Driver;
using System;

namespace AdOut.Planning.DataProvider.Context
{
    public class TimeLineContext : ITimeLineContext
    {
        private readonly IMongoDatabase _db;
        public TimeLineContext(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var client = new MongoClient(connectionString);
            var connection = new MongoUrlBuilder(connectionString);
            _db = client.GetDatabase(connection.DatabaseName);
        }

        public IMongoCollection<PlanTime> Plans => _db.GetCollection<PlanTime>(nameof(PlanTime));
    }
}
