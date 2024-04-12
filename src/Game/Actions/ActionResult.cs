using System.Text.Json.Serialization;

namespace StardewWebApi.Game.Actions;

public record ActionResult(
    bool Success,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] object? Data = null
);