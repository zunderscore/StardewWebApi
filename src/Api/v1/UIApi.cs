using StardewWebApi.Game;
using StardewWebApi.Game.Items;
using StardewWebApi.Types;

namespace StardewWebApi.Api.V1;

public class UIApi : ApiControllerBase
{
    [ApiEndpoint("/ui/showHudMessage")]
    public void ShowHUDMessage()
    {
        var message = Request.QueryString["message"];
        var typeRaw = Request.QueryString["type"];
        var durationRaw = Request.QueryString["duration"];

        if (!String.IsNullOrWhiteSpace(message))
        {
            if (!String.IsNullOrWhiteSpace(typeRaw) && Enum.TryParse(typeRaw, true, out HUDMessageType type))
            {
                UIUtilities.ShowHUDMessage(
                    message,
                    type,
                    duration: Int32.TryParse(durationRaw, out var duration) ? duration : null
                );
            }
            else
            {
                UIUtilities.ShowHUDMessage(
                    message,
                    duration: Int32.TryParse(durationRaw, out var duration) ? duration : null
                );
            }
        }
        else
        {
            Response.BadRequest("Missing field: message");
        }
    }

    [ApiEndpoint("/ui/showHudMessage/item")]
    public void ShowHUDMessageForItem()
    {
        var itemId = Request.QueryString["itemId"];
        var itemName = Request.QueryString["itemName"];
        var message = Request.QueryString["message"];
        var amountRaw = Request.QueryString["amount"];
        var durationRaw = Request.QueryString["duration"];
        var typeKey = Request.QueryString["typeKey"];

        if (!String.IsNullOrWhiteSpace(itemId))
        {
            UIUtilities.ShowHUDMessageForItem(
                itemId,
                message,
                Int32.TryParse(amountRaw, out var amount) ? amount : 1,
                Int32.TryParse(durationRaw, out var duration) ? duration : null,
                typeKey
            );
        }
        else if (!String.IsNullOrWhiteSpace(itemName))
        {
            var item = ItemUtilities.GetItemByDisplayName(itemName);

            if (item != null)
            {
                UIUtilities.ShowHUDMessageForItem(
                    item.QualifiedItemId,
                    message,
                    Int32.TryParse(amountRaw, out var amount) ? amount : 1,
                    Int32.TryParse(durationRaw, out var duration) ? duration : null,
                    typeKey
                );
            }
            else
            {
                Response.BadRequest("No item with that name exists");
            }
        }
        else
        {
            Response.BadRequest("Either itemId or name must be specified");
        }
    }

    [ApiEndpoint("/ui/showHudMessage/large")]
    public void ShowLargeHUDMessage()
    {
        var message = Request.QueryString["message"];

        if (!String.IsNullOrWhiteSpace(message))
        {
            UIUtilities.ShowLargeHUDMessage(message);
        }
        else
        {
            Response.BadRequest("Missing field: message");
        }
    }

    [ApiEndpoint("/ui/showChatMessage")]
    public void ShowChatMessage()
    {
        var message = Request.QueryString["message"];
        var typeRaw = Request.QueryString["type"];

        if (!String.IsNullOrWhiteSpace(message))
        {
            if (!String.IsNullOrWhiteSpace(typeRaw) && Enum.TryParse(typeRaw, true, out ChatMessageType type))
            {
                UIUtilities.ShowChatMessage(message, type);
            }
            else
            {
                UIUtilities.ShowChatMessage(message);
            }
        }
        else
        {
            Response.BadRequest("Missing field: message");
        }
    }
}