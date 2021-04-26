using System;
using System.Collections.Generic;
using System.Linq;
using TileGrid;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Structure
{
    public String name;
    public StructureType type;
    public GameObject prefab;
    public Texture texture;
    public float spawnWeight = 0;
    
    public Upgrades buildingUpgrades;
    public Upgrades clickUpgrades;
    public Upgrades globalUpgrades;

    public bool buildable;
    public ItemList buildCost;
    
    private LimitScript _limit;
    [NonSerialized]
    private bool _init = false;


    public void TickTile(TileGridController controller, CubeCoord pos, Upgrades upgrades)
    {
        // var neighborTiles = controller.GetNeighborTiles(pos);
        // TODO bonus based on tiles!
        Global.Resources.Add(upgrades.Calculate());
    }
  
    public void Init()
    {
        if (!_init && prefab)
        {
            _init = true;
            _limit = prefab.GetComponent<LimitScript>();
        }
    }

    public bool CanBuildDeductCost(TileGridController controller, CubeCoord coord)
    {
        if (!buildable)
        {
            return false;
        }
        Init();
        if (!IsBuildAllowed(controller, coord)) return false;

        var cost = Cost();
        if (Global.Resources.HasResources(cost))
        {
            Global.Resources.AddNoCheck(cost);
            return true;
        }
        return false;
    }

    public Resources Cost()
    {
        return new Resources().Add(buildCost.items);
    }

    public bool IsBuildAllowed(TileGridController controller, CubeCoord coord)
    {
        var neighborTiles = controller.GetNeighborTiles(coord);
        if (_limit)
        {
            if (_limit.BuildLimited(neighborTiles))
            {
                return false;
            }
        }

        return true;
    }

    public Upgrades copyBuildingUpgrades()
    {
        Init();
        var upgrades = new Upgrades();
        upgrades.upgrades.AddRange(buildingUpgrades.upgrades);
        upgrades.aquired = buildingUpgrades.aquired;
        return upgrades;
    }

    public bool IsBase()
    {
        return StructureType.BASE.Equals(type);
    }

    public bool IsWasteland()
    {
        return StructureType.WASTELAND.Equals(type);
    }
    
    public bool IsResearch()
    {
        return StructureType.RESEARCH.Equals(type);
    }
    
    public bool IsMarket()
    {
        return StructureType.MARKETPLACE.Equals(type);
    }

    public bool AcquireNextClickUpgrade()
    {
        return clickUpgrades.AcquireNext();
    }

    public bool AcquireNextGlobalUpgrade()
    {
        return globalUpgrades.AcquireNext();
    }

    public bool HasClickUpgrades()
    {
        return clickUpgrades.aquired > 0;
    }

    public Resources CalculateClick()
    {
        return clickUpgrades.Calculate();
    }

    public bool CanUpgradeClick()
    {
        return clickUpgrades.upgrades.Count > clickUpgrades.aquired;

    }
}

public enum StructureType
{
    BASE, WOODS, WASTELAND, LUMBERJACK, CHARCOAL_BURNER, RESEARCH, MARKETPLACE, SMITH
}



[CustomPropertyDrawer((typeof(Structure)))]
public class StructureDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        var baseHeight = base.GetPropertyHeight(property, label);
        var offsetY = position.y;
        var rect = new Rect(position.x, offsetY, position.width, baseHeight);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("name"), GUIContent.none);
        
        offsetY += baseHeight;
        rect = new Rect(position.x, offsetY, position.width, baseHeight);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("type"),  GUIContent.none);
        
        offsetY += baseHeight;
        rect = new Rect(position.x, offsetY, position.width, baseHeight);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("prefab"), new GUIContent("prefab"));
        
        offsetY += baseHeight;
        rect = new Rect(position.x, offsetY, position.width, baseHeight);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("texture"), new GUIContent("texture"));
        
        offsetY += baseHeight;
        rect = new Rect(position.x, offsetY, position.width, baseHeight);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("spawnWeight"), new GUIContent("spawnWeight"));
        
        offsetY += baseHeight;
        rect = new Rect(position.x, offsetY, position.width, baseHeight);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("buildable"), new GUIContent("buildable"));
        
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        offsetY += baseHeight;
        var upgradeProp = property.FindPropertyRelative("buildingUpgrades");
        var height = UpgradesDrawer.UpgradesHeight(upgradeProp) * baseHeight;
        rect = new Rect(position.x, offsetY, position.width, height);
        EditorGUI.PropertyField(rect, upgradeProp, true);
        
        offsetY += height;
        upgradeProp = property.FindPropertyRelative("clickUpgrades");
        height = UpgradesDrawer.UpgradesHeight(upgradeProp) * baseHeight;
        rect = new Rect(position.x, offsetY, position.width, height);
        EditorGUI.PropertyField(rect, upgradeProp, true);
        
        offsetY += height;
        upgradeProp = property.FindPropertyRelative("globalUpgrades");
        height = UpgradesDrawer.UpgradesHeight(upgradeProp) * baseHeight;
        rect = new Rect(position.x, offsetY, position.width, height);
        EditorGUI.PropertyField(rect, upgradeProp, true);
        
        offsetY += height;
        upgradeProp = property.FindPropertyRelative("buildCost");
        height = (upgradeProp.FindPropertyRelative("items").arraySize + 2) * baseHeight;
        rect = new Rect(position.x, offsetY, position.width, height);
        EditorGUI.PropertyField(rect, upgradeProp, true);
            
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
            
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var baseHeight = base.GetPropertyHeight(property, label);
        var bU = property.FindPropertyRelative("buildingUpgrades");
        var cU = property.FindPropertyRelative("clickUpgrades");
        var gU = property.FindPropertyRelative("globalUpgrades");
        var bC = property.FindPropertyRelative("buildCost").FindPropertyRelative("items");
        var uH = UpgradesDrawer.UpgradesHeight(bU) + UpgradesDrawer.UpgradesHeight(cU) + UpgradesDrawer.UpgradesHeight(gU);
        return (5.5f + uH+5 + bC.arraySize) * baseHeight;
    }

}