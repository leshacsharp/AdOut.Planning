namespace AdOut.Planning.Model.Events
{
    public class AdPointDeletedEvent : IntegrationEvent
    {
        public int AdPointId { get; set; }
    }
}
