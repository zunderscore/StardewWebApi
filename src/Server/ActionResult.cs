using System.Text.Json.Serialization;

namespace StardewWebApi.Server;

public record ActionResult(
    bool Success,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] object? Data = null
);