using System;
using TileGrid;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class TileScript : MonoBehaviour
{
    private Structure _structure;
    public Structure Structure => _structure;
    private Upgrades _upgrades = new Upgrades();
    public GameObject currentStructure;

    private TileGridController _controller;
    public CubeCoord pos;
    private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");
    private Renderer _renderer;

    public GameObject floatyTextPrefab;
    public GameObject resourcePrefab;

    public Upgrades Upgrades => _upgrades;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(BuildingTick), 1f, 1f);
        _controller = GetComponentInParent<TileGridController>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void BuildingTick()
    {
        _structure?.TickTile(_controller, pos, _upgrades);
    }

    public void AquireBuildingUpgrade(Transform transform)
    {
        var next = _upgrades.Next();
        var res = new Resources().Add(next.cost.items);
        floaty(transform, res, _upgrades.AcquireNext(), next.name);
    }
    
    public void AquireClickUpgrade(Transform transform)
    {
        var next = _structure.clickUpgrades.Next();
        var res = new Resources().Add(next.cost.items);
        floaty(transform, res, _structure.AcquireNextClickUpgrade(), next.name);
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
    
    public void ClickTile(Transform tr)
    {
        var calculated = _structure.CalculateClick();

        floaty(tr, calculated, Global.Resources.Add(calculated), "");
    }

    private void Update()
    {
        
    }

    public void BuildStructure(Structure structure, Transform aTransform)
    {
        if (!_controller)
        {
            Debug.Log("Game is not running!");
            return;
        }
        var cost = structure.Cost();
        if (structure.CanBuildDeductCost(_controller, pos))
        {
            AssignStructure(structure);
            _controller.SpawnNewAroundAsync(pos);
            floaty(aTransform, cost, true, structure.name);
        }
        else
        {
            floaty(aTransform, cost, false, structure.name);
        }
        
    }

    public void floaty(Transform aTransform, Resources resources, bool success, String floatyName)
    {
        // TODO icon + text Icon aus TechTree.Textures
        var floaty = Instantiate(floatyTextPrefab, aTransform.parent);
        floaty.GetComponent<Text>().text = floatyName + "\n" + resources;
        if (!success)
        {
            floaty.GetComponent<Text>().color = Color.red;
        }
        floaty.transform.position = Input.mousePosition;
        floaty.GetComponent<PlusScript>().ttl = 0.4f;
    }

    public void displayCost(Transform aTransform, Resources resources, String name)
    {
        foreach (var item in resources.Items)
        {
            var rect = aTransform.GetComponentInChildren<VerticalLayoutGroup>().transform;
            var resObj = Instantiate(resourcePrefab, rect);
            
            var status = resObj.GetComponent<ResourceStatus>();
            status.Init(item.Value);
            foreach (var techTreeTexture in Global.TechTree.Textures)
            {
                if (techTreeTexture.type == item.Key)
                {
                    status.GetComponentInChildren<RawImage>().texture = techTreeTexture.tex;
                    break;
                }

            }
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

    public bool CanUpgradeBuilding()
    {
        return _upgrades.upgrades.Count > _upgrades.aquired;
    }

    public Upgrade NextUpgrade()
    {
        return _upgrades.Next();
    }

    public bool CanDestroy()
    {
        return !_structure.IsBase() && (CanUpgradeClick() || CanUpgradeBuilding());
    }

    public bool CanUpgradeGlobal()
    {
        return _structure.IsResearch();
    }
}
