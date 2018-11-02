using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settingscript : MonoBehaviour
{
    [SerializeField] private Button InfoButton;
    [SerializeField] private GameObject SettingsScreen;
    [SerializeField] private CanvasGroup BlockingPanel;
    //a function that will enable or disable the menu
    public void ShowMenu()
    {

        SettingsScreen.SetActive(!SettingsScreen.activeSelf);
        BlockingPanel.blocksRaycasts = !BlockingPanel.blocksRaycasts;
        InfoButton.interactable = !InfoButton.IsInteractable();
        
    }
}
