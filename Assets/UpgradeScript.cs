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
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.EndProperty();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("name"), GUIContent.none);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("type"), GUIContent.none);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("chance"), GUIContent.none);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("resources"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("cost"));
        }
 
}


[CustomPropertyDrawer((typeof(Item)))]
public class ItemDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.EndProperty();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(property.FindPropertyRelative("type"), GUIContent.none);
        GUILayout.Label("x");
        EditorGUILayout.PropertyField(property.FindPropertyRelative("quantity"), GUIContent.none);
        EditorGUILayout.EndHorizontal();
    }
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