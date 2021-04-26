using System;
using System.Collections.Generic;
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
        _structure?.TickTile(_controller, pos, _upgrades, this);
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

    public GameObject floaty(Transform aTransform, Resources resources, bool success, String floatyName)
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
        return floaty;
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
        return _upgrades.HasUpgradeAvailable();
    }
    
    public List<UpgradeChain> AvailableBuildingUpgrades()
    {
        return _upgrades.GetAvailable();
    }
    
    
    public List<UpgradeChain> getClickUpgrades()
    {
        return _structure.clickUpgrades.GetAvailable();
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
    
    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
        if (number < 1) return string.Empty;            
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900); 
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);            
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new ArgumentOutOfRangeException("something bad happened");
    }


}
