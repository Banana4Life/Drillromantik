using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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
                        var next = structure.globalUpgrades.Next();
                        var res = new Resources().Add(next.cost.items);
                        tile.floaty(transform, res, structure.globalUpgrades.AcquireNext(), next.name);
                        TileSelected(tile);
                    });
                    
                    var eventTrigger = obj.GetComponent<EventTrigger>();
                    var entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerEnter;
                    entry.callback.AddListener(d =>
                    {
                        var globalUpgrade = structure.globalUpgrades.Next();
                        tile.floaty(gameObject.transform, new Resources().Add(globalUpgrade.cost.items), true, globalUpgrade.name); // TODO icons pls
                    });
                    eventTrigger.triggers.Add(entry);
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