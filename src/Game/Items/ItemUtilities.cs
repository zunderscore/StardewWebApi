using StardewValley;
using StardewValley.Extensions;

namespace StardewWebApi.Game.Items;

public static class ItemUtilities
{
    public static Item? GetItemByFullyQualifiedId(string itemId, int amount = 1, int quality = 0)
    {
        var itemMetadata = ItemRegistry.ResolveMetadata(itemId);

        return itemMetadata?.Exists() == true
            ? itemMetadata.CreateItem(amount, quality)
            : null;
    }

    public static Item? GetItemByDisplayName(string itemName, int amount = 1, int quality = 0)
    {
        itemName = itemName.ToLower();

        try
        {
            var parsedItemData = ItemRegistry.ItemTypes.Select(it =>
            {
                return it.GetAllData().FirstOrDefault(i =>
                    i.DisplayName.ToLower() == itemName
                );
            }).FirstOrDefault(i => i is not null);

            return parsedItemData is not null
                ? GetItemByFullyQualifiedId(parsedItemData.QualifiedItemId, amount, quality)
                : null;
        }
        catch
        {
            return null;
        }
    }
}