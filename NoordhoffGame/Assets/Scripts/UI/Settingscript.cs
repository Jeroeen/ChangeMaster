using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settingscript : MonoBehaviour
{
    [SerializeField] private Button interventionButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private CanvasGroup blockingPanel;
    //a function that will enable or disable the menu
    public void ShowMenu()
    {

        settingsScreen.SetActive(!settingsScreen.activeSelf);
        blockingPanel.blocksRaycasts = !blockingPanel.blocksRaycasts;
        settingsButton.interactable = !settingsButton.IsInteractable();
        infoButton.interactable = !infoButton.IsInteractable();
        interventionButton.interactable = !interventionButton.IsInteractable();
        
    }
}
