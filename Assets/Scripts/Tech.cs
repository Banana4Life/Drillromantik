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

[System.Serializable]
public class Structure
{
    public String name;
    public GameObject prefab;
    public Resources costs;
    public Resources exploitation;
    public Resources clickExploitation;

    public float spawnWeight = 0;
    
    public Resources ExploitResources(Upgrades upgrades)
    {
        if (exploitation == null)
        {
            return new Resources();
        }

        return upgrades.Apply(exploitation);
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


    public override string ToString()
    {
        return $"{nameof(Wood)}: {Wood}, {nameof(Stone)}: {Stone}, {nameof(Charcoal)}: {Charcoal}, {nameof(Coal)}: {Coal}, {nameof(Copper)}: {Copper}, {nameof(CopperTools)}: {CopperTools}, {nameof(Iron)}: {Iron}, {nameof(IronTools)}: {IronTools}, {nameof(SteelTools)}: {SteelTools}, {nameof(Gold)}: {Gold}, {nameof(Diamond)}: {Diamond}";
    }

    public void Add(Resources resources)
    {
        if (resources.Wood < 0 && Wood + resources.Wood < 0) return;
        if (resources.Stone < 0 && Stone + resources.Stone < 0) return;
        if (resources.Charcoal < 0 && Charcoal + resources.Charcoal < 0) return;
        if (resources.Coal < 0 && Coal + resources.Coal < 0) return;
        if (resources.Copper < 0 && Copper + resources.Copper < 0) return;
        if (resources.CopperTools < 0 && CopperTools + resources.CopperTools < 0) return;
        if (resources.Iron < 0 && Iron + resources.Iron < 0) return;
        if (resources.IronTools < 0 && IronTools + resources.IronTools < 0) return;
        if (resources.SteelTools < 0 && SteelTools + resources.SteelTools < 0) return;
        if (resources.Gold < 0 && Gold + resources.Gold < 0) return;
        if (resources.Diamond < 0 && Diamond + resources.Diamond < 0) return;
        
        Wood += resources.Wood;
        Stone += resources.Stone;
        Charcoal += resources.Charcoal;
        Coal += resources.Coal;
        Copper += resources.Copper;
        CopperTools += resources.CopperTools;
        Iron += resources.Iron;
        IronTools += resources.IronTools;
        SteelTools += resources.SteelTools;
        Gold += resources.Gold;
        Diamond += resources.Diamond;
    }
}