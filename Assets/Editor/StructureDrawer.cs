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
            
            offsetY += baseHeight;
            rect = new Rect(position.x, offsetY, position.width, baseHeight);
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("limit"), new GUIContent("limit"));
            
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
        
            offsetY += baseHeight;
            var prop = property.FindPropertyRelative("buildingUpgrades");
            var height = HeightUtil.upgrades(prop) * baseHeight;
            rect = new Rect(position.x, offsetY, position.width, height);
            EditorGUI.PropertyField(rect, prop, true);
        
            offsetY += height;
            prop = property.FindPropertyRelative("clickUpgrades");
            height = HeightUtil.upgrades(prop) * baseHeight;
            rect = new Rect(position.x, offsetY, position.width, height);
            EditorGUI.PropertyField(rect, prop, true);
        
            offsetY += height;
            prop = property.FindPropertyRelative("globalUpgrades");
            height = HeightUtil.upgrades(prop) * baseHeight;
            rect = new Rect(position.x, offsetY, position.width, height);
            EditorGUI.PropertyField(rect, prop, true);
        
            offsetY += height;
            prop = property.FindPropertyRelative("buildCost");
            height = HeightUtil.itemList(prop) * baseHeight;
            rect = new Rect(position.x, offsetY, position.width, height);
            EditorGUI.PropertyField(rect, prop, true);
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var baseHeight = base.GetPropertyHeight(property, label);
            var h = 6; // Name/Type/Prefab/Texture/spawnWeight/buildable
            var bU = property.FindPropertyRelative("buildingUpgrades");
            var cU = property.FindPropertyRelative("clickUpgrades");
            var gU = property.FindPropertyRelative("globalUpgrades");
            var bC = property.FindPropertyRelative("buildCost");
            var uH = HeightUtil.upgrades(bU) + HeightUtil.upgrades(cU) + 
                         HeightUtil.upgrades(gU) + HeightUtil.itemList(bC);
            return (h + uH) *  baseHeight;
        }

    }
}