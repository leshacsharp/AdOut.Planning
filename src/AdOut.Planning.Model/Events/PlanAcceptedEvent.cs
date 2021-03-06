using AdOut.Extensions.Communication;

namespace AdOut.Planning.Model.Events
{
    //todo: add the 'PlanReadyEvent' with plan information
    public class PlanAcceptedEvent : IntegrationEvent
    {
        public string PlanId { get; set; }
    }
}
