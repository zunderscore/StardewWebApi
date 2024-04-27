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
            return new ActionResult(false, new ErrorResponse(ex.Message));
        }
    }

    public static ActionResult PlayMusicTrack(string name)
    {
        try
        {
            if (Game1.soundBank.Exists(name))
            {
                Game1.changeMusicTrack(name);
                return new ActionResult(true);
            }
            else
            {
                return new ActionResult(false, new ErrorResponse($"Track {name} not found in sound bank"));
            }
        }
        catch (Exception ex)
        {
            SMAPIWrapper.LogError($"Error playing music track {name}: {ex.Message}");
            return new ActionResult(false, new ErrorResponse(ex.Message));
        }
    }

    public static ActionResult StopMusicTrack()
    {
        try
        {
            Game1.stopMusicTrack(StardewValley.GameData.MusicContext.Default);
            return new ActionResult(true);
        }
        catch (Exception ex)
        {
            SMAPIWrapper.LogError($"Error stopping music track: {ex.Message}");
            return new ActionResult(false, new ErrorResponse(ex.Message));
        }
    }
}