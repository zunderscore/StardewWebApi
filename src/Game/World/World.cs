using StardewValley;

namespace StardewWebApi.Game.World;

public class World
{
    private World(DayInfo today, string farmName)
    {
        Today = today;
        FarmName = farmName;
    }

    public static World GetCurrent()
    {
        return new World(DayInfo.Today, Game1.getFarm().DisplayName);
    }

    public DayInfo Today { get; }
    public string FarmName { get; }
}