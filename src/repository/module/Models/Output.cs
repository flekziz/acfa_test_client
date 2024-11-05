namespace repository.module.Models
{
    public record struct Configuration(string Uid, string DisplayName, string InternalType, Configuration[] Configurations, Property[] Properties);
    public record struct Property(string Id, string? ValueString, Property[] Properties);
    public record struct Event(string Uid, string DisplayName, int Timestamp, EventData Data, string LocalizedString);
    public record struct EventData(string Id, string Name, EventDataType Type);
}
