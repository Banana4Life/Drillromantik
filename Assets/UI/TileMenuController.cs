using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TileMenuController : MonoBehaviour
    {
        public BuildingMenuController buildingMenu;
        public ToolboxMenuController toolboxMenu;
        public ResearchMenuController researchMenu;
        public Button clickButton;

        private void Start()
        {
            buildingMenu.gameObject.SetActive(false);
            toolboxMenu.gameObject.SetActive(false);
            researchMenu.gameObject.SetActive(false);
            clickButton.gameObject.SetActive(false);
        }

        public void TileSelected(TileScript tile)
        {
            buildingMenu.TileSelected(tile);
            toolboxMenu.TileSelected(tile);
            researchMenu.TileSelected(tile);
            clickButton.gameObject.SetActive(tile.HasClickReward());
        }
    }
    
    
}