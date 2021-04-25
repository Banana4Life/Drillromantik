using System;
using TileGrid;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileScript : MonoBehaviour
{
    private Structure _structure;
    public Structure Structure => _structure;
    private Upgrades _upgrades = new Upgrades();
    public GameObject currentStructure;

    public TechTree TechTree;
    private TileGridController _controller;
    public CubeCoord pos;
    private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");
    private Renderer _renderer;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(TileTick), 1f, 1f);
        _controller = GetComponentInParent<TileGridController>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void TileTick()
    {
        _structure?.TickTile(_controller, pos, _upgrades);
    }

    public void AquireBuildingUpgrade()
    {
        _upgrades.AcquireNext();
    }
    
    public void AquireClickUpgrade()
    {
        _structure.AcquireNextClickUpgrade();
    }
    
    public void AquireGlobalUpgrade()
    {
        _structure.AcquireNextGlobalUpgrade();
    }

    public void SelectTile()
    {
        SetTileMaterialPropertyBlock(coloredMaterialPropertyBlock("#D8FFB1"));
    }

    private static MaterialPropertyBlock coloredMaterialPropertyBlock(String hexColor)
    {
        var matPropBlock = new MaterialPropertyBlock();
        ColorUtility.TryParseHtmlString(hexColor, out Color color);
        matPropBlock.SetColor(ColorPropertyId, color);
        return matPropBlock;
    }

    private void SetTileMaterialPropertyBlock(MaterialPropertyBlock matPropBlock)
    {
        _renderer.SetPropertyBlock(matPropBlock, 2);
        _renderer.SetPropertyBlock(matPropBlock, 3);
        _renderer.SetPropertyBlock(matPropBlock, 4);
        _renderer.SetPropertyBlock(matPropBlock, 5);
        _renderer.SetPropertyBlock(matPropBlock, 6);
        _renderer.SetPropertyBlock(matPropBlock, 7);
    }

    public void UnSelectTile()
    {
        SetTileMaterialPropertyBlock(null);
    }
    
    public void HoverTile()
    {
        SetTileMaterialPropertyBlock(coloredMaterialPropertyBlock("#FFD37C"));
    }

    public bool HasClickReward()
    {
        return _structure.HasClickUpgrades();
    }
    
    public Resources ClickTile()
    {
        var calculated = _structure.CalculateClick();
        Global.Resources.Add(calculated);
        return calculated;
    }

    private void Update()
    {
        
    }
    
    [CustomEditor(typeof(TileScript))]
    public class TileInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            // if (GUILayout.Button("CLICK"))
            // {
            //     Global.Resources.Add(ItemType.STONE.of(1), ItemType.WOOD.of(1));
            //     Debug.Log(Global.Resources);
            // }
            //
            // var tileScript = (TileScript) target;
            //
            // GUILayout.BeginHorizontal();
            // GUILayout.Label("Click Upgrades:");
            // if (tileScript._upgrades.ClickUpgrades.Count > tileScript._upgrades.aquiredClickUpgrades)
            // {
            //     if (GUILayout.Button("Upgrade"))
            //     {
            //         tileScript._upgrades.aquiredClickUpgrades++;
            //     }    
            // } 
            // GUILayout.EndHorizontal();
            // for (var i = 0; i < tileScript._upgrades.ClickUpgrades.Count; i++)
            // {
            //     var upgrade = tileScript._upgrades.ClickUpgrades[i];
            //     GUILayout.BeginHorizontal();
            //     GUILayout.Label(upgrade.name + ": " + new Resources().Add(upgrade.resources.items));
            //     GUILayout.Toggle(i < tileScript._upgrades.aquiredClickUpgrades, "");
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

    public void BuildStructure(Structure structure)
    {
        if (!_controller)
        {
            Debug.Log("Game is not running!");
            return;
        }
        if (structure.CanBuildDeductCost(_controller, pos))
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
            currentStructure.transform.GetChild(0).Rotate(Vector3.up, Random.Range(0,360));
        }

        _upgrades = _structure.copyBuildingUpgrades();
    }

    public void Init(CubeCoord coord)
    {
        pos = coord;
        transform.Translate(0,50,0);
    }

    public bool CanUpgradeClick()
    {
        return _structure.CanUpgradeClick();
    }

    public bool CanUpgradeTick()
    {
        return _upgrades.upgrades.Count > _upgrades.aquired;
    }

    public bool CanDestroy()
    {
        return !_structure.IsBase() && (CanUpgradeClick() || CanUpgradeTick());
    }

  
}
