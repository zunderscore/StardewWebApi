using StardewValley;
using StardewWebApi.Server;

namespace StardewWebApi.Game.Events.Processors;

public record FestivalStartedEventData(
    string FestivalName
);

public record FestivalEndedEventData(
    string FestivalName
);

internal class FestivalEventProcessor : IEventProcessor
{
    private Event? _lastFestival;

    public void Initialize() { }

    public void InitializeGameData()
    {
        _lastFestival = Game1.CurrentEvent?.isFestival ?? false
            ? Game1.CurrentEvent
            : null;
    }

    public void ProcessEvents()
    {
        if (!Game1.hasLoadedGame)
        {
            return;
        }

        if (_lastFestival is null && Game1.CurrentEvent is not null && Game1.CurrentEvent.isFestival)
        {
            TriggerFestivalStart();
            _lastFestival = Game1.CurrentEvent;
        }
        else if (_lastFestival is not null && Game1.CurrentEvent is null)
        {
            TriggerFestivalEnd();
            _lastFestival = null;
        }
    }

    private void TriggerFestivalStart()
    {
        WebServer.Instance.SendGameEvent("FestivalStarted", new FestivalStartedEventData
        (
            Game1.CurrentEvent.FestivalName
        ));
    }

    private void TriggerFestivalEnd()
    {
        WebServer.Instance.SendGameEvent("FestivalEnded", new FestivalEndedEventData
        (
            _lastFestival!.FestivalName
        ));
    }
}