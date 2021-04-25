using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ItemList
{
    public Item[] items;

    public ItemList(params Item[] items)
    {
        this.items = items;
    }

    public override string ToString()
    {
        return String.Join(" ", items.Select(e => e.ToString()));
    }

}

public static class Helper
{
    public static Item of(this ItemType type, int quantity)
    {
        return new Item(type, quantity);
    }
}

public enum ItemType
{
    WOOD,
    STONE,
    CHARCOAL,
    COAL,
    COPPER,
    COPPER_TOOLS,
    IRON,
    IRON_TOOLS,
    STEEL_TOOLS,
    GOLD,
    DIAMOND
}


[System.Serializable]
public class Item
{
    public ItemType type;
    public int quantity;

    public Item(ItemType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }

    public override string ToString()
    {
        return $"{type} : {quantity}";
    }
}
