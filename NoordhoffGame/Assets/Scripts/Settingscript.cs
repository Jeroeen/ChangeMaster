using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settingscript : MonoBehaviour
{
    public Button infoButton;
    public CanvasGroup Settings;
    bool disable = false;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showMenu()
    {
        if (!disable)
        {
            if (Settings.interactable)
            {   
                Settings.interactable = false;
                Settings.alpha = 0;
            }   
            else
            {   
                Settings.interactable = true;
                Settings.alpha = 1;
            }

            infoButton.interactable = !infoButton.IsInteractable();
        }
    }

    public void switchDisable()
    {
        disable = !disable;       
    }
}
