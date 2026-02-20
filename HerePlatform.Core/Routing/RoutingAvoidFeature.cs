using System;
using System.Runtime.Serialization;

namespace HerePlatform.Core.Routing;

/// <summary>
/// Features to avoid in route calculation.
/// </summary>
[Flags]
public enum RoutingAvoidFeature
{
    None = 0,

    [EnumMember(Value = "tollRoad")]
    Tolls = 1 << 0,

    [EnumMember(Value = "controlledAccessHighway")]
    Highways = 1 << 1,

    [EnumMember(Value = "ferry")]
    Ferries = 1 << 2,

    [EnumMember(Value = "tunnel")]
    Tunnels = 1 << 3,

    [EnumMember(Value = "dirtRoad")]
    DirtRoad = 1 << 4,

    [EnumMember(Value = "carShuttleTrain")]
    CarShuttleTrain = 1 << 5,

    [EnumMember(Value = "seasonalClosure")]
    SeasonalClosure = 1 << 6,

    [EnumMember(Value = "difficultTurns")]
    DifficultTurns = 1 << 7
}
