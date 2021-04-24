using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class UpgradeScript : MonoBehaviour
{
    public Upgrade[] tickUpgrades;
    public Upgrade[] clickUpgrades;
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
public class Upgrade
{
    public String name;
    public bool grant;
    public float chance = 1;
    public UpgradeType type;
    public ItemList resources;

    public Resources apply(Resources resources)
    {
        if (chance == 1 || UnityEngine.Random.value < chance)
        {
            switch (type)
            {
                case UpgradeType.ADD:
                    resources.Add(this.resources.items);
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