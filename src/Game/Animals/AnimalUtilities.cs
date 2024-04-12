using StardewValley;

namespace StardewWebApi.Game.Animals;

public static class AnimalUtilities
{
    public static FarmAnimal? GetFarmAnimalByName(string name)
    {
        var nameLower = name.ToLower();
        FarmAnimal? foundAnimal = null;

        Utility.ForEachLocation((location) =>
        {
            foreach (var animal in location.animals.Values)
            {
                if (animal.Name.ToLower() == nameLower)
                {
                    foundAnimal = animal;
                    return false;
                }
            }
            return true;
        });

        return foundAnimal;
    }
}