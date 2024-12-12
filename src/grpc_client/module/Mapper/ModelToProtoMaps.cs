using grpc_client.module.Models;

namespace grpc_client.module.Mapper;

internal static class ModelToProtoMaps
{
    //public static void RegisterMaps(MapBuilder maps) => maps
    //    .Register<(Device device, Unit configuration), Drivers.Acfa.V2.CreateDeviceRequest>(CreateDeviceRequest);

    public static Drivers.Acfa.V2.CreateDeviceRequest CreateDeviceRequest((Device, Unit) r, ManualMapper mapper)
    {
        var (device, configuration) = r;

        Drivers.Acfa.V2.Device dev = new()
        {
            Model = device.Model,
            Vendor = device.Vendor,
            Obsolete = device.Obsolete ?? false
        };
        dev.Firmware.AddRange(device.Firmware);

        Drivers.Acfa.V2.UnitDescriptor conf = new()
        {
            Uid = configuration.Uid,
            DisplayName = configuration.DisplayName,
            InternalType = configuration.InternalType
        };
        conf.Units.AddRange(
            mapper.MapEnumerable<Unit, Drivers.Acfa.V2.UnitDescriptor>(
                configuration.Configurations));
        conf.Properties.AddRange(
            mapper.MapEnumerable<Property, Drivers.Acfa.V2.PropertyDescriptor>(
                configuration.Properties));

        Drivers.Acfa.V2.CreateDeviceRequest req = new()
        {
            Configuration = conf,
            Device = dev
        };

        return req;
    }

    public static Drivers.Acfa.V2.PropertyDescriptor PropertyToPropertyDescriptor(Property property, ManualMapper mapper)
    {
        Drivers.Acfa.V2.PropertyDescriptor prop = new()
        {
            Id = property.Id,
            Name = property.Name,
        };
        if (property.Value != null)
            prop.ValueString = property.Value;
        if (property.Properties != null)
        {
            Drivers.Acfa.V2.PropertyDescriptorGroup group = new();
            group.Properties.AddRange(mapper.MapEnumerable<Property,
                Drivers.Acfa.V2.PropertyDescriptor>(property.Properties));
        }
        return prop;
    }

    public static Drivers.Acfa.V2.UnitDescriptor UnitToUnitDescriptor(Unit unit, ManualMapper mapper)
    {
        Drivers.Acfa.V2.UnitDescriptor unitDesc = new()
        {
            Uid = unit.Uid,
            DisplayName = unit.DisplayName,
            InternalType = unit.InternalType
        };
        unitDesc.Units.AddRange(
            mapper.MapEnumerable<Unit, Drivers.Acfa.V2.UnitDescriptor>(
                unit.Configurations));
        unitDesc.Properties.AddRange(
            mapper.MapEnumerable<Property, Drivers.Acfa.V2.PropertyDescriptor>(
                unit.Properties));
        return unitDesc;
    }

    public static Drivers.Acfa.V2.ChangeUnitsRequest.Types.ChangeUnit ChangeUnit(ChangeUnit change, ManualMapper mapper)
    {
        Drivers.Acfa.V2.ChangeUnitsRequest.Types.ChangeUnit ret = new() { Uid = change.Uid };
        ret.Properties.AddRange(mapper.MapEnumerable<Property,
            Drivers.Acfa.V2.PropertyDescriptor>(change.Properties));
        return ret;
    }
    public static Drivers.Acfa.V2.RemoveUnitsRequest.Types.RemoveUnit RemoveUnit(RemoveUnit remove, ManualMapper mapper)
    {
        Drivers.Acfa.V2.RemoveUnitsRequest.Types.RemoveUnit ret = new() { Uid = remove.Uid };
        ret.DestructionArgs.AddRange(mapper.MapEnumerable<Property,
            Drivers.Acfa.V2.PropertyDescriptor>(remove.DestructionArgs));
        return ret;
    }
}
