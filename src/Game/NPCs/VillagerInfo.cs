using StardewValley;
using System.Text.Json.Serialization;

namespace StardewWebApi.Game.NPCs;

public class VillagerInfo : NPCInfo
{
    public VillagerInfo(NPC npc) : base(npc) { }

    public int Id => _npc.id;
    public string BirthdaySeason => _npc.Birthday_Season;
    public int BirthdayDay => _npc.Birthday_Day;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender => _npc.Gender;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FriendshipStatus? FriendshipStatus => Game1.player.friendshipData.ContainsKey(Name)
        ? Game1.player.friendshipData[Name].Status
        : null;
}