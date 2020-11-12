using System;

namespace AdOut.Planning.Model.Events
{
    public abstract class IntegrationEvent
    {
        public string EventId { get; }

        public DateTime CreatedDateUtc { get; }

        public IntegrationEvent()
        {
            EventId = Guid.NewGuid().ToString();
            CreatedDateUtc = DateTime.UtcNow;
        }
    }
}
