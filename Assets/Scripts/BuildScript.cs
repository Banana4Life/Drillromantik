using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class BuildScript : MonoBehaviour
{
    public Resources costs;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool canBuild(List<GameObject> neighborTiles)
    {
        return Tech.Resources.Add(costs);
    }
}
