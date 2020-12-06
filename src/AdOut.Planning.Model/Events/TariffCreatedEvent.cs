using System;

namespace AdOut.Planning.Model.Events
{
    public class TariffCreatedEvent : IntegrationEvent
    {
        public string Id { get; set; }

        public double PriceForMinute { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string AdPointId { get; set; }
    }
}
