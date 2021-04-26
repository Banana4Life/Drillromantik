using UnityEditor;
using UnityEngine;

namespace Editor
{
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
}