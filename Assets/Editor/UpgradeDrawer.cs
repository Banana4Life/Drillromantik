using UnityEditor;
using UnityEngine;

namespace Editor
{
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
            return UpgradeHeight(property) * base.GetPropertyHeight(property, label);
        }

        public static float UpgradeHeight(SerializedProperty prop)
        {
            var resItems = prop.FindPropertyRelative("resources");
            var cosItems = prop.FindPropertyRelative("cost");
         
            var res = height(resItems);
            var cos = height(cosItems);

            return (1 + res + cos);
        }

        private static float height(SerializedProperty itemList)
        {
            var itemArray = itemList.FindPropertyRelative("items");
            return itemList.isExpanded ? (itemArray.isExpanded ? itemArray.arraySize + 5 : 3) : 1;
        }
    }
}