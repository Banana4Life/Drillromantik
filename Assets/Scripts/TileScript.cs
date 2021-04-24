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

    public TechTree TechTree;
    
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
            
            foreach (var techTreeStructure in tileScript.TechTree.Structures)
            {
                if (GUILayout.Button(techTreeStructure.name))
                {
                    tileScript._structure = techTreeStructure;
                    if (tileScript.currentStructure)
                    {
                        DestroyImmediate(tileScript.currentStructure);
                        tileScript.currentStructure = null;
                    }
                    tileScript.currentStructure = Instantiate(tileScript._structure.prefab, tileScript.gameObject.transform);
                }
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
