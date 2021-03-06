using AdOut.Extensions.Communication;

namespace AdOut.Planning.Model.Events
{
    public class PlanRejectedEvent : IntegrationEvent
    {
        public string PlanId { get; set; }
        public string Comment { get; set; }
    }
}
