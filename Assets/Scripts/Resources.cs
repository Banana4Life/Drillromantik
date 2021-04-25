using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Resources
{
    private Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();
    
    public bool Add(Resources resources)
    {
        if (!HasResources(resources)) return false;
        AddNoCheck(resources);
        return true;
    }
    
    public Resources Add(params Item[] resources)
    {
        int val;
        foreach (var resource in resources)
        {
            items[resource.type] = (items.TryGetValue(resource.type, out val) ? val : 0) + resource.quantity;
        }

        return this;
    }

    public Resources AddNoCheck(Resources resources)
    {
        int val;
        foreach (var toAdd in resources.items)
        {
            items[toAdd.Key] = (items.TryGetValue(toAdd.Key, out val) ? val : 0) + toAdd.Value;
        }
        return this;
    }

    public bool HasResources(ItemList resources)
    {
        foreach (var toAdd in resources.items)
        {
            if (toAdd.quantity < 0)
            {
                if ((items.TryGetValue(toAdd.type, out var val) ? val : 0) + toAdd.quantity < 0)
                {
                    Debug.Log("Resource not available: " + toAdd.type);
                    return false;
                }
            }
        }

        return true;
    }
    
    public bool HasResources(Resources resources)
    {
        foreach (var toAdd in resources.items)
        {
            if (toAdd.Value < 0)
            {
                if ((items.TryGetValue(toAdd.Key, out var val) ? val : 0) + toAdd.Value < 0)
                {
                    Debug.Log("Resource not available: " + toAdd.Key);
                    return false;
                }
            }
        }

        return true;
    }

    public void Mul(ItemType itemType, int factor)
    {
        items[itemType] = (items.TryGetValue(itemType, out var val) ? val : 0) * factor;
    }

    public override string ToString()
    {
        return String.Join(" | ", items.Select(e => e.Key + ": " + e.Value));
    }
}