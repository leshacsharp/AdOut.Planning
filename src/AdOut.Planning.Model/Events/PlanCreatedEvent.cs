namespace AdOut.Planning.Model.Events
{
    //todo: need to add other information
    public class PlanCreatedEvent : IntegrationEvent
    {
        public string Title { get; set; }
    }
}
