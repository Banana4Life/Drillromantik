using System;
using System.Collections;
using System.Collections.Generic;using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Building Structure;
    public Upgrades Upgrades = new Upgrades();
    
    
    
    // Start is called before the first frame update
    void Start()
    {
    }
    
    [CustomEditor(typeof(TileScript))]
    public class TileInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("CLICK"))
            {
                Tech.Resources.Add(new Resources{Stone = 1});
                Debug.Log(Tech.Resources);
            }
            if (GUILayout.Button("WOOD_CUTTER"))
            {
                TileScript tile = (TileScript) target;
                tile.Structure = Tech.WOOD_CUTTER;
            }
            if (GUILayout.Button("CHARCOAL_BURNER"))
            {
                TileScript tile = (TileScript) target;
                tile.Structure = Tech.CHARCOAL_BURNER;
            }
            if (GUILayout.Button("SMITH"))
            {
                TileScript tile = (TileScript) target;
                tile.Structure = Tech.SMITH;
            }
            if (GUILayout.Button("MARKET"))
            {
                TileScript tile = (TileScript) target;
                tile.Structure = Tech.MARKET;
            }
            if (GUILayout.Button("LOOKOUT_TOWER"))
            {
                TileScript tile = (TileScript) target;
                tile.Structure = Tech.LOOKOUT_TOWER;
            }
            if (GUILayout.Button("RESEARCH_FACILITY"))
            {
                TileScript tile = (TileScript) target;
                tile.Structure = Tech.RESEARCH_FACILITY;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Structure != null)
        {
            var resources = Structure.ExploitResources(Upgrades);
            Tech.Resources.Add(resources);
            Debug.Log(Tech.Resources);

            // TODO add to global state    
        }
    }
}
