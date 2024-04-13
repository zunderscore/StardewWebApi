using StardewValley;

namespace StardewWebApi.Game.NPCs;

public class NPCStub
{
    private NPCStub(string name, NPCType type)
    {
        Name = name;
        Type = type;
    }

    public static NPCStub FromNPC(NPC npc)
    {
        return new(npc.Name, NPCUtilities.GetNPCType(npc));
    }

    public static NPCStub FromNPCInfo(NPCInfo npc)
    {
        return new(npc.Name, npc.Type);
    }

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