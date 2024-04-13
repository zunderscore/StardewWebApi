using StardewValley;

namespace StardewWebApi.Game;

public enum HUDMessageType
{
    Achievement = 1,
    NewQuest = 2,
    Error = 3,
    Stamina = 4,
    Health = 5,
    Screenshot = 6
}

public enum ChatMessageType
{
    None = 0,
    Info = 1,
    Error = 2
}

public static class UIUtilities
{
    public static void ShowHUDMessage(string message, HUDMessageType type = HUDMessageType.NewQuest, int? duration = null)
    {
        var hudMessage = new HUDMessage(message, (int)type);

        if (duration.HasValue)
        {
            hudMessage.timeLeft = duration.Value * 1000f;
        }

        Game1.addHUDMessage(hudMessage);
    }

    public static void ShowHUDMessageForItem(Item item, string? message = null, int? duration = null, string? typeKey = null)
    {
        var hudMessage = HUDMessage.ForItemGained(item, item.Stack, typeKey);

        if (!String.IsNullOrWhiteSpace(message))
        {
            hudMessage.message = message;
            hudMessage.type = $"{hudMessage.type}{message}";
        }

        if (duration.HasValue)
        {
            hudMessage.timeLeft = duration.Value * 1000f;
        }

        Game1.addHUDMessage(hudMessage);
    }

    public static void ShowLargeHUDMessage(string message, int? duration = null)
    {
        var hudMessage = HUDMessage.ForCornerTextbox(message);

        if (duration.HasValue)
        {
            hudMessage.timeLeft = duration.Value * 1000f;
        }

        Game1.addHUDMessage(hudMessage);
    }

    public static void ShowChatMessage(string message, ChatMessageType type = ChatMessageType.None, Microsoft.Xna.Framework.Color? color = null)
    {
        switch (type)
        {
            case ChatMessageType.Info:
                Game1.chatBox.addInfoMessage(message);
                break;

            case ChatMessageType.Error:
                Game1.chatBox.addErrorMessage(message);
                break;

            case ChatMessageType.None:
            default:
                Game1.chatBox.addMessage(message, color ?? Microsoft.Xna.Framework.Color.White);
                break;
        }
    }
}