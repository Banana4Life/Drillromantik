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
            
            bool hasClickReward = tile.HasClickReward();
            if (hasClickReward)
            {
                RawImage[] rawImages = clickButton.GetComponentsInChildren<RawImage>();
                foreach (RawImage rawImage in rawImages)
                {
                    if (rawImage.gameObject.transform.parent != transform.parent)
                    {
                        rawImage.texture = tile.Structure.texture;
                    }
                }
            }
            clickButton.gameObject.SetActive(hasClickReward);
        }
    }
    
    
}