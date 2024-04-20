using StardewWebApi.Game;
using StardewWebApi.Game.NPCs;
using StardewWebApi.Server;
using StardewWebApi.Server.Routing;

namespace StardewWebApi.Api.V1;

[RequireLoadedGame]
public class NPCApi : ApiControllerBase
{
    [Route("/npcs")]
    public void GetAllNPCs()
    {
        Response.Ok(NPCUtilities.GetAllNPCs()
            .Select(n => NPCInfo.FromNPC(n)
        ));
    }

    [Route("/npcs/name/{name}")]
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

    [Route("/npcs/birthday/{season}/{day}")]
    public void GetNPCByBirthday(string season, int day)
    {
        var npcs = NPCUtilities.GetNPCsByBirthday(season, day)
            .Select(n => NPCInfo.FromNPC(n));

        Response.Ok(npcs);
    }

    [Route("/npcs/pets")]
    public void GetAllPets()
    {
        var npcs = NPCUtilities.GetAllNPCsOfType(NPCType.Pet)
            .Select(n => NPCInfo.FromNPC(n));

        Response.Ok(npcs);
    }
}