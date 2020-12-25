using AdOut.Extensions.Communication;

namespace AdOut.Planning.Model.Events
{
    public class AdPointDeletedEvent : IntegrationEvent
    {
        public string Id { get; set; }
    }
}
