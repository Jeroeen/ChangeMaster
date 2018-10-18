using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InterventionScreen : MonoBehaviour
{
    public CanvasGroup InterventionMenu;
    public GameObject Text;
    public EventSystem eventSystem;


    private Vector2 pos = new Vector2(5.0f, 45.0f);
    private int textCount = 0;
    private ScrollRect intvScroll;
    private RectTransform contentScroll;
    private List<GameObject> Panels = new List<GameObject>();
    private string[] text;
    private string[] advies;
    private List<Intervention> interventionText;

    // Start is called before the first frame update
    void Start()
    {
        intvScroll = InterventionMenu.GetComponentInChildren<ScrollRect>();
        contentScroll = intvScroll.content.GetComponent<RectTransform>();
        contentScroll.anchoredPosition = new Vector2(pos.x, 0.0f);

        text = new string[]
        {
            "Ontslag van de zieke medewerkers.",
            "Medewerkers ziek laten zijn en schoonmaakpersoneel inzetten om daar te werken.",
            "Berisping van de teamleider om normale roosters te maken en ze niet meer te pesten.",
            "Gesprek met de teamleider en haar problemen oplossen.",
            "Teamcoaching opstarten om samen te zoeken naar doel en werksfeer. ",
            "Medewerkers ziek laten zijn en de kinderspeelplek een deel van de dag dicht doen.",
            "Medewerkers ziek laten zijn en de overige animatiemedewerkers alleen voor de groep te zetten. "
        };

        advies = new string[]
        {
            "Deze zou kunnen, maar het nadeel is dat het probleem niet bij hun ligt. De keuze is daarom niet de beste optie.",
            "Kan, maar daarmee los je de oorzaak niet op. Daarnaast creëer je een angstcultuur. Niet de beste keus. ",
            "Kan, maar de teamleider doet het ergens om, dus meer zoeken naar de oorzaak en die met elkaar aanpakken",
            "Kan, maar dat is eenzijdig. Het probleem zit in het team. Niet de beste keus.",
            "Beste keus. Duurt iets langer, maar zorgt voor de meest duurzame oplossing van gedrag. Daarin wordt de teamsamenwerking besproken en gekeken naar wat ieder teamlid nodig heeft. ",
            "Kan, maar daarmee los je de oorzaak niet op. Tevredenheid bij gasten gaat naar beneden. Niet de beste keus",
            "Kan, maar daarmee los je de oorzaak niet op. Grote kans dat de rest ook ziek wordt door werkdruk. Niet de beste keus"
        };
        

        
        for (int i = 0; i < text.Length; i++)
        {
            
            Panels.Add(Instantiate(Text, intvScroll.content.transform));
            Text[] intvText = Panels[i].GetComponentsInChildren<Text>();
            intvText[0].text = text[i];
            Panels[i].name = "button " + i;

            EventTrigger trigger = Panels[i].GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            int ID = i;
            entry.callback.AddListener((eventData) => { clickBehaviour(ID); });
            trigger.triggers.Add(entry);


            RectTransform[] intvRect = Panels[i].GetComponents<RectTransform>();
            intvRect[0].anchoredPosition = pos;
            pos = new Vector2(pos.x + 155.0f, pos.y );
            
            if (i >= 4)
            {
                textCount++;
                contentScroll.sizeDelta = new Vector2(contentScroll.sizeDelta.x + 160, contentScroll.sizeDelta.y );
                contentScroll.anchoredPosition = new Vector2(contentScroll.anchoredPosition.x + 80, contentScroll.anchoredPosition.y);
            }
        }

    }

    public void clickBehaviour(int selected)
    {
       
        foreach(GameObject g in Panels)
        {
            Destroy(g);
        }
        GameObject aText = Instantiate(Text);
        RectTransform textRect = aText.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(textRect.sizeDelta.x + 100, textRect.sizeDelta.y + 40);
        contentScroll.sizeDelta = new Vector2(contentScroll.sizeDelta.x - (160 * textCount), contentScroll.sizeDelta.y);
        contentScroll.anchoredPosition = new Vector2(5, 45);
        intvScroll.horizontal = false;

        textCount += 4;
        Vector2 newPos = new Vector2(pos.x - (155.0f * textCount), pos.y - 45);


        GameObject ChosenText = Instantiate(aText, intvScroll.content.transform);
        RectTransform[] cTextPos = ChosenText.GetComponents<RectTransform>();
        cTextPos[0].anchoredPosition = newPos;
        Text chosenText = ChosenText.GetComponentInChildren<Text>();
        chosenText.text = "Je hebt gekozen voor de interventie: \n" + text[selected];

        GameObject AdviceText = Instantiate(aText, intvScroll.content.transform);
        RectTransform[] aTextPos = AdviceText.GetComponents<RectTransform>();
        aTextPos[0].anchoredPosition = new Vector2(newPos.x + 255.0f, newPos.y);
        Text adviceText = AdviceText.GetComponentInChildren<Text>();
        adviceText.text = "Het volgende advies hoort bij je gekozen interventie: \n" + advies[selected];

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
