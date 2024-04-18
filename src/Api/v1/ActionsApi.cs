using StardewValley;
using StardewWebApi.Game;
using StardewWebApi.Game.Actions;
using StardewWebApi.Game.Items;
using StardewWebApi.Server;

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
    public void GiveMoney(int amount = 1000)
    {
        PlayerActions.GiveMoney(amount);
        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/action/giveItem")]
    public void GiveItem(string? itemId = null, string? itemName = null, int amount = 1, int quality = 0)
    {
        Item? item;
        if (itemId is not null)
        {
            item = ItemUtilities.GetItemByFullyQualifiedId(itemId, amount, quality);
            if (item is null)
            {
                Response.BadRequest($"Item with ID '{itemId}' does not exist");
                return;
            }
        }
        else if (itemName is not null)
        {
            item = ItemUtilities.GetItemByDisplayName(itemName, amount, quality);
            if (item is null)
            {
                Response.BadRequest($"Item with name '{itemName}' does not exist");
                return;
            }
        }
        else
        {
            Response.BadRequest($"Either {nameof(itemId)} or {nameof(itemName)} must be specified");
            return;
        }

        PlayerActions.GiveItem(item);
        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/action/warpPlayer")]
    public void WarpPlayer(WarpLocation location, bool playWarpAnimation = true)
    {
        if (Enum.IsDefined(location))
        {
            Response.Ok(PlayerActions.WarpPlayer(location, playWarpAnimation));
        }
        else
        {
            Response.BadRequest("Invalid location specified");
        }
    }

    [ApiEndpoint("/action/petFarmAnimal")]
    public void PetFarmAnimal(string name)
    {
        Response.Ok(PlayerActions.PetFarmAnimal(name));
    }
}