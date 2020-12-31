using System;
using AdOut.Extensions.Communication;

namespace AdOut.Planning.Model.Events
{
    public class AdPointCreatedEvent : IntegrationEvent
    {
        public string Id { get; set; }

        public string Location { get; set; }

        public string IpAdress { get; set; }

        public TimeSpan StartWorkingTime { get; set; }

        public TimeSpan EndWorkingTime { get; set; }

        public int ScreenWidthCm { get; set; }

        public int ScreenHeightCm { get; set; }

        //todo: move to the IntegrationEvent
        public string Creator { get; set; }
    }
}
