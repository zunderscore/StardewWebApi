using StardewValley;
using System.Text.Json.Serialization;

namespace StardewWebApi.Game.NPCs;

public class NPCInfo
{
    protected readonly NPC _npc;

    protected NPCInfo(NPC npc)
    {
        _npc = npc;
    }

    public static NPCInfo? FromNPCName(string name)
    {
        var npc = NPCUtilities.GetNPCByName(name);

        if (npc is null)
        {
            return null;
        }

        return NPCUtilities.GetNPCType(npc) switch
        {
            NPCType.Villager => VillagerInfo.FromNPC(npc),
            NPCType.Pet => PetInfo.FromNPC(npc),
            _ => new NPCInfo(npc),
        };
    }

    public int Id => _npc.id;
    public string Name => _npc.Name;
    public string DisplayName => _npc.displayName;
    public int Age => _npc.Age;
    public string CurrentLocation => _npc.currentLocation.Name;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NPCType Type => NPCUtilities.GetNPCType(_npc);

    public NPCStub CreateStub()
    {
        return NPCStub.FromNPCInfo(this);
    }
}