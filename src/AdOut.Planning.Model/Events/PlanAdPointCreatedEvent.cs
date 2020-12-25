using AdOut.Extensions.Communication;

namespace AdOut.Planning.Model.Events
{
    public class PlanAdPointCreatedEvent : IntegrationEvent
    {
        public string PlanId { get; set; }

        public string AdPointId { get; set; }
    }
}
