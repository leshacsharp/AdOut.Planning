namespace AdOut.Planning.Model.Events
{
    public class PlanAdPointCreatedEvent : IntegrationEvent
    {
        public int PlanId { get; set; }

        public int AdPointId { get; set; }
    }
}
