using Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer((typeof(Upgrades)))]
    public class UpgradesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            EditorGUI.BeginProperty(position, label, property);

            var baseHeight = base.GetPropertyHeight(property, label);
            EditorGUI.PrefixLabel(position, label);

            var yOffset = position.y += baseHeight;
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var prop = property.FindPropertyRelative("upgradeChains");
            var h = HeightUtil.chainlist(prop);
            var rect = new Rect(position.x, yOffset, position.width, baseHeight * h);
            EditorGUI.PropertyField(rect, prop, true);
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var baseHeight = base.GetPropertyHeight(property, label);
            return HeightUtil.upgrades(property) * baseHeight;
        }
    }
}
//
// [CustomPropertyDrawer((typeof(UpgradeChain)))]
// public class UpgradeChainDrawer : PropertyDrawer
// {
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         EditorGUI.BeginProperty(position, label, property);
//         EditorGUI.PrefixLabel(position, label);
//         var baseHeight = base.GetPropertyHeight(property, label);
//
//         var yOffset = position.y;
//         var rect = new Rect(position.x, yOffset, position.width, baseHeight);
//         EditorGUI.PropertyField(rect, property.FindPropertyRelative("name"), GUIContent.none);
//         yOffset += baseHeight;
//         rect = new Rect(position.x, yOffset, position.width, baseHeight);
//         EditorGUI.PropertyField(rect, property.FindPropertyRelative("aquired"), label);
//             
//         var indent = EditorGUI.indentLevel;
//         EditorGUI.indentLevel = 0;
//
//         yOffset += baseHeight;
//         var prop = property.FindPropertyRelative("chain");
//         rect = new Rect(position.x, yOffset, position.width, baseHeight * HeightUtil.upgradelist(prop));
//         EditorGUI.PropertyField(rect, prop, true);
//             
//         // Set indent back to what it was
//         EditorGUI.indentLevel = indent;
//             
//         EditorGUI.EndProperty();
//     }
//
//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         var baseHeight = base.GetPropertyHeight(property, label);
//         return HeightUtil.chain(property) * baseHeight;
//     }
// }

public static class HeightUtil
{
    public static float chainlist(SerializedProperty listProperty)
    {
        if (!listProperty.isExpanded)
        {
            return 1.5f;
        }
        float h = 0; 
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            h += chain(listProperty.GetArrayElementAtIndex(i));
        }

        return 3.5f + h;
    }

    public static float chain(SerializedProperty property)
    {
        if (!property.isExpanded)
        {
            return 1f;
        }
        var chain = property.FindPropertyRelative("chain");
        float uh = upgradelist(chain);
        
        return 4.5f + uh;
    }

    public static float upgradelist(SerializedProperty listProperty)
    {
        if (!listProperty.isExpanded)
        {
            return 1.5f;
        }
        float h = 0; 
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            h += UpgradeDrawer.UpgradeHeight(listProperty.GetArrayElementAtIndex(i));
        }

        return h + 3.5f;
    }

    public static float upgrades(SerializedProperty property)
    {
        float aq = 1;
        float ch = chainlist( property.FindPropertyRelative("upgradeChains"));
        return 2 + aq + ch;
    }
}