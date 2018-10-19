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
    public EventSystem EventSystem;

    private Vector2 position = new Vector2(5.0f, 45.0f);
    private int textCount = 0;
    private ScrollRect interventionScroll;
    private RectTransform scrollviewContent;
    private List<GameObject> panels = new List<GameObject>();
    private InterventionList interventions;
    private RetrieveJson json;
    private float textboxSizeX;

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


        fillScrollView();

    }
    //function to fill the scroll view for the first time
    private void fillScrollView()
    {
        //add as much interventions as are in the array that we retrieved using "LoadJsonInterventions"
        for (int i = 0; i < interventions.interventions.Length; i++)
        {
            //instantiate the text element and add it to the list that holds all the UI elements
            panels.Add(Instantiate(Text, interventionScroll.content.transform));

            initiateTextObject(panels[i], interventions.interventions[i].intervention, position);
            position = new Vector2(position.x + textboxSizeX + 5.0f, position.y);
            //set the name of the button that we'll later be using to see what intervention was selected
            panels[i].name = "button " + i;

            //add an eventListener that triggers when the user clicks the text element and then executes clickAdvice() 
            //with the correct ID for the current Intervention
            EventTrigger trigger = panels[i].GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            int ID = i;
            entry.callback.AddListener((eventData) => { clickAdvice(ID); });
            trigger.triggers.Add(entry);

                textCount++;
            
            //if more than 4(the length of the window is as long as 4 textelements) textelements are added, lengthen the scrollview content containing the interventions
            if (i >= 4)
            {
                scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x + (textboxSizeX + 10.0f), scrollviewContent.sizeDelta.y);
                scrollviewContent.anchoredPosition = new Vector2(scrollviewContent.anchoredPosition.x + 80.0f, scrollviewContent.anchoredPosition.y);
            }
        }

    }

    // function to execute when the player clicks an intervention
    private void clickAdvice(int selected)
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
        textRect.sizeDelta = new Vector2(textRect.sizeDelta.x + 100.0f, textRect.sizeDelta.y + 40.0f);

        //resize the scrollview's content element;
        scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x - ((textboxSizeX + 10.0f) * (textCount - 4)), scrollviewContent.sizeDelta.y);
        scrollviewContent.anchoredPosition = new Vector2(5.0f, 0.0f);
        interventionScroll.horizontal = false;

        //determine the standard position of all assets
        Vector2 newPos = new Vector2(position.x - ((textboxSizeX + 5.0f) * textCount), position.y-10);

        //create the text that tells the player the intervention they have chosen
        GameObject ChosenText = Instantiate(aText, interventionScroll.content.transform);
        panels.Add(ChosenText);
        initiateTextObject(ChosenText, "Je hebt gekozen voor de interventie: \n\n" + interventions.interventions[selected].intervention, newPos);

        //create the text that tells the player the advice concerning the intervention they have chosen
        GameObject AdviceText = Instantiate(aText, interventionScroll.content.transform);
        panels.Add(AdviceText);
        initiateTextObject(AdviceText, "Het volgende advies hoort bij je gekozen interventie: \n" + interventions.interventions[selected].advice, new Vector2(newPos.x + 255.0f, newPos.y));
       
        //create a button with an onclick that will execute showFinished()
        GameObject NextButton = Instantiate(Button, interventionScroll.content.transform);
        RectTransform NextButtonTransform = NextButton.GetComponent<RectTransform>();
        panels.Add(NextButton);
        initiateTextObject(NextButton, "Doorgaan", new Vector2(newPos.x, -(0.5f * scrollviewContent.sizeDelta.y) + NextButtonTransform.sizeDelta.y));
        NextButton.GetComponent<Button>().onClick.AddListener(delegate { showFinished(selected); });
       

    }

    //function to fill the scrollview with the screen for a finished level 
    private void showFinished(int selected)
    {
        //delete all the current elements
        foreach (GameObject g in panels)
        {
            Destroy(g);
        }
        //create a variable for the intervention the player selected
        Intervention selectedIntervention = interventions.interventions[selected];


        //create the standard text element that will be used to instantiate all other text elements in this function
        GameObject aText = Instantiate(Text);
        panels.Add(aText);
        RectTransform textRect = aText.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(textRect.sizeDelta.x *2, textRect.sizeDelta.y + 40);

        //determine the standard position of all assets

        Vector2 newPos = new Vector2(position.x - ((textboxSizeX + 5.0f) * textCount), position.y - 10);

        //create the text that will tell the player what changed with their skills
        GameObject ChosenText = Instantiate(aText, interventionScroll.content.transform);
        panels.Add(ChosenText);
        RectTransform cTextPos = ChosenText.GetComponent<RectTransform>();
        cTextPos.anchoredPosition = newPos;
        Text chosenText = ChosenText.GetComponentInChildren<Text>();
        chosenText.text = "je hebt level 1 gehaald daarbij heb je de volgende skills gehaald \n"
            + "Analytisch  " + selectedIntervention.Analytisch + "\n"
            + "Enthousiasmerend " + selectedIntervention.Enthousiasmerend + "\n"
            + "Besluitvaardig " + selectedIntervention.Besluitvaardig + "\n"
            + "Empathisch " + selectedIntervention.Empathisch + "\n"
            + "Overtuigend " + selectedIntervention.Overtuigend + "\n"
            + "Creatief " + selectedIntervention.Creatief + "\n"
            + "Kennis van veranderkunde " + selectedIntervention.Kennis_veranderkunde;


    }


    //a function that will enable or disable the menu 
    public void showMenu()
    {
        if (Interventionscreen.interactable)
        {
            Interventionscreen.interactable = false;
            Interventionscreen.alpha = 0;
            Interventionscreen.blocksRaycasts = false;
        }
        else
        {
            Interventionscreen.interactable = true;
            Interventionscreen.alpha = 1;
            Interventionscreen.blocksRaycasts = true;
        }

        InfoButton.interactable = !InfoButton.IsInteractable();
        SettingsButton.interactable = !SettingsButton.IsInteractable();

    }

    //a function to easily set the text and position of a textobject
    private void initiateTextObject(GameObject initiate, string text, Vector2 position )
    {
        //set the text of the textObject
        Text objectText = initiate.GetComponentInChildren<Text>();
        objectText.text = text;
        //set the positionof the textObject
        RectTransform cTextPos = initiate.GetComponent<RectTransform>();
        cTextPos.anchoredPosition = position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
