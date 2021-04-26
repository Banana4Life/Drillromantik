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

        private TileScript _selectedTile;

        private void Start()
        {
            gameObject.SetActive(false);
            buildingUpgradeButton.gameObject.SetActive(false);
            clickUpgradeButton.gameObject.SetActive(false);
            destroyButton.gameObject.SetActive(false);
        }

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

        public void ApplyAction(Button button)
        {
            if (button == destroyButton)
            {
                var wasteland = _selectedTile.TechTree.Structures.First(s => s.IsWasteland());
                _selectedTile.BuildStructure(wasteland);
                TileSelected(_selectedTile);
            }
            else if (button == buildingUpgradeButton)
            {
                _selectedTile.AquireBuildingUpgrade();
                if (!_selectedTile.CanUpgradeBuilding())
                {
                    button.gameObject.SetActive(false);
                }
            }
            else if (button == clickUpgradeButton)
            {
                _selectedTile.AquireClickUpgrade();
                if (!_selectedTile.CanUpgradeClick())
                {
                    button.gameObject.SetActive(false);
                }
            }
        }
    }
}