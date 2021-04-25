using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class BuildScript : MonoBehaviour
{
    public ItemList costs;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool CanBuildDeductCost()
    {
        var cost = new Resources().Add(costs.items);
        if (Global.Resources.HasResources(cost))
        {
            Global.Resources.AddNoCheck(cost);
            return true;
        }
        return false;
    }
}
