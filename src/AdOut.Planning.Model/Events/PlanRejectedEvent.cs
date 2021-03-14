using AdOut.Extensions.Communication;

namespace AdOut.Planning.Model.Events
{
    public class PlanRejectedEvent : IntegrationEvent
    {
        public string Id { get; set; }
        public string Comment { get; set; }
    }
}
