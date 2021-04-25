using System;
using TileGrid;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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

    void TileTick()
    {
        if (_structure != null)
        {
            _structure.TickTile(_upgrades);
        }
    }

    public void SelectTile()
    {
        GetComponentInChildren<Renderer>().SetPropertyBlock(coloredMaterialPropertyBlock("#D8FFB1"));
    }

    private static MaterialPropertyBlock coloredMaterialPropertyBlock(String hexColor)
    {
        var matPropBlock = new MaterialPropertyBlock();
        Color color;
        ColorUtility.TryParseHtmlString(hexColor, out color);
        matPropBlock.SetColor("_Color", color);
        return matPropBlock;
    }

    public void UnSelectTile()
    {
        GetComponentInChildren<Renderer>().SetPropertyBlock(null);
    }
    
    public void HoverTile()
    {
        GetComponentInChildren<Renderer>().SetPropertyBlock(coloredMaterialPropertyBlock("#FFD37C"));
    }
    
    public void ClickTile()
    {
        if (_structure != null)
        {
            _structure.ClickTile(_upgrades);
        }
    }

    private void Update()
    {
        // TODO other script and hide shadows while falling
        var position = transform.position;
        if (position.y > 0)
        {
            transform.Translate(0, 30 * -Time.deltaTime, 0);
        }
        else
        {
            position = new Vector3(position.x, 0, position.z);
            transform.position = position;
        }
    }

    public Structure Structure
    {
        get => _structure;
    }

    
    [CustomEditor(typeof(TileScript))]
    public class TileInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            
            if (GUILayout.Button("CLICK"))
            {
                Global.Resources.Add(ItemType.STONE.of(1), ItemType.WOOD.of(1));
                Debug.Log(Global.Resources);
            }

            var tileScript = (TileScript) target;
            
            foreach (var techTreeStructure in tileScript.TechTree.Structures)
            {
                if (GUILayout.Button(techTreeStructure.name))
                {
                    tileScript.BuildStructure(techTreeStructure);
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

    private void BuildStructure(Structure structure)
    {
        if (!_controller)
        {
            Debug.Log("Game is not running!");
            return;
        }
        if (structure.CanBuild(_controller, pos))
        {
            AssignStructure(structure);
        }
    }

    public void AssignStructure(Structure structure)
    {
        _structure = structure;
        if (currentStructure)
        {
            DestroyImmediate(currentStructure);
            currentStructure = null;
        }

        if (_structure.prefab)
        {
            currentStructure = Instantiate(_structure.prefab, transform, false);
            currentStructure.name = $"{structure.name}";
        }

        _upgrades = _structure.freshUpgrades();
    }

    public void Init(CubeCoord coord)
    {
        pos = coord;
        transform.Translate(0,50,0);
    }
}
