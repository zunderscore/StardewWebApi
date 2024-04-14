using StardewValley;
using StardewWebApi.Game.NPCs;

namespace StardewWebApi.Game.Info;

public record DayInfo(
    Date Date,
    string Weather
)
{
    public static DayInfo Today => new(Date.Today, Game1.getFarm().GetWeather().Weather);

    public IEnumerable<NPCStub> Birthdays => NPCUtilities.GetNPCsByBirthday(Date.Season, Date.Day)
        .Select(n => n.CreateStub());
}