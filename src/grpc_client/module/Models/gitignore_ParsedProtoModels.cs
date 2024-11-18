using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;

namespace grpc_client.module.ParsedProtoModels;

enum RangeConstraintMin{ MinInt, MinDouble, MinInt64, MinUint64 }

enum RangeConstraintMax{ MaxInt, MaxDouble, MaxInt64, MaxUint64 }

enum RangeConstraintDefault{ DefaultInt, DefaultDouble, DefaultInt64, DefaultUint64 }

record RangeConstraint(
    RangeConstraintMin MinCase,
    int? MinInt,
    double? MinDouble,
    Int64? MinInt64,
    UInt64? MinUint64,
    RangeConstraintMax MaxCase,
    int? MaxInt,
    double? MaxDouble,
    Int64? MaxInt64,
    UInt64? MaxUint64,
    RangeConstraintDefault DefaultCase,
    int? DefaultInt,
    double? DefaultDouble,
    Int64? DefaultInt64,
    UInt64? DefaultUint64
);

record PropertyDescriptorGroup(List<PropertyDescriptor> Properties);

enum EnumerationConstraintsItemValue{
    ValueString,
    ValueInt32,
    ValueDouble,
    ValueBool,
    ValueInt64,
    ValueUint64,
    ValuePropertyGroup
};

record EnumerationConstraintsItem(
    string Name,
    EnumerationConstraintsItemValue ValueCase,
    string? ValueString,
    int? ValueInt32,
    double? ValueDouble,
    bool? ValueBool,
    Int64? ValueInt64,
    UInt64? ValueUint64,
    PropertyDescriptorGroup? ValuePropertyGroup,
    List<PropertyDescriptor> Traits,
    List<PropertyDescriptor> Properties
);

enum EnumerationConstraintsDefault{
    DefaultString,
    DefaultInt32,
    DefaultDouble,
    DefaultBool,
    DefaultInt64,
    DefaultUint64,
    ValuePropertyGroup
};

record EnumerationConstraints(
    List<EnumerationConstraintsItem> Items,
    EnumerationConstraintsDefault DefaultCase,
    string? DefaultString,
    int? DefaultInt32,
    double? DefaultDouble,
    bool? DefaultBool,
    Int64? DefaultInt64,
    UInt64? DefaultUint64,
    PropertyDescriptorGroup? ValuePropertyGroup
);

enum PropertyDescriptorValue{
    ValueString,
    ValueInt32,
    ValueDouble,
    ValueBool,
    ValueInt64,
    ValueUint64,
    ValueProperties
};

enum PropertyDescriptorConstraints{ RangeConstraint, EnumConstraint }

// fix DisplayValue namespace
record PropertyDescriptor(
    string Id,
    string Name,
    string Description,
    string Category,
    string Type,
    bool Readonly,
    bool Internal,
    StringValue DisplayValue,
    PropertyDescriptorValue ValueCase,
    string? ValueString,
    int? ValueInt32,
    double? ValueDouble,
    bool? ValueBool,
    Int64? ValueInt64,
    UInt64? ValueUint64,
    PropertyDescriptorGroup? ValueProperties,
    PropertyDescriptorConstraints ConstraintsCase,
    RangeConstraint? RangeConstraint,
    EnumerationConstraints? EnumConstraint
);

enum EUnitStatus{ UnitStatusUnspecified, UnitStatusActive, UnitStatusInactive }

enum EUnitType{
    Acfaundefined,
    Acfa,
    Acfacontroller,
    Acfaloop,
    Acfareader,
    Acfasensor,
    Acfarelay,
    Acfalock,
    Acfaarea,
    Acfagrouparea,
    Acfafence
};

record ResetOperation(string Id, string DisplayName);

record UnitDescriptor(
    string Uid,
    string DisplayId,
    string Type,
    string LocalizedType,
    string DisplayName,
    List<PropertyDescriptor> Properties,
    List<PropertyDescriptor> Traits,
    List<ResetOperation> ResetOperations,
    List<UnitDescriptor> Units,
    List<PropertyDescriptor> DestructionArgs,
    EUnitStatus Status,
    List<PropertyDescriptor> OpaqueParams,
    string InternalType,
    string LocalizedInternalType
);

record Device(string Vendor, string Model, List<string> Firmware, bool Obsolete);

record UnitType(string Type, Device Device);

enum UnitValue{ Uid, Type }

record Unit(UnitValue ValueCase, string? Uid, UnitType? Type);

enum EStateType{ StNormal, StWarning, StAlarm, StFailure }

record State(string Id, string Name, EStateType Type);

record Icon(string Id, string Image);

record IconGroup(List<Icon> Items);

record Line(string Id, string Color, string Width, string Pattern);

record LineGroup(List<Line> Items);

record Area(string Id, string Color, string Pattern);

record AreaGroup(List<Area> Items);

record Text(string Id);

record TextGroup(List<string> Items);

enum VisualizationValue{ Icons, Lines, Areas, Texts }

record Visualization(
    string Id,
    string Name,
    VisualizationValue ValueCase,
    IconGroup? Icons,
    LineGroup? Lines,
    AreaGroup? Areas,
    TextGroup? Texts
);

enum EEventPhase{ Momentary, Began, Ended, Technical, Periodical }

record Event(string Id, string Name, List<PropertyDescriptor> Params);

record Action(
    string Id,
    string Name,
    List<PropertyDescriptor> Input,
    List<PropertyDescriptor> Output
);

enum ELogLevel{ LlInfo, LlWarnng, LlDebug, LlError, LlTrace }

record InitializeModuleRequest(
    string EnvironmentPath,
    ELogLevel LogLevel,
    string LocalizationLanguage
);

record LogMessage(
    ELogLevel Level,
    string File,
    UInt32 Line,
    string Function,
    string Message
);

enum ModuleEventData{ Event, State, Status }

// fix Timestamp namespace
record ModuleEvent(
    string Uid,
    string DisplayName,
    Timestamp Timestamp,
    ModuleEventData DataCase,
    Event? Event,
    State? State,
    EUnitStatus? Status,
    EEventPhase Phase,
    string Description,
    string LocalizedString
);

record ModuleEventPackage(List<ModuleEvent> Events);

enum ModuleMessageValue{ Log, Event, EventPackage }

record ModuleMessage(
    ModuleMessageValue ValueCase,
    LogMessage? Log,
    ModuleEvent? Event,
    ModuleEventPackage? EventPackage
);

record ListDevicesDescriptionRequest(List<Device> Devices);

record ListDevicesDescriptionResponse(List<UnitDescriptor> Devices);

record DownloadDataRequest(List<string> DataId);

record DownloadDataResponse(string DataId);

record DownloadConfigurationRequest(string Uid);

record DownloadConfigurationResponse(string ParentUid, UnitDescriptor Unit);

record CreateDeviceRequest(Device Device, UnitDescriptor Configuration);

record AddUnitsRequest(string ParentUid, List<UnitDescriptor> Units);

record ChangeUnit(string Uid, List<PropertyDescriptor> Properties);

record ChangeUnitsRequest(List<ChangeUnit> Changed);

record RemoveUnit(string Uid, List<PropertyDescriptor> DestructionArgs);

record RemoveUnitsRequest(List<RemoveUnit> Removed);

record ConfigurationChangeResponse(List<string> Successed, List<string> Failed);

record ListUnitsActionsRequest(List<Unit> Units);

record UnitAction(string Uid, List<Action> Actions);

record ListUnitsActionsResponse(List<UnitAction> Items);

enum EUnitDisplayMode{ DmFull, DmWithProperties, DmWithSubUnits }

record ListUnitsRequest(List<string> UnitUids, int DisplayMode);

record ListUnitsResponse(
    List<UnitDescriptor> Units,
    List<string> UnreachableObjects,
    List<string> NotFoundObjects
);

record ListUnitsFactoryRequest(List<Unit> Units);

record UnitFactory(string Uid, List<UnitDescriptor> Factory);

record ListUnitsFactoryResponse(List<UnitFactory> Items);

record ListUnitsVisualizationsRequest(List<Unit> Units);

record UnitVisualizations(string Uid, List<Visualization> Visualizations);

record ListUnitsVisualizationsResponse(List<UnitVisualizations> Items);

record ListUnitsEventsRequest(List<Unit> Units);

record UnitEvents(string Uid, List<Event> Events);

record ListUnitsEventsResponse(List<UnitEvents> Items);

record ListUnitsStatesRequest(List<Unit> Units);

record ListCurrentStatesRequest(List<Unit> Units);

record UnitStates(string Uid, List<State> States);

record ListUnitsStatesResponse(List<UnitStates> Items);

record PerformActionRequest(string Uid, string Id, List<PropertyDescriptor> Properties);

record PerformActionResponse(string ErrorMessage, List<PropertyDescriptor> Properties);

record UnitTypeInfo(string Name, string DisplayName);

record ListUnitTypesResponse(List<UnitTypeInfo> Items);
