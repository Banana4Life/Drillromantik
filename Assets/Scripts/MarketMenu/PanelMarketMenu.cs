using UnityEngine;

public class PanelMarketMenu : MonoBehaviour{

void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMarketMenu();
        }
    }
    
    public void CloseMarketMenu()
    {
        gameObject.SetActive(false);
    }
}
