using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class UpgradeScript : MonoBehaviour
{
    public Upgrade2[] upgrades;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
public class Upgrade2
{
    public String name;
    public float chance = 1;
    public UpgradeType type;
    public Resources resources;

    public Resources2 apply(Resources2 resources)
    {
        if (chance == 1 || UnityEngine.Random.value < chance)
        {
            switch (type)
            {
                case UpgradeType.ADD:
                    resources.AddNoCheck(this.resources);
                    break;
                case UpgradeType.MULTIPLY:
                    foreach (var item in this.resources.items)
                    {
                        resources.Mul(item.type, item.quantity);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return resources;
    }
    
}

public enum UpgradeType
{
    ADD, MULTIPLY
}