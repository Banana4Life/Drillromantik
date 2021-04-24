using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public static class Tech
{
    public static Resources Resources = new Resources();

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
