using System;
using System.Collections.Generic;
using System.Linq;
using TileGrid;
using UnityEngine;

[Serializable]
public class Structure
{
    public String name;
    public StructureType type;
    public GameObject prefab;
    public Texture texture;
    public float spawnWeight = 0;
    
    public Upgrades buildingUpgrades;
    public Upgrades clickUpgrades;
    public Upgrades globalUpgrades;

    public bool buildable;
    public ItemList buildCost;
    
    private LimitScript _limit;
    [NonSerialized]
    private bool _init = false;


    public Resources TickTile(TileGridController controller, CubeCoord pos, Upgrades upgrades, TileScript tile)
    {
        // var neighborTiles = controller.GetNeighborTiles(pos);
        // TODO bonus based on tiles!
        var buildingResources = upgrades.Calculate(new Resources());
        var modified = globalUpgrades.Calculate(buildingResources);
        if (Global.Resources.Add(modified))
        {
            var floaty = tile.floaty(controller.worldUi.transform, modified, true, "");
            floaty.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position);
        }
        
        return modified;
    }
  
    public void Init()
    {
        if (!_init && prefab)
        {
            _init = true;
            _limit = prefab.GetComponent<LimitScript>();
        }
    }

    public bool CanBuildDeductCost(TileGridController controller, CubeCoord coord)
    {
        if (!buildable)
        {
            return false;
        }
        Init();
        if (!IsBuildAllowed(controller, coord)) return false;

        var cost = Cost();
        if (Global.Resources.HasResources(cost))
        {
            Global.Resources.AddNoCheck(cost);
            return true;
        }
        return false;
    }

    public Resources Cost()
    {
        return new Resources().Add(buildCost.items);
    }

    public bool IsBuildAllowed(TileGridController controller, CubeCoord coord)
    {
        if (!buildable)
        {
            return false;
        }
        var neighborTiles = controller.GetNeighborTiles(coord);
        if (_limit)
        {
            if (_limit.BuildLimited(neighborTiles))
            {
                return false;
            }
        }

        return true;
    }

    public Upgrades copyBuildingUpgrades()
    {
        Init();
        var upgrades = new Upgrades();
        upgrades.upgrades.AddRange(buildingUpgrades.upgrades);
        upgrades.aquired = buildingUpgrades.aquired;
        return upgrades;
    }

    public bool IsBase()
    {
        return StructureType.BASE.Equals(type);
    }

    public bool IsWasteland()
    {
        return StructureType.WASTELAND.Equals(type);
    }
    
    public bool IsResearch()
    {
        return StructureType.RESEARCH.Equals(type);
    }
    
    public bool IsMarket()
    {
        return StructureType.MARKETPLACE.Equals(type);
    }

    public bool AcquireNextClickUpgrade()
    {
        return clickUpgrades.AcquireNext();
    }

    public bool AcquireNextGlobalUpgrade()
    {
        return globalUpgrades.AcquireNext();
    }

    public bool HasClickUpgrades()
    {
        return clickUpgrades.aquired > 0;
    }

    public Resources CalculateClick()
    {
        return clickUpgrades.Calculate(new Resources());
    }

    public bool CanUpgradeClick()
    {
        return clickUpgrades.upgrades.Count > clickUpgrades.aquired;

    }
}

public enum StructureType
{
    BASE, WOODS, WASTELAND, LUMBERJACK, CHARCOAL_BURNER, RESEARCH, MARKETPLACE, SMITH
}