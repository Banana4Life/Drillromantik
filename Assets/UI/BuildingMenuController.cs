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
        
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void TileSelected(TileScript tile)
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
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
                }
                gameObject.SetActive(true);
            }
        }

        public void Build(Button button)
        {
            
        }
    }
}