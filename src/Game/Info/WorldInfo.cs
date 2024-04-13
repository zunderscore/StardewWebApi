namespace StardewWebApi.Game.Info;

public record WorldInfo(
    DayInfo Today,
    string FarmName
)
{ }