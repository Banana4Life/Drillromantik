using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResearchMenuController : MonoBehaviour
    {
        public GameObject buildingButtonPrefab;
        
        public void TileSelected(TileScript tile)
        {
            if (tile.Structure.IsResearch())
            {
                for (var i = 0; i < transform.childCount; ++i)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                
                var upgradableStructures =
                    Global.TechTree.Structures.Where(s => s.globalUpgrades.upgrades.Count > s.globalUpgrades.aquired).ToList();

                if (upgradableStructures.Count == 0)
                {
                    gameObject.SetActive(false);
                    return;
                }
                
                foreach (var structure in upgradableStructures)
                {
                    var obj = Instantiate(buildingButtonPrefab, transform);
                    obj.name = structure.name;
                    var rawImage = obj.GetComponent<RawImage>();
                    rawImage.texture = structure.texture;
                    var button = obj.GetComponent<Button>();
                    button.onClick.AddListener(() =>
                    {
                        structure.globalUpgrades.AcquireNext();
                        TileSelected(tile);
                    });
                }
                
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}