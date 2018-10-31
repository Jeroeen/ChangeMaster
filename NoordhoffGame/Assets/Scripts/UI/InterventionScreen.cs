using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InterventionScreen : MonoBehaviour
{
    public Button InfoButton;
    public Button SettingsButton;
    public CanvasGroup Interventionscreen;
    public GameObject Text;
    public GameObject Button;
    public CanvasGroup BlockingPanel;
    public PlayerScript Player;

    private Vector2 position = new Vector2(0.0f, 0.0f);
    private int textCount = 0;
    private ScrollRect interventionScroll;
    private RectTransform scrollviewContent;
    private List<GameObject> panels = new List<GameObject>();
    private InterventionList interventions;
    private RetrieveJson json;
    private float textboxSizeX;
    private float elementLimit;

    // Start is called before the first frame update
    void Start()
    {
        //retrieve the list of interventions for this lvl(level 1) from the associated Json file
        json = new RetrieveJson();
        interventions = json.LoadJsonInterventions(1);
        //interventionScroll is the ScrollRect that contains the list of interventions to choose from
        interventionScroll = Interventionscreen.GetComponentInChildren<ScrollRect>();
        //set the content element of the scrollview to the position 0,0 since, for some reason, sometimes it moves away from that position
        scrollviewContent = interventionScroll.content.GetComponent<RectTransform>();
        scrollviewContent.anchoredPosition = new Vector2(0.0f, 0.0f);
        //get the size of the text box for use further in the file
        RectTransform textRect = Text.GetComponent<RectTransform>();
        textboxSizeX = textRect.sizeDelta.x;
        position = new Vector2(textboxSizeX / 8, -textboxSizeX / 8);
        FillScrollView();

    }
    //function to fill the scroll view for the first time
    private void FillScrollView()
    {
        //add as much interventions as are in the array that we retrieved using "LoadJsonInterventions"
        for (int i = 0; i < interventions.Interventions.Length; i++)
        {
            //instantiate the text element and add it to the list that holds all the UI elements
            panels.Add(Instantiate(Text, interventionScroll.content.transform));

            InitiateTextObject(panels[i], interventions.Interventions[i].InterventionText, position);
            position = new Vector2(position.x + textboxSizeX + 5.0f, position.y);
            //set the name of the button that we'll later be using to see what intervention was selected
            panels[i].name = "button " + i;

            //add an eventListener that triggers when the user clicks the text element and then executes clickAdvice() 
            //with the correct ID for the current Intervention
            EventTrigger trigger = panels[i].GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            int ID = i;
            entry.callback.AddListener((eventData) => {ClickAdvice(ID); });
            trigger.triggers.Add(entry);
            textCount++;
            elementLimit = scrollviewContent.sizeDelta.x /textboxSizeX;
            //if more than 4(the length of the window is as long as 4 textelements) textelements are added, lengthen the scrollview content containing the interventions
            if (i >= elementLimit - 1) 
            {
                scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x + (textboxSizeX), scrollviewContent.sizeDelta.y);
                scrollviewContent.anchoredPosition = new Vector2(scrollviewContent.anchoredPosition.x + (textboxSizeX /2) , scrollviewContent.anchoredPosition.y);
            }
        }
    }

    // function to execute when the player clicks an intervention
    private void ClickAdvice(int selected)
    {
       //delete all the current elements
        foreach(GameObject g in panels)
        {
            Destroy(g);
        }
        //create the standard text element that will be used to instantiate all other text elements in this function
        GameObject aText = Instantiate(Text);
        panels.Add(aText);
        RectTransform textRect = aText.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(textRect.sizeDelta.x *2, textRect.sizeDelta.y+(textRect.sizeDelta.y / 4));       
        //resize the scrollview's content element;
        scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x - ((textboxSizeX + 10.0f) * (textCount - 4)), scrollviewContent.sizeDelta.y);
        scrollviewContent.anchoredPosition = new Vector2(5.0f, 0.0f);
        interventionScroll.horizontal = false;
        //determine the standard position of all assets
        Vector2 newPos = new Vector2(position.x - ((textboxSizeX + 5.0f) * textCount), position.y-10);
        textboxSizeX = textboxSizeX * 2;
        //create the text that tells the player the intervention they have chosen
        GameObject ChosenText = Instantiate(aText, interventionScroll.content.transform);
        panels.Add(ChosenText);
        InitiateTextObject(ChosenText, "Je hebt gekozen voor de interventie: \n\n" + interventions.Interventions[selected].InterventionText, newPos);
        //create the text that tells the player the advice concerning the intervention they have chosen
        GameObject AdviceText = Instantiate(aText, interventionScroll.content.transform);
        panels.Add(AdviceText);
        InitiateTextObject(AdviceText, "Het volgende advies hoort bij je gekozen interventie: \n" + interventions.Interventions[selected].Advice, new Vector2(newPos.x + textboxSizeX, newPos.y));       
        //create a button with an onclick that will execute showFinished()
        GameObject NextButton = Instantiate(Button, interventionScroll.content.transform);
        RectTransform NextButtonTransform = NextButton.GetComponent<RectTransform>();
        panels.Add(NextButton);
        InitiateTextObject(NextButton, "Doorgaan", new Vector2(newPos.x, -(0.5f * scrollviewContent.sizeDelta.y) + NextButtonTransform.sizeDelta.y));
        NextButton.GetComponent<Button>().onClick.AddListener(delegate { ShowFinished(selected); });
       

    }

    //function to fill the scrollview with the screen for a finished level 
    private void ShowFinished(int selected)
    {

        //delete all the current elements
        foreach (GameObject g in panels)
        {
            Destroy(g);
        }
        //create a variable for the intervention the player selected
        Intervention selectedIntervention = interventions.Interventions[selected];

        PlayerScript.Analytisch += selectedIntervention.Analytisch;
        PlayerScript.Enthousiasmerend += selectedIntervention.Enthousiasmerend;
        PlayerScript.Besluitvaardig += selectedIntervention.Besluitvaardig;
        PlayerScript.Empathisch += selectedIntervention.Empathisch;
        PlayerScript.Overtuigend += selectedIntervention.Overtuigend;
        PlayerScript.Creatief += selectedIntervention.Creatief;
        PlayerScript.Veranderkunde_Kennis += selectedIntervention.Kennis_veranderkunde;


        //create the standard text element that will be used to instantiate all other text elements in this function
        GameObject aText = Instantiate(Text);
        panels.Add(aText);
        RectTransform textRect = aText.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(textRect.sizeDelta.x *2, textRect.sizeDelta.y *2);
        //determine the standard position of all assets
        Vector2 newPos = new Vector2(position.x - ((textboxSizeX /2) * textCount), position.y);
        //create the text that will tell the player what changed with their skills
        GameObject ChosenText = Instantiate(aText, interventionScroll.content.transform);
        panels.Add(ChosenText);
        RectTransform cTextPos = ChosenText.GetComponent<RectTransform>();
        cTextPos.anchoredPosition = new Vector2(newPos.x /2, newPos.y);
        Text chosenText = ChosenText.GetComponentInChildren<Text>();
        chosenText.text = "gefeliciteerd " + Player.GetPlayerTitle() + " \n"
            + "je hebt level 1 gehaald daarbij heb je de volgende skills gehaald \n"
            + "Analytisch  " + selectedIntervention.Analytisch + "\n"
            + "Enthousiasmerend " + selectedIntervention.Enthousiasmerend + "\n"
            + "Besluitvaardig " + selectedIntervention.Besluitvaardig + "\n"
            + "Empathisch " + selectedIntervention.Empathisch + "\n"
            + "Overtuigend " + selectedIntervention.Overtuigend + "\n"
            + "Creatief " + selectedIntervention.Creatief + "\n"
            + "Kennis van veranderkunde " + selectedIntervention.Kennis_veranderkunde;



        //determine the standard position of all assets
        newPos = new Vector2(newPos.x + textboxSizeX, newPos.y);
        //create the text that will tell the player what changed with their skills
        GameObject pChosenText = Instantiate(aText, interventionScroll.content.transform);
        panels.Add(pChosenText);
        RectTransform pTextPos = pChosenText.GetComponent<RectTransform>();
        pTextPos.anchoredPosition = new Vector2(newPos.x, newPos.y);
        Text playerText = pChosenText.GetComponentInChildren<Text>();
        playerText.text = "je hebt nu de volgende skills \n"
            + "Analytisch  " + PlayerScript.Analytisch + "\n"
            + "Enthousiasmerend " + PlayerScript.Enthousiasmerend + "\n"
            + "Besluitvaardig " + PlayerScript.Besluitvaardig + "\n"
            + "Empathisch " + PlayerScript.Empathisch + "\n"
            + "Overtuigend " + PlayerScript.Overtuigend + "\n"
            + "Creatief " + PlayerScript.Creatief + "\n"
            + "Kennis van veranderkunde " + PlayerScript.Veranderkunde_Kennis;
    }
    //a function that will enable or disable the menu 
    public void ShowMenu()
    {
        if (Interventionscreen.interactable)
        {
            BlockingPanel.blocksRaycasts = false;
            Interventionscreen.interactable = false;
            Interventionscreen.alpha = 0;
            Interventionscreen.blocksRaycasts = false;
        }
        else
        {
            BlockingPanel.blocksRaycasts = true;
            Interventionscreen.interactable = true;
            Interventionscreen.alpha = 1;
            Interventionscreen.blocksRaycasts = true;
        }
        InfoButton.interactable = !InfoButton.IsInteractable();
        SettingsButton.interactable = !SettingsButton.IsInteractable();
    }

    //a function to easily set the text and position of a textobject
    private void InitiateTextObject(GameObject initiate, string text, Vector2 position )
    {
        //set the text of the textObject
        Text objectText = initiate.GetComponentInChildren<Text>();
        objectText.text = text;
        //set the positionof the textObject
        RectTransform cTextPos = initiate.GetComponent<RectTransform>();
        cTextPos.anchoredPosition = position;
    }
}
