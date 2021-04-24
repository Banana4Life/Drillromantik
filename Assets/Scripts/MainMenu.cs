using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Hide menu
        GameObject.Find("Menu").SetActive(false);
        // Show hud
        //TODO: switch to game
    }

    public void Quit()
    {
        Application.Quit();
    }
}
