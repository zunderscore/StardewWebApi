using System.Text.Json.Serialization;

namespace StardewWebApi.Game.Events;

public record GameEvent(
    string Event,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] object? Data = null
);