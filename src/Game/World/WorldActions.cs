using StardewValley;
using StardewWebApi.Server;

namespace StardewWebApi.Game.World;

public static class WorldActions
{
    public static ActionResult PlaySound(string name, int? pitch = null)
    {
        try
        {
            var result = Game1.playSound(name, pitch);
            return new ActionResult(result);
        }
        catch (Exception ex)
        {
            SMAPIWrapper.LogError($"Error playing sound {name}: {ex.Message}");
            return new ActionResult(false, ex.Message);
        }
    }
}