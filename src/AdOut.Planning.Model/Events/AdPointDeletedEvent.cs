namespace AdOut.Planning.Model.Events
{
    public class AdPointDeletedEvent : IntegrationEvent
    {
        public string AdPointId { get; set; }
    }
}
