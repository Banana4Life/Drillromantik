using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TileScript))]
    public class TileInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            var tileScript = (TileScript) target;
            
            // for (var i = 0; i < tileScript.Upgrades.upgrades.Count; i++)
            // {
            //     var upgrade = tileScript.Upgrades.upgrades[i];
            //     GUILayout.BeginHorizontal();
            //     GUILayout.Label(upgrade.name + ": " + new Resources().Add(upgrade.resources.items));
            //     GUILayout.Toggle(i < tileScript.Upgrades.aquired, "");
            //     GUILayout.EndHorizontal();
            // }
            // GUILayout.BeginHorizontal();
            // GUILayout.Label("Tick Upgrades:");
            // if (tileScript._upgrades.ClickUpgrades.Count > tileScript._upgrades.aquired)
            // {
            //     if (GUILayout.Button("Upgrade"))
            //     {
            //         tileScript._upgrades.aquired++;
            //     }    
            // } 
            // GUILayout.EndHorizontal();
            // for (var i = 0; i < tileScript._upgrades.upgrades.Count; i++)
            // {
            //     var upgrade = tileScript._upgrades.upgrades[i];
            //     GUILayout.BeginHorizontal();
            //     GUILayout.Label(upgrade.name + ": " + new Resources().Add(upgrade.resources.items));
            //     GUILayout.Toggle(i < tileScript._upgrades.aquired, "");
            //     GUILayout.EndHorizontal();
            // }
            //
            // foreach (var techTreeStructure in tileScript.TechTree.Structures)
            // {
            //     if (GUILayout.Button(techTreeStructure.name))
            //     {
            //         tileScript.BuildStructure(techTreeStructure);
            //     }
            // }
            //
            // if (GUILayout.Button("CLEAR"))
            // {
            //     tileScript._structure = null;
            //     if (tileScript.currentStructure)
            //     {
            //         DestroyImmediate(tileScript.currentStructure);
            //         tileScript.currentStructure = null;
            //     }
            // }
        }
    }
}
