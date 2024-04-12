using StardewValley;

namespace StardewWebApi.Game.Info;

public static class GameInfo
{
    public static DayInfo GetDayInfo()
    {
        return new(
            Game1.Date,
            Game1.getFarm().GetWeather().Weather
        );
    }

    public static WorldInfo GetWorldInfo()
    {
        return new(
            GetDayInfo(),
            Game1.getFarm().DisplayName
        );
    }
}