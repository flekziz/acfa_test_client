namespace repository.module.Models.Internal
{
    internal class EventDataInternal
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public EventDataType Type { get; set; }

        public string EventId { get; set; }
        public EventInternal Event { get; set; }
    }
}
