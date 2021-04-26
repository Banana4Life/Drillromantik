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
        var buildingResources = upgrades.Calculate(new Resources());
        var modified = globalUpgrades.Calculate(buildingResources);
        
        if (tile.Structure.type == StructureType.LUMBERJACK)
        {
            var neighborTiles = controller.GetNeighborTiles(pos);
            modified.Mul(ItemType.WOOD, neighborTiles.Count(t => t.GetComponent<TileScript>().Structure.type == StructureType.WOODS));
        }
        
        if (tile.Structure.type == StructureType.CHARCOAL_BURNER)
        {
            var neighborTiles = controller.GetNeighborTiles(pos);
            var cnt = neighborTiles.Count(t => t.GetComponent<TileScript>().Structure.type == StructureType.CHARCOAL_BURNER);
            modified.Mul(ItemType.CHARCOAL, cnt);
        }
        
        if (modified.Items.Count > 0)
        {
            if (Global.Resources.Add(modified))
            {
            
                var floaty = tile.floaty(controller.worldUi.transform, modified, true, "");
                floaty.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position);
            }    
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
        return buildingUpgrades.Clone();
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
        return clickUpgrades.HasAquiredAny();
    }

    public Resources CalculateClick()
    {
        return clickUpgrades.Calculate(new Resources());
    }

    public bool CanUpgradeClick()
    {
        return clickUpgrades.HasUpgradeAvailable();
    }
}

public enum StructureType
{
    BASE, WOODS, WASTELAND, LUMBERJACK, CHARCOAL_BURNER, RESEARCH, MARKETPLACE, SMITH
}