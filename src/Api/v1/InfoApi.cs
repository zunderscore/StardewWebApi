using StardewWebApi.Game.Info;
using StardewWebApi.Game.Players;
using StardewWebApi.Types;

namespace StardewWebApi.Api.V1;

[RequireLoadedGame]
public class InfoApi : ApiControllerBase
{
    [ApiEndpoint("/info/player")]
    public void GetPlayerInfo()
    {
        Response.Ok(Player.FromMain());
    }

    [ApiEndpoint("/info/world")]
    public void GetWorldInfo()
    {
        Response.Ok(GameInfo.GetWorldInfo());
    }
}