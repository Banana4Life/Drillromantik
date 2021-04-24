using TileGrid;
using UnityEditor;
using UnityEngine;

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
    
    public void ClickTile()
    {
        if (_structure != null)
        {
            _structure.ClickTile(_upgrades);
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
                Tech.Resources.Add(new Resources(ItemType.STONE.of(1), ItemType.WOOD.of(1)));
                Debug.Log(Tech.Resources);
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
    }

    public void Init(CubeCoord coord)
    {
        pos = coord;
    }
}
