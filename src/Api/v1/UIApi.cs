using StardewValley;
using StardewWebApi.Game;
using StardewWebApi.Game.Items;
using StardewWebApi.Server;

namespace StardewWebApi.Api.V1;

public class UIApi : ApiControllerBase
{
    [ApiEndpoint("/ui/showHudMessage")]
    public void ShowHUDMessage(string message, HUDMessageType? type = null, int? duration = null)
    {
        if (type.HasValue)
        {
            if (Enum.IsDefined(type.Value))
            {
                UIUtilities.ShowHUDMessage(message, type.Value, duration);
            }
            else
            {
                Response.BadRequest($"Invalid type specified");
                return;
            }
        }
        else
        {
            UIUtilities.ShowHUDMessage(message, duration: duration);
        }

        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/ui/showHudMessage/item")]
    public void ShowHUDMessageForItem(
        string message,
        int? duration = null,
        string? itemId = null,
        string? itemName = null,
        int amount = 1,
        int quality = 0,
        string? typeKey = null
    )
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

        UIUtilities.ShowHUDMessageForItem(
            item,
            message,
            duration,
            typeKey
        );
        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/ui/showHudMessage/large")]
    public void ShowLargeHUDMessage(string message)
    {
        UIUtilities.ShowLargeHUDMessage(message);
        Response.Ok(new ActionResult(true));
    }

    [ApiEndpoint("/ui/showChatMessage")]
    public void ShowChatMessage(string message, ChatMessageType type = ChatMessageType.None)
    {
        UIUtilities.ShowChatMessage(message, type);
        Response.Ok(new ActionResult(true));
    }
}