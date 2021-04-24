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
        Func = r => { return r; }
    };
}


[Serializable]
public class Structure
{
    public String name;
    public GameObject prefab;
    private ExploitationScript _exploitation;
    private LimitScript _limit;
    private BuildScript _build;
    private bool _init2 = false;

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
        if (!_init2 && prefab)
        {
            _init2 = true;
            _exploitation = prefab.GetComponent<ExploitationScript>();
            _build = prefab.GetComponent<BuildScript>();
            _limit = prefab.GetComponent<LimitScript>();
        }
    }

    public bool CanBuild(List<GameObject> neighborTiles)
    {
        Init();
        if (_limit)
        {
            if (_limit.BuildLimited(neighborTiles))
            {
                return false;
            }
        }
        if (_build)
        {
            return _build.canBuild(neighborTiles);
        }

        Debug.Log("Build deny bypassed for " + name);
        return true; // TODO prevent building?
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

    public Item(ItemType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }

    public override string ToString()
    {
        return $"{nameof(type)}: {type}, {nameof(quantity)}: {quantity}";
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