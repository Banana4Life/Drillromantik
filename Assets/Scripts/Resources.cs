using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class Resources
{
    public readonly Dictionary<ItemType, BigInteger> Items = new Dictionary<ItemType, BigInteger>();
    
    public bool Add(Resources resources)
    {
        if (!HasResources(resources)) return false;
        AddNoCheck(resources);
        return true;
    }
    
    public Resources Add(params Item[] resources)
    {
        foreach (var resource in resources)
        {
            Items[resource.type] = Items.GetOrElse(resource.type) + resource.quantity;
        }

        return this;
    }

    public Resources AddNoCheck(Resources resources)
    {
        foreach (var toAdd in resources.Items)
        {
            Items[toAdd.Key] = Items.GetOrElse(toAdd.Key) + toAdd.Value;
        }
        return this;
    }

    public bool HasResources(ItemList resources)
    {
        foreach (var toAdd in resources.items)
        {
            if (toAdd.quantity < 0)
            {
                if (Items.GetOrElse(toAdd.type) + toAdd.quantity < 0)
                {
                    return false;
                }
            }
        }

        return true;
    }
    
    public bool HasResources(Resources resources)
    {
        foreach (var toAdd in resources.Items)
        {
            if (toAdd.Value < 0)
            {
                if (Items.GetOrElse(toAdd.Key) + toAdd.Value < 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void Mul(ItemType itemType, int factor)
    {
        Items[itemType] = Items.GetOrElse(itemType) * factor;
    }
    
    public Resources Mul(int factor)
    {
        foreach (var k in Items.Keys.ToList()) 
        {
            Items[k] *= factor;
        }

        return this;
    }

    public override string ToString()
    {
        return String.Join(" | ", Items.Select(e => e.Key + ": " + e.Value));
    }
}