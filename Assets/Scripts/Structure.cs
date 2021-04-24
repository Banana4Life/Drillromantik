using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Structure
{
    public String name;
    public GameObject prefab;
    private ExploitationScript _exploitation;
    private LimitScript _limit;
    private BuildScript _build;
    private bool _init2 = false;

    public float spawnWeight = 0;

    public void TickTile(Upgrades upgrades)
    {
        Init();
        if (_exploitation && _exploitation.tick)
        {
            Tech.Resources.Add(_exploitation.Calculate(upgrades));
            Debug.Log(Tech.Resources);
        }
    }

    public void Init()
    {
        if (!_init2 && prefab)
        {
            _init2 = true;
            _exploitation = prefab.GetComponent<ExploitationScript>();
            _build = prefab.GetComponent<BuildScript>();
            _limit = prefab.GetComponent<LimitScript>();
        }
    }

    public bool CanBuild(List<GameObject> neighborTiles)
    {
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
}