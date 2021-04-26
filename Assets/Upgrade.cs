using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Random = System.Random;
using System;

[Serializable]
public class Upgrade
{
    public String name;
    public float chance = 1;
    public UpgradeType type;

    public ItemList resources;
    public ItemList cost;

    public Resources Cost()
    {
        return new Resources().Add(cost.items);
    }

    public Resources Apply(Resources resources)
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
    ADD,
    MULTIPLY
}