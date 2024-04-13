using StardewWebApi.Game;
using StardewWebApi.Game.NPCs;
using StardewWebApi.Server;

namespace StardewWebApi.Api.V1;

[RequireLoadedGame]
public class NPCApi : ApiControllerBase
{
    [ApiEndpoint("/npcs")]
    public void GetAllNPCs()
    {
        Response.Ok(NPCUtilities.GetAllNPCs()
            .Select(n => NPCInfo.FromNPC(n)
        ));
    }

    [ApiEndpoint("/npcs/name")]
    public void GetNPCByName(string name)
    {
        var npc = NPCInfo.FromNPCName(name);

        if (npc is not null)
        {
            Response.Ok(npc);
        }
        else
        {
            Response.NotFound($"No NPC found with name '{name}'");
        }
    }

    [ApiEndpoint("/npcs/birthday")]
    public void GetNPCByBirthday(string season, int day)
    {
        var npcs = NPCUtilities.GetNPCsByBirthday(season, day)
            .Select(n => NPCInfo.FromNPC(n));

        Response.Ok(npcs);
    }

    [ApiEndpoint("/npcs/pets")]
    public void GetAllPets()
    {
        var npcs = NPCUtilities.GetAllNPCsOfType(NPCType.Pet)
            .Select(n => NPCInfo.FromNPC(n));

        Response.Ok(npcs);
    }
}