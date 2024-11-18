using System.Collections.Generic;

namespace grpc_client.module.Models;

public enum EStateType { StNormal, StWarning, StAlarm, StFailure }
public enum EventDataType { None, Event, State, Status }

public record EventData(
    string Id, 
    string Name, 
    EventDataType Type, 
    List<Property>? EventParams, 
    EStateType? StateType
)
{
    public static EventData Empty
        => new("empty", "empty", EventDataType.None, null, null);
    public static EventData Event(string id, string name, List<Property>? @params)
        => new(id, name, EventDataType.Event, @params, null);
    public static EventData State(string id, string name, EStateType state)
        => new(id, name, EventDataType.State, null, state);
    public static EventData Other(string id, string name, EventDataType type)
        => new(id, name, type, null, null);
};

public record ModuleEvent(
    string Uid,
    string DisplayName,
    long Timestamp,
    EventData Data,
    string LocalizedString
);
/*{
    public static ModuleEvent Empty
        => new("empty", "empty", 0, EventData.Empty, "empty");
};*/

public record Property(
    string Id, 
    string? Name, 
    string? Value, 
    List<Property>? Properties
);

public record Unit(
    string Uid, 
    string DisplayName, 
    string InternalType, 
    List<Unit> Configurations, 
    List<Property> Properties
);


public record Device(string Vendor, string Model, List<string>? Firmware, bool? Obsolete);


// do i really need it??
public record ConfigurationChangeResponse(List<string> Successed, List<string> Failed)
{
    public bool Ok() => Failed.Count == 0;
}

public record UnitEvents(string Uid, List<EventData> Events);
// record UnitStates(string Uid, List<EventData> States); // it's not like anyone gonna use states anyway

public record ChangeUnit(string Uid, List<Property> Properties);

public record RemoveUnit(string Uid, List<Property> DestructionArgs);

public record DownloadConfigurationResponse(string ParentUid, Unit Unit);
