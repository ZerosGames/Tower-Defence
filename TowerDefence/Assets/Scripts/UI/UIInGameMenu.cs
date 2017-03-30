using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGameMenu : MonoBehaviour {

    private bool UIOnScreen = false;

    public void ResumeGame()
    {
        ShowUI(false);
        GameManager.gameManager.SetPause(false);
    }

    public void OptionsMenu()
    {
        ShowUI(false);
        //Activate Options Menu
    }

    public void MainMenu()
    {
        //Travel to main menu
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowUI(bool _ShowUI)
    {
        UIOnScreen = _ShowUI;
        gameObject.SetActive(_ShowUI);
    }

    public bool GetUIOnScreen()
    {
        return UIOnScreen;
    }
}
