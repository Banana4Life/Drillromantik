using System;
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
            var techTree = Global.FindTechTree();
            if (tile.Structure.IsResearch())
            {
                for (var i = 0; i < transform.childCount; ++i)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                
                var upgradableStructures = techTree.Structures.Where(s => s.globalUpgrades.HasUpgradeAvailable()).ToList();

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
                        tile.displayCost(button.transform, new Resources().Add(globalUpgrade.cost.items), globalUpgrade.name);
                    });
                    eventTrigger.triggers.Add(entry);
                    
                    entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerExit;
                    entry.callback.AddListener(d =>
                    {
                        foreach (Transform child in button.transform.GetComponentInChildren<VerticalLayoutGroup>().transform)
                        {
                            Destroy(child.gameObject);
                        }
                    });
                    eventTrigger.triggers.Add(entry);
                    
                    button.GetComponentInChildren<Text>().text = structure.globalUpgrades.Next().name;
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