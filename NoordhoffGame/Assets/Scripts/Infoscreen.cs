using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Infoscreen : MonoBehaviour
{
    public Button SettingsButton;
    public CanvasGroup InfoScreen;
    public GameObject Panel;
    public CanvasGroup blockingPanel;

    private List<Sprite> images = new List<Sprite>();
    private Vector2 position = new Vector2(0.0f, -5.0f);
    private ScrollRect infoScrollview;
    private RetrieveJson json;


    // Start is called before the first frame update
    void Start()
    {
        //retrieve the list of interventions for this lvl(level 1) from the associated Json file
        json = new RetrieveJson();
        InfoList information = json.LoadJsonInformation(1);
        RetrieveAsset.RetrieveAssets();
        for (int i = 0; i < information.InformationList.Length; i++)
        {
            images.Add(RetrieveAsset.GetSpriteByName(information.InformationList[i].Image));
        }
        //get the height of the panel, this will be used later
        RectTransform panelRect = Panel.GetComponent<RectTransform>();
        float panelSizeY = panelRect.sizeDelta.y;
        //get the Scrollview component from the Canvasgroup containing it
        infoScrollview = InfoScreen.GetComponentInChildren<ScrollRect>();
        //a list that contains all the UI elements that i'll be making
        List<GameObject> Panels = new List<GameObject>();
        //set the content element of the scrollview to the position 0,0 since, for some reason, sometimes it moves away from that position
        RectTransform scrollviewContent = infoScrollview.content.GetComponent<RectTransform>();
        scrollviewContent.anchoredPosition = new Vector2(0.0f, 0.0f);
        //get the RectTransform of the scrollview content
        RectTransform infoScrollviewRect = infoScrollview.content.GetComponent<RectTransform>();
        for (int i = 0; i < images.Count; i++)
        {
            //add the created UI element to the Panels list
            Panels.Add(Instantiate(Panel, infoScrollview.content.transform));
            //create a panel, containing a text element, an image of the person who's talking and set the position of that panel.
            //create the text element
            Text[] infoText = Panels[i].GetComponentsInChildren<Text>();
            infoText[0].text = information.InformationList[i].Text;
            //create the image
            Image[] infoImage = Panels[i].GetComponentsInChildren<Image>();
            infoImage[1].sprite = images[i];
            //set the position
            RectTransform[] infoRectTransform = Panels[i].GetComponents<RectTransform>();
            infoRectTransform[0].anchoredPosition = position;
            float elementLimit = scrollviewContent.sizeDelta.y / panelSizeY;
            //if there are more than 3 elements, make the content element from the scrollview bigger and set it to the correct position 
            if (i >= elementLimit - 1) 
            {
                infoScrollviewRect.sizeDelta = new Vector2(infoScrollviewRect.sizeDelta.x, infoScrollviewRect.sizeDelta.y +(panelSizeY + 10));
                infoScrollviewRect.anchoredPosition = new Vector2(infoScrollviewRect.anchoredPosition.x, infoScrollviewRect.anchoredPosition.y -((0.5f* panelSizeY) + 10));
            }
            // set the position for the next element
            position = new Vector2(position.x, position.y - (panelSizeY + 10.0f));
        }
        
    }

    //a function that will enable or disable the menu 
    public void enableInfo()
    {
        if(InfoScreen.interactable)
        {
            blockingPanel.blocksRaycasts = false;
            InfoScreen.interactable = false;
            InfoScreen.alpha = 0;
            InfoScreen.blocksRaycasts = false;
        }
        else
        {
            blockingPanel.blocksRaycasts = true;
            InfoScreen.interactable = true;
            InfoScreen.alpha = 1;
            InfoScreen.blocksRaycasts = true;
        }
        SettingsButton.interactable = !SettingsButton.IsInteractable();       
    }
}
