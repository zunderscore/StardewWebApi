using StardewValley;
using StardewValley.Characters;

namespace StardewWebApi.Game.NPCs;

public class PetInfo : NPCInfo
{
    private readonly Pet _pet;

    protected PetInfo(NPC npc) : base(npc)
    {
        _pet = (Pet)_npc;
    }

    public static PetInfo? FromPetName(string name)
    {
        return FromNPC(NPCUtilities.GetNPCByName(name));
    }

    public static PetInfo? FromNPC(NPC? npc)
    {
        return npc is null or not Pet
            ? null
            : new(npc);
    }

    public Guid PetId => _pet.petId.Value;
    public string PetType => _pet.petType.Value;
    public int TimesPet => _pet.timesPet.Value;
}