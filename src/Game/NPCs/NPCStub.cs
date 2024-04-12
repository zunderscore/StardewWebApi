using StardewValley;

namespace StardewWebApi.Game.NPCs;

public class NPCStub
{
    private NPCStub(int id, string name, NPCType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public static NPCStub FromNPC(NPC npc)
    {
        return new(npc.id, npc.Name, NPCUtilities.GetNPCType(npc));
    }

    public static NPCStub FromNPCInfo(NPCInfo npc)
    {
        return new(npc.Id, npc.Name, npc.Type);
    }

    public int Id { get; }
    public string Name { get; }
    public NPCType Type { get; }
}

public static class NPCStubExtensions
{
    public static NPCStub CreateStub(this NPC npc)
    {
        return NPCStub.FromNPC(npc);
    }
}