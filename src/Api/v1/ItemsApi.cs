using StardewWebApi.Game;
using StardewWebApi.Game.Items;
using StardewWebApi.Server;
using StardewWebApi.Server.Routing;

namespace StardewWebApi.Api.V1;

[RequireLoadedGame]
public class ItemsApi : ApiControllerBase
{
    [Route("/items")]
    public void GetAllItems()
    {
        Response.Ok(ItemUtilities.GetAllItems().Select(i => BasicItem.FromItem(i)));
    }

    [Route("/items/id/{itemId}")]
    public void GetItemById(string itemId)
    {
        Response.Ok(BasicItem.FromItem(ItemUtilities.GetItemByFullyQualifiedId(itemId)));
    }

    [Route("/items/type/{itemType}")]
    public void GetAllItemsByType(string itemType)
    {
        Response.Ok(ItemUtilities.GetAllItemsByType(itemType).Select(i => BasicItem.FromItem(i)));
    }

    [Route("/items/type/{itemType}/id/{itemId}")]
    public void GetItemByTypeAndId(string itemType, string itemId)
    {
        Response.Ok(BasicItem.FromItem(ItemUtilities.GetItemByTypeAndId(itemType, itemId)));
    }
}