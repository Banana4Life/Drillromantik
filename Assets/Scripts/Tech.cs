using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Tech
{
    public static Building WOOD_CUTTER = new Building{Costs = new Resources{Wood = 10, Stone = 10}, Exploitation = () => new Resources{Wood = 1}};
    public static Building CHARCOAL_BURNER = new Building{Costs = new Resources{Wood = 20}, Exploitation = () => new Resources{Wood = -1, Charcoal = 1}};
    public static Building SMITH = new Building{Costs = new Resources{Stone = 20}, Exploitation = () => new Resources{Charcoal = -1, Copper = -1, CopperTools = 1}};
    public static Building MARKET = new Building{Costs = new Resources{Wood = 500, Stone = 500}};
    public static Building LOOKOUT_TOWER = new Building{Costs = new Resources{Wood = 100, Stone = 20}};
    public static Building RESEARCH_FACILITY = new Building{Costs = new Resources{Wood = 500, Stone = 500, Charcoal = 500, CopperTools = 10, Gold = 100}};

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

public class Building
{
    public Resources Costs{get;set;}
    public Func<Resources> Exploitation{get;set;}

    public Resources ExploitResources(Upgrades upgrades)
    {
        if (Exploitation == null)
        {
            return new Resources();
        }

        return upgrades.Apply(Exploitation.Invoke());
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

public class Resources
{ 
    public int Wood{ get;set;}
    public int Stone{ get;set;}
    public int Charcoal{ get;set;}
    public int Coal{ get;set;}
    public int Copper{ get;set;}
    public int CopperTools{ get;set;}
    public int Iron{ get;set;}
    public int IronTools{ get;set;}
    public int SteelTools{ get;set;}
    public int Gold{ get;set;}
    public int Diamond{ get;set;}


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