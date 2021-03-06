using AdOut.Extensions.Repositories;
using AdOut.Planning.Model.Classes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Database
{
    public class PlanTime : PersistentEntity
    {
        [BsonId]
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<AdPlanTime> Ads { get; set; }

        public IEnumerable<SchedulePeriod> Schedules { get; set; }

        public IEnumerable<string> AdPoints { get; set; }
    }
}
