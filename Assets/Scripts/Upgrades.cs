using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class Upgrades
{
    public UpgradeChain[] upgradeChains;
   
    public Resources Calculate(Resources toAdd)
    {
        foreach (var upgradeChain in upgradeChains)
        {
            for (var i = 0; i < upgradeChain.aquired; i++)
            {
                var upgrade = upgradeChain.chain[i];
                toAdd = upgrade.Apply(toAdd);
            }    
        }
        

        return toAdd;
    }

    public Upgrade Next(int chain = 0)
    {
        return upgradeChains[chain].Next();
    }
    
    public bool AcquireNext(int chain = 0)
    {
        return upgradeChains[chain].AquireNext();
    }
    
    public bool HasUpgradeAvailable(int chain = 0)
    {
        if (upgradeChains.Length > chain)
        {
            return upgradeChains[chain].HasUpgradeAvailable();
        }

        return false;

    }

    public Upgrades Clone()
    {
        var upgrades = new Upgrades();
        upgrades.upgradeChains = new UpgradeChain[upgradeChains.Length];
        for (var i = 0; i < upgradeChains.Length; i++)
        {
            var upgradeChain = upgradeChains[i];
            upgrades.upgradeChains[i] = new UpgradeChain();
            upgrades.upgradeChains[i].aquired = upgradeChain.aquired;
            upgrades.upgradeChains[i].chain = upgradeChain.chain;
            upgrades.upgradeChains[i].name = upgradeChain.name;
        }
        return upgrades;
    }

    public bool HasAquiredAny()
    {
        foreach (var upgradeChain in upgradeChains)
        {
            if (upgradeChain.aquired > 0)
            {
                return true;
            }
        }

        return false;
    }
}

[Serializable]
public class UpgradeChain
{
    public String name;
    public int aquired;
    public List<Upgrade> chain = new List<Upgrade>();

    public Upgrade Next()
    {
        return chain[aquired];
    }

    public bool HasUpgradeAvailable()
    {
        return chain.Count > aquired;
    }
    
    public bool AquireNext()
    {
        if (HasUpgradeAvailable())
        {
            var upgrade = Next();
            if (Global.Resources.HasResources(upgrade.cost))
            {
                Global.Resources.Add(upgrade.cost.items);
                aquired++;
                return true;
            }
        }

        return false;
    }
    
    
}