using StardewValley;

namespace StardewWebApi.Game.Events.Processors;

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

        if (_lastFestival == null && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
        {
            TriggerFestivalStart();
            _lastFestival = Game1.CurrentEvent;
        }
        else if (_lastFestival != null && Game1.CurrentEvent == null)
        {
            TriggerFestivalEnd();
            _lastFestival = null;
        }
    }

    private void TriggerFestivalStart()
    {
        WebServer.Instance.SendGameEvent("FestivalStarted", new
        {
            Name = Game1.CurrentEvent.FestivalName
        });
    }

    private void TriggerFestivalEnd()
    {
        WebServer.Instance.SendGameEvent("FestivalEnded", new
        {
            Name = _lastFestival!.FestivalName
        });
    }
}