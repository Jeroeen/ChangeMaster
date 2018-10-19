using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settingscript : MonoBehaviour
{
    public Button InfoButton;
    public CanvasGroup SettingsScreen;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    //a function that will enable or disable the menu
    public void showMenu()
    {
        if (SettingsScreen.interactable)
        {   
            SettingsScreen.interactable = false;
            SettingsScreen.alpha = 0;
            SettingsScreen.blocksRaycasts = false;
        }   
        else
        {   
            SettingsScreen.interactable = true;
            SettingsScreen.alpha = 1;
            SettingsScreen.blocksRaycasts = true;
        }

        InfoButton.interactable = !InfoButton.IsInteractable();
        
    }

     // Update is called once per frame
    void Update()
    {
        
    }
}
