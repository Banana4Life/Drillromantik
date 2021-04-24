using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Tech
{
    
    public static Resources Resources = new Resources();

    public static Upgrade SHARP_AXES = new Upgrade
    {
        Func = r =>
        {
            r.Wood *= 2;
            return r;
        }
    };
}


[Serializable]
public class Structure
{
    public String name;
    public GameObject prefab;
    private ExploitationScript _exploitation;
    public Resources costs;
    private bool _init = false;

    public float spawnWeight = 0;
 
    public void TickTile(Upgrades upgrades)
    {
        Init();
        if (_exploitation && _exploitation.tick)
        {
            Tech.Resources.Add(_exploitation.Calculate(upgrades));
            Debug.Log(Tech.Resources);
        }
    }

    public void Init()
    {
        if (!_init && prefab)
        {
            _init = true;
            _exploitation = prefab.GetComponent<ExploitationScript>();
        }
    }
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

[System.Serializable]
public class Item
{
    public ItemType type;
    public int quantity;
}

public enum ItemType
{
    WOOD, STONE, CHARCOAL, COAL, COPPER, COPPER_TOOLS, IRON, IRON_TOOLS, STEEL_TOOLS, GOLD, DIAMOND
}

[System.Serializable]
public class Resources
{ 
    public int Wood;
    public int Stone;
    public int Charcoal;
    public int Coal;
    public int Copper;
    public int CopperTools;
    public int Iron;
    public int IronTools;
    public int SteelTools;
    public int Gold;
    public int Diamond;

    public Item[] items;

    public override string ToString()
    {
        return $"{nameof(Wood)}: {Wood}, {nameof(Stone)}: {Stone}, {nameof(Charcoal)}: {Charcoal}, {nameof(Coal)}: {Coal}, {nameof(Copper)}: {Copper}, {nameof(CopperTools)}: {CopperTools}, {nameof(Iron)}: {Iron}, {nameof(IronTools)}: {IronTools}, {nameof(SteelTools)}: {SteelTools}, {nameof(Gold)}: {Gold}, {nameof(Diamond)}: {Diamond}";
    }

    public bool Add(Resources resources)
    {
        foreach (var toAdd in resources.items)
        {
            if (toAdd.quantity < 0)
            {
                foreach (var item in items)
                {
                    if (item.quantity + toAdd.quantity < 0)
                    {
                        return false;
                    }
                }
                
            }    
        }
        
        foreach (var toAdd in resources.items)
        {
            foreach (var item in items)
            {
                if (item.type == toAdd.type)
                {
                    item.quantity += toAdd.quantity;
                    break;
                }
            }
        }

        return true;
    }
}