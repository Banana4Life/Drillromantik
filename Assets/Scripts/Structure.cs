using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Structure
{
    public String name;
    public GameObject prefab;
    private ExploitationScript _exploitationScript;
    private LimitScript _limit;
    private BuildScript _build;
    [NonSerialized]
    private bool _init = false;

    public float spawnWeight = 0;

    public void TickTile(Upgrades upgrades)
    {
        Init();
        if (_exploitationScript && _exploitationScript.tick)
        {
            Tech.Resources.Add(_exploitationScript.CalculateTick(upgrades));
            Debug.Log(Tech.Resources);
        }
    }
    
    public void ClickTile(Upgrades upgrades)
    {
        Init();
        if (_exploitationScript && _exploitationScript.click)
        {
            Tech.Resources.Add(_exploitationScript.CalculateClick(upgrades));
            Debug.Log(Tech.Resources);
        }
    }

    public void Init()
    {
        if (!_init && prefab)
        {
            _init = true;
            _exploitationScript = prefab.GetComponent<ExploitationScript>();
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