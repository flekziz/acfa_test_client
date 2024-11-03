namespace repository.module.Models.Output
{
    public class Event
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public int Timestamp { get; set; }
        public EventData Data { get; set; }
        public string LocalizedString { get; set; }
    }
}
