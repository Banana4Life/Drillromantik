using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolboxMenuController : MonoBehaviour
    {
        public Button destroyButton;
        public Button buildingUpgradeButton;
        public Button clickUpgradeButton;
        public Text clickUpgradeText;
        public Text buildingUpgradeText;

        private TileScript _selectedTile;
        
        public void TileSelected(TileScript tile)
        {
            var canDestroy = tile.CanDestroy();
            var canUpgradeBuilding = tile.CanUpgradeBuilding();
            var canUpgradeClick = tile.CanUpgradeClick();

            if (canDestroy || canUpgradeBuilding || canUpgradeClick)
            {
                gameObject.SetActive(true);

                destroyButton.gameObject.SetActive(canDestroy);
                if (canUpgradeBuilding)
                {
                    buildingUpgradeText.text = tile.NextUpgrade().name;
                } 
                buildingUpgradeButton.gameObject.SetActive(canUpgradeBuilding);
                if (canUpgradeClick)
                {
                    clickUpgradeText.text = tile.Structure.clickUpgrades.Next().name;
                }
                clickUpgradeButton.gameObject.SetActive(canUpgradeClick);

                _selectedTile = tile;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void HideCost(Button button)
        {
            foreach (Transform child in button.transform.GetComponentInChildren<VerticalLayoutGroup>().transform)
            {
                Destroy(child.gameObject);
            }
        }
        public void ShowCost(Button button)
        {
            if (button == destroyButton)
            {
                _selectedTile.displayCost(button.transform, new Resources(),  "Destroy Building");
            }
            else if (button == buildingUpgradeButton)
            {
                var nextUpgrade = _selectedTile.NextUpgrade();
                _selectedTile.displayCost(button.transform, new Resources().Add(nextUpgrade.cost.items), nextUpgrade.name);
            }
            else if (button == clickUpgradeButton)
            {
                var nextUpgrade = _selectedTile.Structure.clickUpgrades.Next();
                _selectedTile.displayCost(button.transform, new Resources().Add(nextUpgrade.cost.items), nextUpgrade.name);
            }
        }

        public void ApplyAction(Button button)
        {
            if (button == destroyButton)
            {
                var wasteland = Global.TechTree.Structures.First(s => s.IsWasteland());
                _selectedTile.AssignStructure(wasteland);
                TileSelected(_selectedTile);
            }
            else if (button == buildingUpgradeButton)
            {
                _selectedTile.AquireBuildingUpgrade(gameObject.transform);
                if (!_selectedTile.CanUpgradeBuilding())
                {
                    button.gameObject.SetActive(false);
                }
                else
                {
                    buildingUpgradeText.text = _selectedTile.NextUpgrade().name;
                }
            }
            else if (button == clickUpgradeButton)
            {
                _selectedTile.AquireClickUpgrade(gameObject.transform);
                if (!_selectedTile.CanUpgradeClick())
                {
                    button.gameObject.SetActive(false);
                }
                else
                {
                    clickUpgradeText.text = _selectedTile.Structure.clickUpgrades.Next().name;
                }
            }
        }
    }
}