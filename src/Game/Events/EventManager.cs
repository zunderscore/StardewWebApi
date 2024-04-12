using StardewModdingAPI.Events;
using StardewValley;
using StardewWebApi.Game.Events.Processors;
using StardewWebApi.Game.Info;

namespace StardewWebApi.Game.Events;

internal class EventManager
{
    private readonly List<IEventProcessor> _eventProcessors = new();

    internal EventManager()
    {
        _eventProcessors.Add(new FestivalEventProcessor());
        _eventProcessors.Add(new RelationshipEventProcessor());

        _eventProcessors.ForEach(e => e.Initialize());
    }

    public void RegisterEvents()
    {
        SMAPIWrapper.Instance.Helper!.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;

        SMAPIWrapper.Instance.Helper!.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        SMAPIWrapper.Instance.Helper!.Events.GameLoop.Saved += OnSaved;
        SMAPIWrapper.Instance.Helper!.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
        SMAPIWrapper.Instance.Helper!.Events.GameLoop.DayStarted += OnDayStarted;
        SMAPIWrapper.Instance.Helper!.Events.GameLoop.TimeChanged += OnTimeChanged;
        SMAPIWrapper.Instance.Helper!.Events.GameLoop.DayEnding += OnDayEnding;

        SMAPIWrapper.Instance.Helper!.Events.Player.InventoryChanged += OnPlayerInventoryChanged;
        SMAPIWrapper.Instance.Helper!.Events.Player.LevelChanged += OnPlayerLevelChanged;
        SMAPIWrapper.Instance.Helper!.Events.Player.Warped += OnPlayerWarped;
    }

    private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
    {
        foreach (var processor in _eventProcessors)
        {
            processor.ProcessEvents();
        }
    }

    private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
    {
        foreach (var processor in _eventProcessors)
        {
            processor.InitializeGameData();
        }

        WebServer.Instance.SendGameEvent("SaveLoaded", new
        {
            FarmName = Game1.getFarm().DisplayName,
            PlayerName = Game1.player.Name
        });
    }

    private void OnSaved(object? sender, SavedEventArgs e)
    {
        WebServer.Instance.SendGameEvent("Saved", new
        {
            FarmName = Game1.getFarm().DisplayName,
            PlayerName = Game1.player.Name
        });
    }

    private void OnReturnedToTitle(object? sender, ReturnedToTitleEventArgs e)
    {
        WebServer.Instance.SendGameEvent("ReturnedToTitle");
    }

    private void OnDayStarted(object? sender, DayStartedEventArgs e)
    {
        WebServer.Instance.SendGameEvent("DayStarted", GameInfo.GetDayInfo());
    }

    private void OnTimeChanged(object? sender, TimeChangedEventArgs e)
    {
        WebServer.Instance.SendGameEvent("TimeChanged", new
        {
            e.OldTime,
            e.NewTime
        });
    }

    private void OnDayEnding(object? sender, DayEndingEventArgs e)
    {
        WebServer.Instance.SendGameEvent("DayEnding", new
        {
            Season = Game1.CurrentSeasonDisplayName,
            Day = Game1.dayOfMonth
        });
    }

    private void OnPlayerInventoryChanged(object? sender, InventoryChangedEventArgs e)
    {
        WebServer.Instance.SendGameEvent("PlayerInventoryChanged", new
        {
            e.Player.Name,
            Added = e.Added.Select(i => new
            {
                i.Name,
                i.DisplayName,
                i.Category,
                i.Stack
            }),
            Removed = e.Removed.Select(i => new
            {
                i.ItemId,
                i.Name,
                i.DisplayName,
                i.Category,
                i.Stack
            }),
            QuantityChanged = e.QuantityChanged.Select(q => new
            {
                q.Item.ItemId,
                q.Item.Name,
                q.Item.DisplayName,
                q.Item.Category,
                q.OldSize,
                q.NewSize,
            })
        });
    }

    private void OnPlayerLevelChanged(object? sender, LevelChangedEventArgs e)
    {
        WebServer.Instance.SendGameEvent("PlayerLevelChanged", new
        {
            e.Player.Name,
            Skill = e.Skill.ToString(),
            e.OldLevel,
            e.NewLevel
        });
    }

    private void OnPlayerWarped(object? sender, WarpedEventArgs e)
    {
        WebServer.Instance.SendGameEvent("PlayerWarped", new
        {
            e.Player.Name,
            OldLocation = e.OldLocation.Name,
            NewLocation = e.NewLocation.Name
        });
    }
}