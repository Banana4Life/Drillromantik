using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public static class Tech
{
    public static Resources2 Resources = new Resources2();

    public static Upgrade SHARP_AXES = new Upgrade
    {
        Func = r => { return r; }
    };
}


public class Upgrades
{
    private List<Upgrade> List = new List<Upgrade>();

    public Upgrades Add(Upgrade upgrade)
    {
        List.Add(upgrade);
        return this;
    }

    public Resources Apply(Resources resources)
    {
        foreach (var upgrade in List)
        {
            resources = upgrade.Func.Invoke(resources);
        }

        return resources;
    }
}

public class Upgrade
{
    public Func<Resources, Resources> Func { get; set; }
}

public class Research
{
    public Resources costs;

    public Research(Resources costs)
    {
        this.costs = costs;
    }
}


public class Resources2
{
    private Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();
    
    public bool Add(Resources resources)
    {
        if (!HasResources(resources)) return false;
        AddNoCheck(resources);
        return true;
    }

    public Resources2 AddNoCheck(Resources resources)
    {
        foreach (var toAdd in resources.items)
        {
            int val;
            items[toAdd.type] = items.TryGetValue(toAdd.type, out val) ? val : toAdd.quantity;
        }
        return this;
    }

    private bool HasResources(Resources resources)
    {
        foreach (var toAdd in resources.items)
        {
            if (toAdd.quantity < 0)
            {
                int val;
                if ((items.TryGetValue(toAdd.type, out val) ? val : 0) + toAdd.quantity < 0)
                {
                    Debug.Log("Resource not available: " + toAdd.type);
                    return false;
                }
            }
        }

        return true;
    }

    public void Mul(ItemType itemType, int factor)
    {
        int val;
        items[itemType] = items.TryGetValue(itemType, out val) ? val : 0 * factor;
    }
}