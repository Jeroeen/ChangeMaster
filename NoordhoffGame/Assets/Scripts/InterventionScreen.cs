using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InterventionScreen : MonoBehaviour
{
    public CanvasGroup InterventionMenu;
    public GameObject Text;
    public GameObject Button;
    public EventSystem eventSystem;

    private Vector2 pos = new Vector2(5.0f, 45.0f);
    private int textCount = 0;
    private ScrollRect intvScroll;
    private RectTransform contentScroll;
    private List<GameObject> Panels = new List<GameObject>();
    private string[] text;
    private string[] advies;
    private InterventionList interventions;

    private RetrieveJson json;

    // Start is called before the first frame update
    void Start()
    {
        //retrieve the list of interventions for this lvl(level 1) from the associated Json file
        json = new RetrieveJson();
        interventions = json.LoadJsonInterventions(1);

        //intvScroll is the ScrollRect that contains the list of interventions to choose from
        intvScroll = InterventionMenu.GetComponentInChildren<ScrollRect>();
        //contentscroll is an element of intvScroll that we are going to use to actually put the text elements in
        contentScroll = intvScroll.content.GetComponent<RectTransform>();
        contentScroll.anchoredPosition = new Vector2(pos.x, 0.0f);
        

        fillScrollView();

    }
    //function to fill the scroll view for the first time
    private void fillScrollView()
    {
        //add as much interventions as are in the array that we retrieved using "LoadJsonInterventions"
        for (int i = 0; i < interventions.interventions.Length; i++)
        {
            //instantiate the text element
            Panels.Add(Instantiate(Text, intvScroll.content.transform));

            initiateTextObject(Panels[i], interventions.interventions[i].intervention, pos);
            //VVVV
            pos = new Vector2(pos.x + 155.0f, pos.y);
            Panels[i].name = "button " + i;

            //add an eventListener that triggers when the user clicks the text element and then executes clickAdvice() 
            //with the correct ID for the current Intervention
            EventTrigger trigger = Panels[i].GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            int ID = i;
            entry.callback.AddListener((eventData) => { clickAdvice(ID); });
            trigger.triggers.Add(entry);

                textCount++;
            
            //if more than 4(the length of the window is as long as 4 textelements) textelements are added, lengthen the scrollview content containing the interventions
            if (i >= 4)
            {
                contentScroll.sizeDelta = new Vector2(contentScroll.sizeDelta.x + 160, contentScroll.sizeDelta.y);
                contentScroll.anchoredPosition = new Vector2(contentScroll.anchoredPosition.x + 80, contentScroll.anchoredPosition.y);
            }
        }

    }

    // function to execute when the player clicks an intervention
    private void clickAdvice(int selected)
    {
       //delete all the current elements
        foreach(GameObject g in Panels)
        {
            Destroy(g);
        }
        //create the standard text element that will be used to instantiate all other text elements in this function
        GameObject aText = Instantiate(Text);
        Panels.Add(aText);
        RectTransform textRect = aText.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(textRect.sizeDelta.x + 100, textRect.sizeDelta.y + 40);

        //resize the scrollview's content element;
        contentScroll.sizeDelta = new Vector2(contentScroll.sizeDelta.x - (160 * (textCount - 4)), contentScroll.sizeDelta.y);
        contentScroll.anchoredPosition = new Vector2(5, 0);
        intvScroll.horizontal = false;

        //determine the standard position of all assets
        Vector2 newPos = new Vector2(pos.x - (155.0f * textCount), pos.y-10);

        //create the text that tells the player the intervention they have chosen
        GameObject ChosenText = Instantiate(aText, intvScroll.content.transform);
        Panels.Add(ChosenText);
        initiateTextObject(ChosenText, "Je hebt gekozen voor de interventie: \n\n" + interventions.interventions[selected].intervention, newPos);

        //create the text that tells the player the advice concerning the intervention they have chosen
        GameObject AdviceText = Instantiate(aText, intvScroll.content.transform);
        Panels.Add(AdviceText);
        initiateTextObject(AdviceText, "Het volgende advies hoort bij je gekozen interventie: \n" + interventions.interventions[selected].advice, new Vector2(newPos.x + 255.0f, newPos.y));
       
        //create a button with an onclick that will execute showFinished()
        GameObject NextButton = Instantiate(Button, intvScroll.content.transform);
        RectTransform NextButtonTransform = NextButton.GetComponent<RectTransform>();
        Panels.Add(NextButton);
        initiateTextObject(NextButton, "Doorgaan", new Vector2(newPos.x, -(0.5f * contentScroll.sizeDelta.y) + NextButtonTransform.sizeDelta.y));
        NextButton.GetComponent<Button>().onClick.AddListener(delegate { showFinished(selected); });
       

    }

    //function to fill the scrollview with the screen for a finished level 
    private void showFinished(int selected)
    {
        //delete all the current elements
        foreach (GameObject g in Panels)
        {
            Destroy(g);
        }
        //create a variable for the intervention the player selected
        Intervention selectedIntervention = interventions.interventions[selected];


        //create the standard text element that will be used to instantiate all other text elements in this function
        GameObject aText = Instantiate(Text);
        Panels.Add(aText);
        RectTransform textRect = aText.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(textRect.sizeDelta.x *2, textRect.sizeDelta.y + 40);

        //determine the standard position of all assets

        Vector2 newPos = new Vector2(pos.x - (155.0f * textCount), pos.y - 10);

        //create the text that will tell the player what changed with their skills
        GameObject ChosenText = Instantiate(aText, intvScroll.content.transform);
        Panels.Add(ChosenText);
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
