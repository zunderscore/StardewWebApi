using StardewModdingAPI.Events;
using StardewWebApi.Game.Events.Processors;

namespace StardewWebApi.Game.Events;

internal class EventManager
{
    private readonly List<IEventProcessor> _eventProcessors = new();

    internal EventManager()
    {
        _eventProcessors.Add(new DefaultEventProcessor());
        _eventProcessors.Add(new FestivalEventProcessor());
        _eventProcessors.Add(new RelationshipEventProcessor());

        _eventProcessors.ForEach(e => e.Initialize());

        SMAPIWrapper.Instance.Helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
        SMAPIWrapper.Instance.Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
    }

    private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
    {
        foreach (var processor in _eventProcessors)
        {
            processor.InitializeGameData();
        }
    }

    private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
    {
        foreach (var processor in _eventProcessors)
        {
            processor.ProcessEvents();
        }
    }
}