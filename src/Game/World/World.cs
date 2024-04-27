using StardewValley;

namespace StardewWebApi.Game.World;

public class World
{
    private World(DayInfo today, string farmName, string currentMusicTrack)
    {
        Today = today;
        FarmName = farmName;
        CurrentMusicTrack = currentMusicTrack;
    }

    public static World Current => new(
        DayInfo.Today,
        Game1.getFarm().DisplayName,
        Game1.getMusicTrackName()
    );

    public DayInfo Today { get; }
    public string FarmName { get; }
    public string CurrentMusicTrack { get; }
}