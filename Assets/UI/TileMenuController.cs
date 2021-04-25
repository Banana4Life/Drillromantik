using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TileMenuController : MonoBehaviour
    {
        public BuildingMenuController buildingMenu;
        public ToolboxMenuController toolboxMenu;
        public Button clickButton;

        private void Start()
        {
            clickButton.gameObject.SetActive(false);
        }

        public void TileSelected(TileScript tile)
        {
            buildingMenu.TileSelected(tile);
            toolboxMenu.TileSelected(tile);
            clickButton.gameObject.SetActive(tile.HasClickReward());
        }
    }
}