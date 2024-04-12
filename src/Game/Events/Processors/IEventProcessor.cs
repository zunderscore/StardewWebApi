namespace StardewWebApi.Game.Events.Processors;

internal interface IEventProcessor
{
    void Initialize();

    void InitializeGameData();

    void ProcessEvents();
}