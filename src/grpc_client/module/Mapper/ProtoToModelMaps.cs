using System.Linq;
using grpc_client.module.Models;

namespace grpc_client.module.Mapper;

internal class ProtoToModelMaps
{
    //public static void RegisterMaps(MapBuilder maps) => maps
    //    .Register<Drivers.Acfa.V2.PropertyDescriptor, string>(PropertyValueToString)
    //    .Register<Drivers.Acfa.V2.PropertyDescriptor, Property>(PropertyDescriptorToProperty)
    //    .Register<Drivers.Acfa.V2.ModuleEvent, ModuleEvent>(ModuleEventToEvent);

    public static ModuleEvent ModuleEventToEvent(Drivers.Acfa.V2.ModuleEvent e, ManualMapper mapper)
    {
        var data = e.DataCase switch
        {
            Drivers.Acfa.V2.ModuleEvent.DataOneofCase.Event => 
                EventData.Event(e.Event.Id, e.Event.Name, 
                    mapper.MapEnumerable<Drivers.Acfa.V2.PropertyDescriptor, Property>(e.Event.Params).ToList()),
            
            Drivers.Acfa.V2.ModuleEvent.DataOneofCase.State => 
                EventData.State(e.State.Id, e.State.Name, (EStateType)e.State.Type),
            
            Drivers.Acfa.V2.ModuleEvent.DataOneofCase.Status =>
                EventData.Other(e.State.Id, e.State.Name, EventDataType.Status),
            
            _ => EventData.Empty
        };

        return new ModuleEvent(
            e.Uid,
            e.DisplayName,
            e.Timestamp.Seconds,
            data,
            e.LocalizedString
        );
    }

    public static string PropertyValueToString(Drivers.Acfa.V2.PropertyDescriptor prop, ManualMapper mapper)
    {
        return prop.ValueCase switch
        {
            Drivers.Acfa.V2.PropertyDescriptor.ValueOneofCase.ValueInt32 =>
                prop.ValueInt32.ToString(),
            Drivers.Acfa.V2.PropertyDescriptor.ValueOneofCase.ValueInt64 =>
                prop.ValueInt64.ToString(),
            Drivers.Acfa.V2.PropertyDescriptor.ValueOneofCase.ValueUint64 =>
                prop.ValueUint64.ToString(),
            Drivers.Acfa.V2.PropertyDescriptor.ValueOneofCase.ValueDouble =>
                prop.ValueDouble.ToString(),
            Drivers.Acfa.V2.PropertyDescriptor.ValueOneofCase.ValueBool =>
                prop.ValueBool.ToString(),
            Drivers.Acfa.V2.PropertyDescriptor.ValueOneofCase.ValueString =>
                prop.ValueString,
            _ => ""
        };
    }

    /* 
    public internal static string PropertyValueToString(Drivers.Acfa.V2.PropertyDescriptor prop, ManualMapper mapper)
    {
        var values = new object[] {
            prop.ValueInt32, prop.ValueInt64, prop.ValueUint64,
            prop.ValueDouble, prop.ValueBool, prop.ValueString
        }; // wait, they all have default values...
        foreach (var obj in values)
            if (obj != null)
                return obj.ToString()!;

        return "grpc service error";
    } //*/

    public static Property PropertyDescriptorToProperty(Drivers.Acfa.V2.PropertyDescriptor prop, ManualMapper mapper)
        => prop.ValueCase switch
        {
            Drivers.Acfa.V2.PropertyDescriptor.ValueOneofCase.ValueProperties =>
                new Property(
                    prop.Id,
                    prop.Name,
                    null,
                    mapper.MapEnumerable<Drivers.Acfa.V2.PropertyDescriptor, Property>(
                        prop.ValueProperties.Properties).ToList()
                ),
            // in all other cases converting to string
            _ => new Property(
                    prop.Id,
                    prop.Name,
                    mapper.Map<string>(prop),
                    null
                )
        };

    public static Unit UnitDescriptorToUnit(Drivers.Acfa.V2.UnitDescriptor unit, ManualMapper mapper)
        => new Unit(
            unit.Uid, 
            unit.DisplayName,
            unit.InternalType,
            mapper.MapEnumerable<Drivers.Acfa.V2.UnitDescriptor, Unit>(unit.Units).ToList(),
            mapper.MapEnumerable<Drivers.Acfa.V2.PropertyDescriptor, Property>(unit.Properties).ToList()
        );

    public static EventData EventToEventData(Drivers.Acfa.V2.Event e, ManualMapper mapper) =>
        EventData.Event(
            e.Id, e.Name, 
            mapper.MapEnumerable<Drivers.Acfa.V2.PropertyDescriptor, Property>(e.Params).ToList()
        );

    public static UnitEvents UnitEvents(Drivers.Acfa.V2.ListUnitsEventsResponse.Types.UnitEvents events, ManualMapper mapper) =>
        new UnitEvents(events.Uid, mapper.MapEnumerable<Drivers.Acfa.V2.Event, EventData>(events.Events).ToList());

    public static ConfigurationChangeResponse ConfigurationChangeResponse(Drivers.Acfa.V2.ConfigurationChangeResponse response, ManualMapper mapper) =>
        new ConfigurationChangeResponse(response.Successed.ToList(), response.Failed.ToList());

    public static DownloadConfigurationResponse DownloadConfigurationResponse(Drivers.Acfa.V2.DownloadConfigurationResponse response, ManualMapper mapper) =>
        new DownloadConfigurationResponse(response.ParentUid, mapper.Map<Unit>(response.Unit)!);
}
