using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class UpgradeScript : MonoBehaviour
{
    public Upgrades Upgrades;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}

[CustomPropertyDrawer((typeof(Upgrade)))]
public class UpgradeDrawer : PropertyDrawer
{
     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            var baseHeight = base.GetPropertyHeight(property, label);

            var chanceRect = new Rect(position.x, position.y, 30, baseHeight);
            var unitRect = new Rect(position.x + 35, position.y, 50, baseHeight);
            var nameRect = new Rect(position.x + 90, position.y, position.width - 90, baseHeight);
            EditorGUI.PropertyField(chanceRect, property.FindPropertyRelative("chance"), GUIContent.none);
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("type"), GUIContent.none);
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var yOffset = position.y + baseHeight;
            var resItems = property.FindPropertyRelative("resources");
            var rect = new Rect(position.x, yOffset, position.width, baseHeight * height(resItems));
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("resources"), true);

            yOffset += baseHeight * height(resItems);
            var cosItems = property.FindPropertyRelative("cost");
            rect = new Rect(position.x, yOffset, position.width, baseHeight * height(cosItems));
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("cost"), true);
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }

     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
     {
         var resItems = property.FindPropertyRelative("resources");
         var cosItems = property.FindPropertyRelative("cost");
         
         var res = height(resItems);
         var cos = height(cosItems);
         
         return (1 + res +cos) * base.GetPropertyHeight(property, label);
     }

     private static int height(SerializedProperty itemList)
     {
         var itemArray = itemList.FindPropertyRelative("items");
         return itemList.isExpanded ? (itemArray.isExpanded ? itemArray.arraySize + 5 : 3) : 1;
     }
}

[CustomPropertyDrawer(typeof(Item))]
public class ItemDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        var baseHeight = base.GetPropertyHeight(property, label);
        
        var typeRect = new Rect(position.x, position.y, 90, baseHeight);
        var quantityRect = new Rect(position.x + 90, position.y, position.width - 90, baseHeight);
        EditorGUI.PropertyField(typeRect,  property.FindPropertyRelative("type"), GUIContent.none);
        EditorGUI.PropertyField(quantityRect, property.FindPropertyRelative("quantity"), GUIContent.none);    
        EditorGUI.EndProperty();

    }
    
    // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    // {
    //     return property.arraySize * base.GetPropertyHeight(property, label);
    // }
}

[Serializable]
public class Upgrade
{
    public String name;
    public float chance = 1;
    public UpgradeType type;

    public ItemList resources;
    public ItemList cost;

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