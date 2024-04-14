using StardewValley;
using StardewWebApi.Game.NPCs;

namespace StardewWebApi.Game.Info;

public class DayInfo : Date
{
    public DayInfo(WorldDate date, string weather) : base(date)
    {
        Weather = weather;
    }

    public string Weather { get; }

    public IEnumerable<NPCStub> Birthdays => NPCUtilities.GetNPCsByBirthday(Season, Day)
        .Select(n => n.CreateStub());
}