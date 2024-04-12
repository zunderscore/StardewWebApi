namespace StardewWebApi.Types;

public record NumericIdNameDescription(int Id, string Name, string Description)
    : NameDescription(Name, Description);

public record StringIdNameDescription(string Id, string Name, string Description)
    : NameDescription(Name, Description);