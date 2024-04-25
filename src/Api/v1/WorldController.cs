using StardewWebApi.Game;
using StardewWebApi.Game.World;
using StardewWebApi.Server;
using StardewWebApi.Server.Routing;

namespace StardewWebApi.Api.V1;

[Route("/api/v1/world")]
public class WorldController : ApiControllerBase
{
    [Route("/"), RequireLoadedGame]
    public void GetWorldInfo()
    {
        Response.Ok(World.GetCurrent());
    }

    [Route("/actions/playSound/{name}")]
    public void PlaySound(string name, int? pitch = null)
    {
        Response.Ok(WorldActions.PlaySound(name, pitch));
    }
}