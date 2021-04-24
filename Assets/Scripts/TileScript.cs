using System;
using System.Collections;
using System.Collections.Generic;
using TileGrid;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.Serialization;

public class TileScript : MonoBehaviour
{
    private Structure _structure;
    private Upgrades _upgrades = new Upgrades();
    public GameObject currentStructure;

    public TechTree TechTree;
    private TileGridController _controller;
    public CubeCoord pos;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TileTick", 1f, 1f);
        _controller = GetComponentInParent<TileGridController>();
    }
    
    [CustomEditor(typeof(TileScript))]
    public class TileInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("CLICK"))
            {
                Tech.Resources.Add(new Resources(ItemType.STONE.of(1), ItemType.WOOD.of(1)));
                Debug.Log(Tech.Resources);
            }

            var tileScript = (TileScript) target;
            
            foreach (var techTreeStructure in tileScript.TechTree.Structures)
            {
                if (GUILayout.Button(techTreeStructure.name))
                {
                    if (techTreeStructure.CanBuild(tileScript._controller.GetNeighborTiles(tileScript.pos)))
                    {
                        tileScript._structure = techTreeStructure;
                        if (tileScript.currentStructure)
                        {
                            DestroyImmediate(tileScript.currentStructure);
                            tileScript.currentStructure = null;
                        }

                        if (tileScript._structure.prefab)
                        {
                            tileScript.currentStructure = Instantiate(tileScript._structure.prefab, tileScript.gameObject.transform);
                        }
                    }
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

    void TileTick()
    {
        if (_structure != null)
        {
            _structure.TickTile(_upgrades);
        }
    }

    public Structure Structure
    {
        get => _structure;
        set => _structure = value;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
