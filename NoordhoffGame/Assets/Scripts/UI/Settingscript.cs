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

        SettingsScreen.gameObject.SetActive(!SettingsScreen.gameObject.activeSelf);
        BlockingPanel.blocksRaycasts = !BlockingPanel.blocksRaycasts;
        InfoButton.interactable = !InfoButton.IsInteractable();
        
    }
}
