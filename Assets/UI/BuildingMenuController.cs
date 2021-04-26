using System;
using System.Linq;
using TileGrid;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingMenuController : MonoBehaviour
    {
        public TileGridController tileGridController;
        public GameObject buildingButtonPrefab;
        
        public void TileSelected(TileScript tile)
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            
            if (!tile.Structure.IsWasteland())
            {
                gameObject.SetActive(false);
                return;
            }
            var buildableStructures =
                Global.TechTree.Structures.Where(s => s.IsBuildAllowed(tileGridController, tile.pos)).ToList();

            if (buildableStructures.Count == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            
            foreach (var structure in buildableStructures)
            {
                var obj = Instantiate(buildingButtonPrefab, transform);
                obj.name = structure.name;
                var rawImage = obj.GetComponent<RawImage>();
                rawImage.texture = structure.texture;
                var button = obj.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    tile.BuildStructure(structure, gameObject.transform);
                    TileSelected(tile);
                });
            }
            gameObject.SetActive(true);
        }
    }
}