using System;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Resources
{
    public Item[] items;

    public Resources(params Item[] items)
    {
        this.items = items;
    }

    public override string ToString()
    {
        return String.Join(" ", items.Select(e => e.ToString()));
    }

    public bool Add(Resources resources)
    {
        foreach (var toAdd in resources.items)
        {
            if (toAdd.quantity < 0)
            {
                bool found = false;
                foreach (var item in items)
                {
                    if (item.type == toAdd.type)
                    {
                        found = true;
                        if (item.quantity + toAdd.quantity < 0)
                        {
                            Debug.Log("Resource not available: " + toAdd.type);
                            return false;
                        }                        
                    }
                }
                if (!found)
                {
                    Debug.Log("Resource not available: " + toAdd.type);
                    return false;
                }
            }
        }


        foreach (var toAdd in resources.items)
        {
            bool added = false;
            foreach (var item in items)
            {
                if (item.type == toAdd.type)
                {
                    item.quantity += toAdd.quantity;
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                items = items.Concat(new[] {toAdd.type.of(toAdd.quantity)}).ToArray();
            }
        }

        return true;
    }
}