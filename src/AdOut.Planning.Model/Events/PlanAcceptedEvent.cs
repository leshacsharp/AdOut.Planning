using AdOut.Extensions.Communication;

namespace AdOut.Planning.Model.Events
{
    public class PlanAcceptedEvent : IntegrationEvent
    {
        public string Id { get; set; }
    }
}
