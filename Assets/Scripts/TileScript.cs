using System;
using System.Collections;
using System.Collections.Generic;using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Building Structure;
    public Upgrades Upgrades = new Upgrades();
    public GameObject Building;

    public GameObject[] buildingPrefabs;
    
    // Start is called before the first frame update
    void Start()
    {
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
            GameObject buildingPrefab = null;
            if (GUILayout.Button("WOOD_CUTTER"))
            {
                buildingPrefab = tileScript.buildingPrefabs[2];
                tileScript.Structure = Tech.WOOD_CUTTER;
            }
            if (GUILayout.Button("CHARCOAL_BURNER"))
            {
                tileScript.Structure = Tech.CHARCOAL_BURNER;
                buildingPrefab = tileScript.buildingPrefabs[3];
            }
            if (GUILayout.Button("SMITH"))
            {
                tileScript.Structure = Tech.SMITH;
                buildingPrefab = tileScript.buildingPrefabs[4];
            }
            if (GUILayout.Button("MARKET"))
            {
                tileScript.Structure = Tech.MARKET;
                buildingPrefab = tileScript.buildingPrefabs[5];
            }
            if (GUILayout.Button("LOOKOUT_TOWER"))
            {
                tileScript.Structure = Tech.LOOKOUT_TOWER;
                // buildingPrefab = tileScript.buildingPrefabs[6];
            }
            if (GUILayout.Button("RESEARCH_FACILITY"))
            {
                tileScript.Structure = Tech.RESEARCH_FACILITY;
                // buildingPrefab = tileScript.buildingPrefabs[7];
            }
           
            if (buildingPrefab)
            {
                if (tileScript.Building)
                {
                    Destroy(tileScript.Building);
                    tileScript.Building = null;
                }
                tileScript.Building = Instantiate(buildingPrefab, tileScript.gameObject.transform);
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
