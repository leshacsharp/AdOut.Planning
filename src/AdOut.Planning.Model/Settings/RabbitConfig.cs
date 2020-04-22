namespace AdOut.Planning.Model.Settings
{
    public class RabbitConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public int ChannelsPool { get; set; }
    }
}
