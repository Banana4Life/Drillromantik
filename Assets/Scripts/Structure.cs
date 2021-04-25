using System;
using System.Collections.Generic;
using System.Linq;
using TileGrid;
using UnityEngine;

[Serializable]
public class Structure
{
    public String name;
    public GameObject prefab;
    private LimitScript _limit;
    private BuildScript _build;
    private UpgradeScript _upgrade;
    [NonSerialized]
    private bool _init = false;

    public float spawnWeight = 0;

    public void TickTile(Upgrades upgrades)
    {
        Global.Resources.Add(upgrades.CalculateTick());
    }
    
    public void ClickTile(Upgrades upgrades)
    {
        Global.Resources.Add(upgrades.CalculateClick());
    }

    public void Init()
    {
        if (!_init && prefab)
        {
            _init = true;
            _build = prefab.GetComponent<BuildScript>();
            _limit = prefab.GetComponent<LimitScript>();
            _upgrade = prefab.GetComponent<UpgradeScript>();
        }
    }

    public bool CanBuild(TileGridController controller, CubeCoord coord)
    {
        var neighborTiles = controller.GetNeighborTiles(coord);
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

    public Upgrades freshUpgrades()
    {
        Init();
        var upgrades = new Upgrades();
        if (_upgrade)
        {
            upgrades.ClickUpgrades.AddRange(_upgrade.Upgrades.ClickUpgrades);
            upgrades.TickUpgrades.AddRange(_upgrade.Upgrades.TickUpgrades);
            upgrades.aquiredClickUpgrades = _upgrade.Upgrades.aquiredClickUpgrades;
            upgrades.aquiredTickUpgrades = _upgrade.Upgrades.aquiredTickUpgrades;
        }
        return upgrades;
    }
}
