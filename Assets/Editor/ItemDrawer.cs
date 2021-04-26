using UnityEditor;
using UnityEngine;

namespace Editor
{
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
}