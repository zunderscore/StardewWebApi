using StardewModdingAPI;

namespace StardewWebApi.Game.Mods;

public class ModStub
{
    private readonly IModInfo _modInfo;

    public ModStub(IModInfo modInfo)
    {
        _modInfo = modInfo;
    }

    public string Name => _modInfo.Manifest.Name;
    public string UniqueId => _modInfo.Manifest.UniqueID;
    public string Description => _modInfo.Manifest.Description;
    public string Author => _modInfo.Manifest.Author;
    public string Version => _modInfo.Manifest.Version.ToString();
    public string Url => $"/api/v1/mods/{UniqueId}";
}