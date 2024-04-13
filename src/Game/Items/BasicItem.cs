using StardewValley;

namespace StardewWebApi.Game.Items;

public class BasicItem
{
    private readonly Item _item;

    private BasicItem(Item item)
    {
        _item = item;
    }

    public static BasicItem? FromItem(Item? item)
    {
        return item is not null ? new(item) : null;
    }

    public string ItemId => _item.ItemId;
    public string TypeDefinitionId => _item.TypeDefinitionId;
    public string QualifiedItemId => _item.QualifiedItemId;
    public string Name => _item.Name;
    public string DisplayName => _item.DisplayName;
    public int Quality => _item.Quality;
    public int Category => _item.Category;
    public int StackSize => _item.Stack;
}