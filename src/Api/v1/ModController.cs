using StardewWebApi.Game;
using StardewWebApi.Game.Mods;
using StardewWebApi.Server;
using StardewWebApi.Server.Routing;

namespace StardewWebApi.Api.V1;

[Route("/api/v1/mods")]
public class ModController : ApiControllerBase
{
    [Route("/")]
    public void GetMods()
    {
        Response.Ok(SMAPIWrapper.Instance.GetAllMods().Select(m => new ModStub(m)));
    }

    [Route("/{modId}")]
    public void GetModById(string modId)
    {
        Response.Ok(SMAPIWrapper.Instance.GetAllMods()
            .FirstOrDefault(m => m.Manifest.UniqueID.ToLower() == modId.ToLower())
        );
    }
}