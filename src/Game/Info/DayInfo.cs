using System.Text.Json.Serialization;
using StardewValley;
using StardewWebApi.Game.NPCs;

namespace StardewWebApi.Game.Info;

public class DayInfo
{
    private readonly WorldDate _date;

    public DayInfo(WorldDate date, string weather)
    {
        _date = date;
        Weather = weather;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Season Season => _date.Season;

    public int Day => _date.DayOfMonth;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DayOfWeek DayOfWeek => _date.DayOfWeek;

    public string ShortDayOfWeek => Game1.shortDayNameFromDayOfSeason(Day);

    public int Year => _date.Year;

    public string Weather { get; }

    public List<NPCStub> Birthdays => NPCUtilities.GetNPCsByBirthday(Season, Day)
        .Select(n => n.CreateStub())
        .ToList();
}