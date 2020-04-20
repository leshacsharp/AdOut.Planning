using System;

namespace AdOut.Planning.Model.Events
{
    public abstract class IntegrationEvent
    {
        public Guid EventId { get; }

        public DateTime CreatedDateUtc { get; }

        public IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            CreatedDateUtc = DateTime.UtcNow;
        }
    }
}
