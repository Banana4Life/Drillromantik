using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class Upgrades
{
    [FormerlySerializedAs("aquiredTickUpgrades")] public int aquired;
    [FormerlySerializedAs("TickUpgrades")] public List<Upgrade> upgrades = new List<Upgrade>();

    public Upgrades AddUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        return this;
    }

    public Resources Calculate(Resources toAdd)
    {
        for (var i = 0; i < aquired; i++)
        {
            var upgrade = upgrades[i];
            toAdd = upgrade.Apply(toAdd);
        }

        return toAdd;
    }


    public Upgrade Next()
    {
        return upgrades[aquired];
    }
    
    public bool AcquireNext()
    {
        if (upgrades.Count > aquired)
        {
            var upgrade = upgrades[aquired];
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