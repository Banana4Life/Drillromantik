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

    public bool UpgradeTick()
    {
        if (TickUpgrades.Count > aquiredTickUpgrades)
        {
            var upgrade = TickUpgrades[aquiredTickUpgrades];
            if (Global.Resources.HasResources(upgrade.cost))
            {
                Global.Resources.Add(upgrade.cost.items);
                aquiredTickUpgrades++;
                return true;
            }
        }

        return false;
    }

    public bool UpgradeClick()
    {
        if (ClickUpgrades.Count > aquiredClickUpgrades)
        {
            var upgrade = ClickUpgrades[aquiredClickUpgrades];
            if (Global.Resources.HasResources(upgrade.cost))
            {
                Global.Resources.Add(upgrade.cost.items);
                aquiredClickUpgrades++;
                return true;
            }
        }

        return false;
    }
}