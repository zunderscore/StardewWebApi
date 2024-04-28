using StardewValley.GameData.Locations;
using StardewValley.TokenizableStrings;

namespace StardewWebApi.Game.RawData;

public class LocationStub
{
    private readonly LocationData _locationData;

    public LocationStub(string name, LocationData locationData)
    {
        Name = name;
        _locationData = locationData;

        DisplayName = TokenParser.ParseText(_locationData.DisplayName);
    }

    public string Name { get; }
    public string DisplayName { get; }

    public string Url => $"/api/v1/data/locations/{Name}";
}