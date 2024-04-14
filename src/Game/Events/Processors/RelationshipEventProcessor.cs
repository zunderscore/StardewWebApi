using StardewValley;
using StardewWebApi.Game.NPCs;
using StardewWebApi.Game.Players;
using StardewWebApi.Server;

namespace StardewWebApi.Game.Events.Processors;

internal record TrackedRelationship(int Points);

public class RelationshipChangedEventData
{
    private readonly NPC _npc;

    public RelationshipChangedEventData(string npcName, int previousPoints, int newPoints)
    {
        _npc = NPCUtilities.GetNPCByName(npcName)!;
        PreviousPoints = previousPoints;
        NewPoints = newPoints;
    }

    public NPCStub NPC => _npc.CreateStub();
    public int PreviousPoints { get; }
    public int NewPoints { get; }
    public int PreviousHearts => Relationship.GetHeartsFromPoints(PreviousPoints);
    public int NewHearts => Relationship.GetHeartsFromPoints(NewPoints);
}

internal class RelationshipEventProcessor : IEventProcessor
{
    private readonly Dictionary<string, TrackedRelationship> _relationships = new();

    public void Initialize() { }

    public void InitializeGameData()
    {
        RefreshFriendshipList();
    }

    private void RefreshFriendshipList()
    {
        foreach (var name in Game1.player.friendshipData.Keys)
        {
            _relationships[name] = new(
                Game1.player.friendshipData[name].Points
            );
        }
    }

    public void ProcessEvents()
    {
        if (!Game1.hasLoadedGame)
        {
            return;
        }

        var increasedFriendships = new List<string>();
        var decreasedFriendships = new List<string>();

        foreach (var name in _relationships.Keys)
        {
            if (Game1.player.friendshipData[name].Points > _relationships[name].Points)
            {
                increasedFriendships.Add(name);
            }
            else if (Game1.player.friendshipData[name].Points < _relationships[name].Points)
            {
                decreasedFriendships.Add(name);
            }
        }

        if (increasedFriendships.Count == 1)
        {
            TriggerFriendshipIncrease(increasedFriendships[0]);
        }
        else if (increasedFriendships.Count > 1)
        {
            TriggerMultipleFriendshipIncrease(increasedFriendships);
        }

        if (decreasedFriendships.Count == 1)
        {
            TriggerFriendshipDecrease(decreasedFriendships[0]);
        }
        else if (decreasedFriendships.Count > 1)
        {
            TriggerMultipleFriendshipDecrease(decreasedFriendships);
        }

        RefreshFriendshipList();
    }

    private RelationshipChangedEventData BuildRelationshipChangeData(string name)
    {
        return new(
            name,
            Game1.player.friendshipData[name].Points,
            _relationships[name].Points
        );
    }

    private IEnumerable<RelationshipChangedEventData> BuildRelationshipChangeData(IEnumerable<string> names) =>
        names.Select(n => BuildRelationshipChangeData(n));

    private void TriggerFriendshipIncrease(string name)
    {
        WebServer.Instance.SendGameEvent("FriendshipIncreased", BuildRelationshipChangeData(name));
    }

    private void TriggerMultipleFriendshipIncrease(IEnumerable<string> names)
    {
        WebServer.Instance.SendGameEvent("MultipleFriendshipsIncreased", BuildRelationshipChangeData(names));
    }

    private void TriggerFriendshipDecrease(string name)
    {
        WebServer.Instance.SendGameEvent("FriendshipDecreased", BuildRelationshipChangeData(name));
    }

    private void TriggerMultipleFriendshipDecrease(IEnumerable<string> names)
    {
        WebServer.Instance.SendGameEvent("MultipleFriendshipsDecreased", BuildRelationshipChangeData(names));
    }
}