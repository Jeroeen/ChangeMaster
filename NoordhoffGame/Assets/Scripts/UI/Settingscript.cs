using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settingscript : MonoBehaviour
{
    [SerializeField] private Button infoButton;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private CanvasGroup blockingPanel;
    //a function that will enable or disable the menu
    public void ShowMenu()
    {
        Debug.Log("Pipo");
        settingsScreen.SetActive(!settingsScreen.activeSelf);
        blockingPanel.blocksRaycasts = !blockingPanel.blocksRaycasts;
        infoButton.interactable = !infoButton.IsInteractable();
        
    }
}
