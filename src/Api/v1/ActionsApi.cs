using StardewWebApi.Game.Actions;
using StardewWebApi.Types;

namespace StardewWebApi.Api.V1;

[RequireLoadedGame]
public class ActionsApi : ApiControllerBase
{
    [ApiEndpoint("/action/refillEnergy")]
    public void RefillEnergy()
    {
        PlayerActions.RefillEnergy();
        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/action/passOut")]
    public void PassOut()
    {
        PlayerActions.PassOut();
        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/action/fullyHeal")]
    public void FullyHeal()
    {
        PlayerActions.FullyHeal();
        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/action/knockOut")]
    public void KnockOut()
    {
        PlayerActions.KnockOut();
        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/action/giveMoney")]
    public void GiveMoney()
    {
        var amountRaw = Request.QueryString["amount"];

        if (!String.IsNullOrWhiteSpace(amountRaw))
        {
            if (int.TryParse(amountRaw, out var amount))
            {
                PlayerActions.GiveMoney(amount);
                Response.Ok(new ActionResult(true));
            }
            else
            {
                Response.BadRequest("amount must be valid integer");
            }
        }
        else
        {
            Response.BadRequest("Missing field: amount");
        }
    }

    [ApiEndpoint("/action/giveItem")]
    public void GiveItem()
    {
        var itemId = Request.QueryString["itemId"];
        var name = Request.QueryString["name"];
        var amountRaw = Request.QueryString["amount"];
        var qualityRaw = Request.QueryString["quality"];

        if (!String.IsNullOrWhiteSpace(itemId))
        {
            PlayerActions.GiveItemByItemId(
                itemId,
                Int32.TryParse(amountRaw, out var amount) ? amount : 1,
                Int32.TryParse(qualityRaw, out var quality) ? quality : 0
            );

            Response.Ok(new ActionResult(true));
        }
        else if (!String.IsNullOrWhiteSpace(name))
        {
            PlayerActions.GiveItemByDisplayName(
                name,
                Int32.TryParse(amountRaw, out var amount) ? amount : 1,
                Int32.TryParse(qualityRaw, out var quality) ? quality : 0
            );

            Response.Ok(new ActionResult(true));
        }
        else
        {
            Response.BadRequest("Either itemId or name must be specified");
        }
    }

    [ApiEndpoint("/action/warpPlayer")]
    public void WarpPlayer()
    {
        var locationRaw = Request.QueryString["location"];

        if (!String.IsNullOrWhiteSpace(locationRaw))
        {
            if (Enum.TryParse(locationRaw, out WarpLocation location))
            {
                Response.Ok(PlayerActions.WarpPlayer(location));
            }
            else
            {
                Response.BadRequest("Invalid location specified");
            }
        }
        else
        {
            Response.BadRequest("Missing field: location");
        }
    }

    [ApiEndpoint("/action/petFarmAnimal")]
    public void PetFarmAnimal()
    {
        var name = Request.QueryString["name"];

        if (!String.IsNullOrWhiteSpace(name))
        {
            Response.Ok(PlayerActions.PetFarmAnimal(name));
        }
        else
        {
            Response.BadRequest("Missing field: name");
        }
    }
}