using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ToolboxMenuController : MonoBehaviour
    {
        public Button destroyButton;
        
        public GameObject upgradeButtonPrefab;

        private TileScript _selectedTile;
        private Dictionary<Button, UpgradeChain> _upgradeButtons = new Dictionary<Button, UpgradeChain>();
        
        public void TileSelected(TileScript tile)
        {
            var canDestroy = tile.CanDestroy();
            var buildingUpgrades = tile.AvailableBuildingUpgrades();
            var clickUpgrades = tile.getClickUpgrades();

            foreach (var oldGameObject in _upgradeButtons.Keys)
            {
                Destroy(oldGameObject.gameObject);
            }
            _upgradeButtons.Clear();

            if (canDestroy || buildingUpgrades.Count > 0 || clickUpgrades.Count > 0)
            {
                gameObject.SetActive(true);
                destroyButton.gameObject.SetActive(canDestroy);

                foreach (var buildingUpgrade in buildingUpgrades)
                {
                    var newButton = Instantiate(upgradeButtonPrefab, transform);
                    newButton.GetComponentInChildren<Text>().text = buildingUpgrade.Next().name;
                    var button = newButton.GetComponent<Button>();
                    AttachTriggers(button);
                    _upgradeButtons[button] = buildingUpgrade;
                }

                foreach (var clickUpgrade in clickUpgrades)
                {
                    var newButton = Instantiate(upgradeButtonPrefab, transform);
                    newButton.GetComponentInChildren<Text>().text = clickUpgrade.Next().name;
                    var button = newButton.GetComponent<Button>();
                    AttachTriggers(button);
                    _upgradeButtons[button] = clickUpgrade;
                }

                _selectedTile = tile;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void ApplyAction(Button button)
        {
            if (button == destroyButton)
            {
                var wasteland = Global.FindTechTree().Structures.First(s => s.IsWasteland());
                _selectedTile.AssignStructure(wasteland);
                TileSelected(_selectedTile);
            }
        }
        
        public void AttachTriggers(Button button)
        {
            button.onClick.AddListener(() =>
            {
                var upgradeChain = _upgradeButtons[button];
                var next = upgradeChain.Next();
                if (upgradeChain.AquireNext())
                {
                    _selectedTile.floaty(transform, next.Cost(), true, next.name);
                }
                TileSelected(_selectedTile);
            });
            
            var eventTrigger = button.gameObject.GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
            entry.callback.AddListener(d =>
            {
                var upgrade = _upgradeButtons[button].Next();
                _selectedTile.displayCost(button.transform, upgrade.Cost(), upgrade.name);
            });
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
            entry.callback.AddListener(d =>
            {
                foreach (Transform child in button.transform.GetComponentInChildren<VerticalLayoutGroup>().transform)
                {
                    Destroy(child.gameObject);
                }
            });
            eventTrigger.triggers.Add(entry);
        }
    }
    
 
    
}