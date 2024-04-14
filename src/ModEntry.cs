using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewWebApi.Game;
using StardewWebApi.Game.Events;
using StardewWebApi.Server;

namespace StardewWebApi;

public class ModEntry : Mod
{
    private EventManager? _eventManager;

    public override void Entry(IModHelper helper)
    {
        SMAPIWrapper.Instance.Initialize(Monitor, helper);

        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        SMAPIWrapper.Instance.Log("Starting web server");
        WebServer.Instance.StartWebServer();

        _eventManager = new EventManager();
    }
}