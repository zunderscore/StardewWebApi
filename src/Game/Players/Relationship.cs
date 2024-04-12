using StardewValley;
using StardewWebApi.Game.NPCs;

namespace StardewWebApi.Game.Players;

public class Relationship
{
    private readonly NPC _npc;
    private readonly Friendship _friendship;

    private Relationship(NPC npc, Friendship friendship)
    {
        _npc = npc;
        _friendship = friendship;
    }

    public static Relationship FromFriendshipData(string name, Friendship friendship)
    {
        var npc = NPCUtilities.GetNPCByName(name)!;

        return new(npc, friendship);
    }

    public static int GetHeartsFromPoints(int points) => (int)Math.Floor(points / 250D);

    public NPCStub NPC => _npc.CreateStub();
    public int Points => _friendship.Points;
    public int Hearts => GetHeartsFromPoints(Points);
    public bool HasBeenGivenGiftToday => GiftsGivenToday > 0;
    public int GiftsGivenToday => _friendship.GiftsToday;
    public int GiftsGivenThisWeek => _friendship.GiftsThisWeek;
    public WorldDate LastGiftDate => _friendship.LastGiftDate;
    public bool IsDating => _friendship.IsDating();
    public bool IsEngaged => _friendship.IsEngaged();
    public bool IsMarried => _friendship.IsMarried();
    public bool IsRoommate => _friendship.IsRoommate();
    public bool IsDivorced => _friendship.IsDivorced();
}