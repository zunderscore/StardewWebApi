using StardewValley;
using StardewValley.Characters;

namespace StardewWebApi.Game.NPCs;

public class PetInfo : NPCInfo
{
    private readonly Pet _pet;

    public PetInfo(NPC npc) : base(npc)
    {
        _pet = (Pet)_npc;
    }

    public Guid PetId => _pet.petId.Value;
    public string PetType => _pet.petType.Value;
    public int TimesPet => _pet.timesPet.Value;
}