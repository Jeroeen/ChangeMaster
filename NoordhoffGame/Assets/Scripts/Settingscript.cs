using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settingscript : MonoBehaviour
{
    public Button InfoButton;
    public CanvasGroup SettingsScreen;
    public CanvasGroup BlockingPanel;
    //a function that will enable or disable the menu
    public void ShowMenu()
    {
        if (SettingsScreen.interactable)
        {
            BlockingPanel.blocksRaycasts = false;
            SettingsScreen.interactable = false;
            SettingsScreen.alpha = 0;
            SettingsScreen.blocksRaycasts = false;
        }   
        else
        {
            BlockingPanel.blocksRaycasts = true;
            SettingsScreen.interactable = true;
            SettingsScreen.alpha = 1;
            SettingsScreen.blocksRaycasts = true;
        }

        InfoButton.interactable = !InfoButton.IsInteractable();
        
    }
}
