using UnityEditor;
using UnityEngine;

namespace Editor
{
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
}