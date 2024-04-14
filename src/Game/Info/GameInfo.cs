using StardewValley;

namespace StardewWebApi.Game.Info;

public static class GameInfo
{
    public static WorldInfo GetWorldInfo()
    {
        return new(
            DayInfo.Today,
            Game1.getFarm().DisplayName
        );
    }
}