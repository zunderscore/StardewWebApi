using StardewValley;
using System.Text.Json.Serialization;

namespace StardewWebApi.Game.NPCs;

public class VillagerInfo : NPCInfo
{
    protected VillagerInfo(NPC npc) : base(npc) { }

    public static VillagerInfo? FromVillagerName(string name)
    {
        return FromNPC(NPCUtilities.GetNPCByName(name));
    }

    public static VillagerInfo? FromNPC(NPC? npc)
    {
        return npc is null || !npc.IsVillager
            ? null
            : new(npc);
    }

    public string BirthdaySeason => _npc.Birthday_Season;
    public int BirthdayDay => _npc.Birthday_Day;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender => _npc.Gender;

    public FriendshipStatus? FriendshipStatus => Game1.player.friendshipData.ContainsKey(Name)
        ? Game1.player.friendshipData[Name].Status
        : null;
}