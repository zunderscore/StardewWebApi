using StardewWebApi.Game;
using StardewWebApi.Game.Actions;
using StardewWebApi.Game.Items;
using StardewWebApi.Server;
using StardewWebApi.Server.Routing;

namespace StardewWebApi.Api.V1;

[RequireLoadedGame]
[Route("/action")]
public class ActionsApi : ApiControllerBase
{
    [Route("/refillEnergy")]
    public void RefillEnergy()
    {
        PlayerActions.RefillEnergy();
        Response.Ok(new ActionResult(true));
    }

    [Route("/passOut")]
    public void PassOut()
    {
        PlayerActions.PassOut();
        Response.Ok(new ActionResult(true));
    }

    [Route("/fullyHeal")]
    public void FullyHeal()
    {
        PlayerActions.FullyHeal();
        Response.Ok(new ActionResult(true));
    }

    [Route("/knockOut")]
    public void KnockOut()
    {
        PlayerActions.KnockOut();
        Response.Ok(new ActionResult(true));
    }

    [Route("/giveMoney/{amount}")]
    public void GiveMoney(int amount = 1000)
    {
        PlayerActions.GiveMoney(amount);
        Response.Ok(new ActionResult(true));
    }

    [Route("/giveItem/id/{itemId}")]
    public void GiveItemById(string itemId, int amount = 1, int quality = 0)
    {
        var item = ItemUtilities.GetItemByFullyQualifiedId(itemId, amount, quality);
        if (item is null)
        {
            Response.BadRequest($"Item with ID '{itemId}' does not exist");
            return;
        }

        PlayerActions.GiveItem(item);
        Response.Ok(new ActionResult(true, BasicItem.FromItem(item)));
    }

    [Route("/giveItem/name/{itemName}")]
    public void GiveItemByName(string itemName, int amount = 1, int quality = 0)
    {
        var item = ItemUtilities.GetItemByDisplayName(itemName, amount, quality);
        if (item is null)
        {
            Response.BadRequest($"Item with name '{itemName}' does not exist");
            return;
        }

        PlayerActions.GiveItem(item);
        Response.Ok(new ActionResult(true));
    }

    [Route("/warpPlayer/{location}")]
    public void WarpPlayer(WarpLocation location, bool playWarpAnimation = true)
    {
        Response.Ok(PlayerActions.WarpPlayer(location, playWarpAnimation));
    }

    [Route("/petFarmAnimal/{name}")]
    public void PetFarmAnimal(string name)
    {
        Response.Ok(PlayerActions.PetFarmAnimal(name));
    }
}