using System.Linq;
using TileGrid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class BuildingMenuController : MonoBehaviour
    {
        public TileGridController tileGridController;
        public GameObject buildingButtonPrefab;

        public void TileSelected(TileScript tile)
        {
            var techTree = Global.FindTechTree();
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
                techTree.Structures.Where(s => s.IsBuildAllowed(tileGridController, tile.pos)).ToList();

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

                var eventTrigger = obj.GetComponent<EventTrigger>();
                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener(d =>
                {
                    var cost = structure.Cost();
                    tile.displayCost(button.transform, cost, structure.name);
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

                button.GetComponentInChildren<Text>().text = structure.name;
            }
            gameObject.SetActive(true);
        }
    }
}