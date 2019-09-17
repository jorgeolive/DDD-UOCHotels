namespace UOCHotels.RoomServiceManagement.Messaging.Configuration
{
    public class Exchange
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string[] Queues { get; set; }
    }
}