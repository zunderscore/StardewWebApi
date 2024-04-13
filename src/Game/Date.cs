using System.Text.Json.Serialization;
using StardewValley;

namespace StardewWebApi.Game;

public class Date
{
    private readonly WorldDate _date;

    public Date(Season season, int day, int year) : this(new WorldDate(year, season, day)) { }

    public Date(WorldDate date)
    {
        _date = date;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Season Season => _date.Season;

    public int Day => _date.DayOfMonth;

    public int Year => _date.Year;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DayOfWeek DayOfWeek => _date.DayOfWeek;

    public string ShortDayOfWeek => Game1.shortDayNameFromDayOfSeason(Day);
}