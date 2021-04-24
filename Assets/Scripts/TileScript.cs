using System;
using System.Collections;
using System.Collections.Generic;using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.Serialization;

public class TileScript : MonoBehaviour
{
    private Structure _structure;
    private Upgrades _upgrades = new Upgrades();
    public GameObject currentStructure;

    public GameObject[] structurePrefabs;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TileUpdate", 1f, 1f);
    }
    
    [CustomEditor(typeof(TileScript))]
    public class TileInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("CLICK"))
            {
                Tech.Resources.Add(new Resources{Stone = 1});
                Debug.Log(Tech.Resources);
            }

            var tileScript = (TileScript) target;
            GameObject structurePrefab = null;
            if (GUILayout.Button("BASE"))
            {
                tileScript._structure = null;
                structurePrefab = tileScript.structurePrefabs[0];
            }
            if (GUILayout.Button("FOREST"))
            {
                tileScript._structure = null;
                structurePrefab = tileScript.structurePrefabs[1];
            }
            if (GUILayout.Button("WOOD_CUTTER"))
            {
                structurePrefab = tileScript.structurePrefabs[2];
                tileScript._structure = Tech.WOOD_CUTTER;
            }
            if (GUILayout.Button("CHARCOAL_BURNER"))
            {
                tileScript._structure = Tech.CHARCOAL_BURNER;
                structurePrefab = tileScript.structurePrefabs[3];
            }
            if (GUILayout.Button("SMITH"))
            {
                tileScript._structure = Tech.SMITH;
                structurePrefab = tileScript.structurePrefabs[4];
            }
            if (GUILayout.Button("MARKET"))
            {
                tileScript._structure = Tech.MARKET;
                structurePrefab = tileScript.structurePrefabs[5];
            }
            if (GUILayout.Button("LOOKOUT_TOWER"))
            {
                tileScript._structure = Tech.LOOKOUT_TOWER;
                // buildingPrefab = tileScript.buildingPrefabs[6];
            }
            if (GUILayout.Button("RESEARCH_FACILITY"))
            {
                tileScript._structure = Tech.RESEARCH_FACILITY;
                // buildingPrefab = tileScript.buildingPrefabs[7];
            }
           
            if (structurePrefab)
            {
                if (tileScript.currentStructure)
                {
                    DestroyImmediate(tileScript.currentStructure);
                    tileScript.currentStructure = null;
                }
                tileScript.currentStructure = Instantiate(structurePrefab, tileScript.gameObject.transform);
            }
            
            if (GUILayout.Button("CLEAR"))
            {
                tileScript._structure = null;
                if (tileScript.currentStructure)
                {
                    DestroyImmediate(tileScript.currentStructure);
                    tileScript.currentStructure = null;
                }
            }
        }
    }

    void TileUpdate()
    {
        if (_structure != null)
        {
            var resources = _structure.ExploitResources(_upgrades);
            Tech.Resources.Add(resources);
            Debug.Log(Tech.Resources);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
