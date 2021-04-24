using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimitScript : MonoBehaviour
{
    public abstract bool BuildLimited(List<GameObject> neigborTiles);
}
public class LumberJackLimit : LimitScript
{
    public GameObject forestPrefab;


    public override bool BuildLimited(List<GameObject> neigborTiles)
    {
        foreach (var buildCheckNeigborTile in neigborTiles)
        {
            var structure = buildCheckNeigborTile.GetComponent<TileScript>().Structure;
            if (structure != null)
            {
                if (structure.prefab == forestPrefab)
                {
                    return false;
                }
            }
                
           
        }

        return true;
    }
}
