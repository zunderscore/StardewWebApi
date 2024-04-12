using StardewWebApi.Game.NPCs;
using StardewWebApi.Types;

namespace StardewWebApi.Api.V1;

[RequireLoadedGame]
public class NPCApi : ApiControllerBase
{
    [ApiEndpoint("/npcs")]
    public void GetAllNPCs()
    {
        Response.Ok(NPCUtilities.GetAllNPCs()
            .Select(n => NPCInfo.FromNPCName(n.Name)
        ));
    }

    [ApiEndpoint("/npc")]
    public void GetNPCByName()
    {
        var name = Request.QueryString["name"];

        if (!String.IsNullOrWhiteSpace(name))
        {
            var npc = NPCInfo.FromNPCName(name);

            if (npc is null)
            {
                Response.NotFound();
            }
            else
            {
                Response.Ok(npc);
            }
        }
        else
        {
            Response.BadRequest("Missing field: name");
        }
    }

    [ApiEndpoint("/npc/birthday")]
    public void GetNPCByBirthday()
    {
        var season = Request.QueryString["season"];

        if (!String.IsNullOrWhiteSpace(season))
        {
            var dayRaw = Request.QueryString["day"];

            if (!String.IsNullOrWhiteSpace(dayRaw))
            {
                if (Int32.TryParse(dayRaw, out var day))
                {
                    var npcs = NPCUtilities.GetNPCsByBirthday(season, day)
                        .Select(n => NPCInfo.FromNPCName(n.Name));

                    Response.Ok(npcs);
                }
                else
                {
                    Response.BadRequest("day must be numeric");
                }
            }
            else
            {
                Response.BadRequest("Missing field: day");
            }
        }
        else
        {
            Response.BadRequest("Missing field: season");
        }
    }

    [ApiEndpoint("/pets")]
    public void GetAllPets()
    {
        Response.Ok(NPCUtilities.GetAllNPCsOfType(NPCType.Pet)
            .Select(n => NPCInfo.FromNPCName(n.Name)
        ));
    }
}