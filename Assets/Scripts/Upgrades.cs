using System;
using System.Collections.Generic;

[Serializable]
public class Upgrades
{
    public int aquiredClickUpgrades;
    public int aquiredTickUpgrades;
    public List<Upgrade> ClickUpgrades = new List<Upgrade>();
    public List<Upgrade> TickUpgrades = new List<Upgrade>();

    public Upgrades AddTickUpgrade(Upgrade upgrade)
    {
        TickUpgrades.Add(upgrade);
        return this;
    }
    
    public Upgrades AddClickUpgrade(Upgrade upgrade)
    {
        ClickUpgrades.Add(upgrade);
        return this;
    }

    public Resources CalculateTick()
    {
        var toAdd = new Resources();
        for (var i = 0; i < aquiredTickUpgrades; i++)
        {
            var upgrade = TickUpgrades[i];
            toAdd = upgrade.Apply(toAdd);
        }

        return toAdd;
    }

    public Resources CalculateClick()
    {
        var toAdd = new Resources();
        for (var i = 0; i < aquiredClickUpgrades; i++)
        {
            var upgrade = ClickUpgrades[i];
            toAdd = upgrade.Apply(toAdd);
        }

        return toAdd;
    }

    public void UpgradeTick()
    {
        if (TickUpgrades.Count > aquiredClickUpgrades)
        {
            var upgrade = TickUpgrades[aquiredTickUpgrades+1];
            if (Global.Resources.HasResources(upgrade.cost))
            {
                Global.Resources.Add(upgrade.cost.items);
                aquiredTickUpgrades++;    
            }
        }
    }
}