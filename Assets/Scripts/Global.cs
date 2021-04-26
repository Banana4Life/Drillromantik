using UnityEngine;

public static class Global
{
    public static Resources Resources = new Resources();

    private static TechTree _techTree;
    public static TechTree FindTechTree()
    {
        if (_techTree == null)
        {
            _techTree = GameObject.Find("TechTree").GetComponent<TechTree>();
        }

        return _techTree;
    }
}