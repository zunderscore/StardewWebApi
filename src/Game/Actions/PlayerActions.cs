using StardewValley;
using StardewWebApi.Game.Animals;
using StardewWebApi.Game.Items;
using StardewWebApi.Server;

namespace StardewWebApi.Game.Actions;

public enum WarpLocation
{
    Random = 0,
    Farm,
    Beach,
    Mountain,
    Desert,
    Island
}

public static class PlayerActions
{
    public static void RefillEnergy()
    {
        Game1.player.Stamina = Game1.player.MaxStamina;
    }

    public static void PassOut()
    {
        Game1.player.Stamina = -16;
    }

    public static void FullyHeal()
    {
        Game1.player.health = Game1.player.maxHealth;
    }

    public static void KnockOut()
    {
        Game1.player.health = 0;
    }

    public static void GiveMoney(int amount)
    {
        Game1.player.addUnearnedMoney(amount);
    }

    private static WarpLocation GetNewWarpLocation()
    {
        var locations = new List<WarpLocation>() {
            WarpLocation.Farm,
            WarpLocation.Beach,
            WarpLocation.Mountain
        };

        if (Game1.player.mailReceived.Contains("ccVault"))
        {
            locations.Add(WarpLocation.Desert);
        }

        if (Game1.player.mailReceived.Contains("willyBoatFixed"))
        {
            locations.Add(WarpLocation.Island);
        }

        return locations[Random.Shared.Next(0, locations.Count)];
    }

    public static ActionResult WarpPlayer(WarpLocation location = WarpLocation.Random)
    {
        var result = new ActionResult(true);

        if (location == WarpLocation.Random)
        {
            location = GetNewWarpLocation();
        }

        try
        {
            if ((location == WarpLocation.Desert && !Game1.player.mailReceived.Contains("ccVault") && !Game1.player.mailReceived.Contains("jojaVault"))
                || (location == WarpLocation.Island && !Game1.player.mailReceived.Contains("willyBoatFixed"))
            )
            {
                throw new Exception("Can't warp to that location yet!");
            }

            switch (location)
            {
                case WarpLocation.Farm:
                    Game1.player.resetState();
                    Game1.warpFarmer("Farm", 48, 7, false);
                    break;

                case WarpLocation.Beach:
                    Game1.player.resetState();
                    Game1.warpFarmer("Beach", 20, 4, false);
                    break;

                case WarpLocation.Mountain:
                    Game1.player.resetState();
                    Game1.warpFarmer("Mountain", 31, 20, false);
                    break;

                case WarpLocation.Desert:
                    Game1.player.resetState();
                    Game1.warpFarmer("Desert", 35, 43, false);
                    break;

                case WarpLocation.Island:
                    Game1.player.resetState();
                    Game1.warpFarmer("IslandSouth", 11, 11, false);
                    break;

                default:
                    throw new Exception("Unknown warp location");
            }
        }
        catch (Exception ex)
        {
            SMAPIWrapper.Instance.Log($"Error warping player: {ex.Message}", StardewModdingAPI.LogLevel.Error);
            result = new(false, new ErrorResponse(ex.Message));
        }

        return result;
    }

    public static ActionResult PetFarmAnimal(string animalName)
    {
        var result = new ActionResult(true);
        var animal = AnimalUtilities.GetFarmAnimalByName(animalName);

        if (animal != null)
        {
            if (animal.wasAutoPet.Value)
            {
                result = new(false, new ErrorResponse($"{animal.Name} was already auto-pet today"));
            }
            else if (animal.wasPet.Value)
            {
                result = new(false, new ErrorResponse($"{animal.Name} was already pet today"));
            }
            else
            {
                Game1.player.FarmerSprite.PauseForSingleAnimation = false;
                animal.pet(Game1.player);
            }
        }
        else
        {
            result = new(false, new ErrorResponse($"Could not find a farm animal named {animalName}"));
        }

        return result;
    }

    public static void GiveItem(Item item)
    {
        Game1.player.addItemByMenuIfNecessary(item);
    }

    public static void GiveItems(IEnumerable<Item> items)
    {
        Game1.player.addItemsByMenuIfNecessary(new List<Item>(items));
    }
}