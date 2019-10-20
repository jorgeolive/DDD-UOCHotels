using System;
namespace UOCHotels.RoomServiceManagement.Messaging.Configuration
{
    public class RabbitMqSubscriberConfiguration
    {
        public string HostName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public Exchange Exchange { get; set; }

        public int Port { get; set; }
        public string EnvVarTest { get; set; }
    }
}
