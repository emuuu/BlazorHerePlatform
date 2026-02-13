using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using HerePlatformComponents.Serialization;

namespace HerePlatformComponents.Maps.Services.Routing;

/// <summary>
/// Routing optimization mode.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<RoutingMode>))]
public enum RoutingMode
{
    [EnumMember(Value = "fast")]
    Fast,

    [EnumMember(Value = "short")]
    Short
}
