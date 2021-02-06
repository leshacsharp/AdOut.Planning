using AdOut.Extensions.Communication;

namespace AdOut.Planning.Model.Events
{
    //todo: need to add other information
    public class PlanCreatedEvent : IntegrationEvent
    {
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
