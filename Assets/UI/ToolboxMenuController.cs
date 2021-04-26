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
        public GameObject resourcePrefab;

        private TileScript _selectedTile;
        
        public void TileSelected(TileScript tile)
        {
            var canDestroy = tile.CanDestroy();
            var canUpgradeTick = tile.CanUpgradeBuilding();
            var canUpgradeClick = tile.CanUpgradeClick();

            if (canDestroy || canUpgradeTick || canUpgradeClick)
            {
                gameObject.SetActive(true);

                destroyButton.gameObject.SetActive(canDestroy);
                buildingUpgradeButton.gameObject.SetActive(canUpgradeTick);
                clickUpgradeButton.gameObject.SetActive(canUpgradeClick);

                _selectedTile = tile;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void ShowCost(Button button)
        {
            if (button == destroyButton)
            {
                _selectedTile.floaty(gameObject.transform, new Resources(), false, "Destroy Building");
            }
            else if (button == buildingUpgradeButton)
            {
                var nextUpgrade = _selectedTile.NextUpgrade();
                _selectedTile.floaty(gameObject.transform, new Resources().Add(nextUpgrade.cost.items), true, nextUpgrade.name); // TODO icons pls
            }
            else if (button == clickUpgradeButton)
            {
                var nextUpgrade = _selectedTile.Structure.clickUpgrades.Next();
                _selectedTile.floaty(gameObject.transform, new Resources().Add(nextUpgrade.cost.items), true, nextUpgrade.name); // TODO icons pls
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
            }
            else if (button == clickUpgradeButton)
            {
                _selectedTile.AquireClickUpgrade(gameObject.transform);
                if (!_selectedTile.CanUpgradeClick())
                {
                    button.gameObject.SetActive(false);
                }
            }
        }
    }
}