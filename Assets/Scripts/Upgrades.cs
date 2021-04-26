using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Upgrades
{
    [FormerlySerializedAs("aquiredTickUpgrades")] public int aquired;
    [FormerlySerializedAs("TickUpgrades")] public List<Upgrade> upgrades = new List<Upgrade>();

    public Upgrades AddUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        return this;
    }

    public Resources Calculate()
    {
        var toAdd = new Resources();
        for (var i = 0; i < aquired; i++)
        {
            var upgrade = upgrades[i];
            toAdd = upgrade.Apply(toAdd);
        }

        return toAdd;
    }


    public Upgrade Next()
    {
        return upgrades[aquired];
    }
    
    public bool AcquireNext()
    {
        if (upgrades.Count > aquired)
        {
            var upgrade = upgrades[aquired];
            if (Global.Resources.HasResources(upgrade.cost))
            {
                Global.Resources.Add(upgrade.cost.items);
                aquired++;
                return true;
            }
        }

        return false;
    }

}

[CustomPropertyDrawer((typeof(Upgrades)))]
public class UpgradesDrawer : PropertyDrawer
{
     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            var baseHeight = base.GetPropertyHeight(property, label);

            var chanceRect = new Rect(position.x, position.y, position.width, baseHeight);
            EditorGUI.PropertyField(chanceRect, property.FindPropertyRelative("aquired"), label);
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var yOffset = position.y + baseHeight;
            var upgradeList = property.FindPropertyRelative("upgrades");
            var rect = new Rect(position.x, yOffset, position.width, baseHeight * 5);
            EditorGUI.PropertyField(rect, upgradeList, true);
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }

     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
     {
         var baseHeight = base.GetPropertyHeight(property, label);
         return UpgradesHeight(property) * baseHeight;
     }

     public static float UpgradesHeight(SerializedProperty property)
     {
         var upgradeList = property.FindPropertyRelative("upgrades");
         if (!upgradeList.isExpanded)
         {
             return 2;
         }
         return 5 + height(upgradeList);
     }

     private static float height(SerializedProperty upgradeList)
     {
         // if (!upgradeList.isExpanded)
         // {
         //     return 1;
         // }
         float h = 0; 
         for (int i = 0; i < upgradeList.arraySize; i++)
         {
             var upgrade = upgradeList.GetArrayElementAtIndex(i);
             h += UpgradeDrawer.UpgradeHeight(upgrade);
         }

         return h;
     }
}