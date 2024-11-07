namespace repository.module.Models.Internal
{
    internal class EventInternal
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public int Timestamp { get; set; }
        public EventDataInternal Data { get; set; }
        public string LocalizedString { get; set; }
    }
}
