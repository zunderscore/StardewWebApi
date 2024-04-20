using StardewWebApi.Game;
using StardewWebApi.Game.Info;
using StardewWebApi.Game.Players;
using StardewWebApi.Server;
using StardewWebApi.Server.Routing;

namespace StardewWebApi.Api.V1;

[RequireLoadedGame]
public class InfoApi : ApiControllerBase
{
    [Route("/info/player")]
    public void GetPlayerInfo()
    {
        Response.Ok(Player.FromMain());
    }

    [Route("/info/world")]
    public void GetWorldInfo()
    {
        Response.Ok(GameInfo.GetWorldInfo());
    }
}