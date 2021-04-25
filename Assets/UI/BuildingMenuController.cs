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

        private TileScript _selectedTile;
        
        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void removeAll()
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public void TileSelected(TileScript tile)
        {
            removeAll();
            var buildableStructures =
                tile.TechTree.Structures.Where(s => s.texture && s.CanBuild(tileGridController, tile.pos)).ToList();

            if (buildableStructures.Count == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                foreach (var structure in buildableStructures)
                {
                    var obj = Instantiate(buildingButtonPrefab, transform);
                    obj.name = structure.name;
                    var rawImage = obj.GetComponent<RawImage>();
                    rawImage.texture = structure.texture;
                    var button = obj.GetComponent<Button>();
                    button.onClick.AddListener(() =>
                    {
                        removeAll();
                        tile.BuildStructure(structure);
                    });
                }
                gameObject.SetActive(true);
            }
        }

        public void Build(Button button)
        {
            
        }
    }
}