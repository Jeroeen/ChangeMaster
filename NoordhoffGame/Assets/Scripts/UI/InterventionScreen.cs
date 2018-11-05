using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameSaveLoad.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class InterventionScreen : MonoBehaviour
{
	[SerializeField] private Button infoButton;
	[SerializeField] private Button settingsButton;
	[SerializeField] private GameObject interventionscreen;
	[SerializeField] private GameObject text;
	[SerializeField] private GameObject button;
	[SerializeField] private GameObject skillPanel;
	[SerializeField] private CanvasGroup blockingPanel;
	[SerializeField] private Transition transition;

	private Player player;
	private Vector2 position = new Vector2(0.0f, 0.0f);
	private int textCount = 0;
	private ScrollRect interventionScroll;
	private RectTransform scrollviewContent;
	private List<GameObject> panels = new List<GameObject>();
	private InterventionList interventions;
	private RetrieveJson json;
	private float textboxSizeX;
	private float elementLimit;
	private bool isFading;

	// Start is called before the first frame update
	void Start()
	{
		//retrieve the list of interventions for this lvl(level 1) from the associated Json file
		json = new RetrieveJson();
		player = Player.GetPlayer();
		interventions = json.LoadJsonInterventions(1);
		//interventionScroll is the ScrollRect that contains the list of interventions to choose from
		interventionScroll = interventionscreen.GetComponentInChildren<ScrollRect>();
		//set the content element of the scrollview to the position 0,0 since, for some reason, sometimes it moves away from that position
		scrollviewContent = interventionScroll.content.GetComponent<RectTransform>();
		scrollviewContent.anchoredPosition = new Vector2(0.0f, 0.0f);
		//get the size of the text box for use further in the file
		RectTransform textRect = text.GetComponent<RectTransform>();
		textboxSizeX = textRect.sizeDelta.x;
		position = new Vector2(textboxSizeX / 8, -textboxSizeX / 8);
		FillScrollView();

	}

	void Update()
	{
		if (!isFading) return;

		if (!transition.transform.gameObject.activeSelf)
		{
			transition.transform.gameObject.SetActive(true);
		}
		else
		{
			if (transition.FadeOut())
			{
				SceneManager.LoadScene("Baseview");
			}
		}
	}

	//function to fill the scroll view for the first time
	private void FillScrollView()
	{
		//add as much interventions as are in the array that we retrieved using "LoadJsonInterventions"
		for (int i = 0; i < interventions.Interventions.Length; i++)
		{
			//instantiate the text element and add it to the list that holds all the UI elements
			panels.Add(Instantiate(text, interventionScroll.content.transform));

			InitiateTextObject(panels[i], interventions.Interventions[i].InterventionText, position);
			position = new Vector2(position.x + textboxSizeX + 5.0f, position.y);
			//set the name of the button that we'll later be using to see what intervention was selected
			panels[i].name = "button " + i;

			//add an eventListener that triggers when the user clicks the text element and then executes clickAdvice() 
			//with the correct ID for the current Intervention
			EventTrigger trigger = panels[i].GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
			int id = i;
			entry.callback.AddListener((eventData) => { ClickAdvice(id); });
			trigger.triggers.Add(entry);

			//count the amount of text elements
			textCount++;
			elementLimit = scrollviewContent.sizeDelta.x / textboxSizeX;
			//if more than 4(the length of the window is as long as 4 textelements) textelements are added, lengthen the scrollview content containing the interventions
			if (i >= elementLimit - 1)
			{
				scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x + textboxSizeX, scrollviewContent.sizeDelta.y);
				scrollviewContent.anchoredPosition = new Vector2(scrollviewContent.anchoredPosition.x + textboxSizeX / 2, scrollviewContent.anchoredPosition.y);
			}
		}
	}

	// function to execute when the player clicks an intervention
	private void ClickAdvice(int selected)
	{
		//delete all the current elements
		foreach (GameObject g in panels)
		{
			Destroy(g);
		}
		//create the standard text element that will be used to instantiate all other text elements in this function
		GameObject aText = Instantiate(text);
		panels.Add(aText);
		RectTransform textRect = aText.GetComponent<RectTransform>();
		textRect.sizeDelta = new Vector2(textRect.sizeDelta.x * 2, textRect.sizeDelta.y + textRect.sizeDelta.y / 4);

		//resize the scrollview's content element;
		scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x - (textboxSizeX + 10.0f) * (textCount - 4), scrollviewContent.sizeDelta.y);
		scrollviewContent.anchoredPosition = new Vector2(5.0f, 0.0f);
		interventionScroll.horizontal = false;

		//determine the standard position of all assets
		Vector2 newPos = new Vector2(position.x - (textboxSizeX + 5.0f) * textCount, position.y - 10);
		textboxSizeX = textboxSizeX * 2;

		//create the text that tells the player the intervention they have chosen
		GameObject chosenText = Instantiate(aText, interventionScroll.content.transform);
		panels.Add(chosenText);
		InitiateTextObject(chosenText, "Je hebt gekozen voor de interventie: \n\n" + interventions.Interventions[selected].InterventionText, newPos);

		//create the text that tells the player the advice concerning the intervention they have chosen
		GameObject adviceText = Instantiate(aText, interventionScroll.content.transform);
		panels.Add(adviceText);
		InitiateTextObject(adviceText, "Het volgende advies hoort bij je gekozen interventie: \n" + interventions.Interventions[selected].Advice, new Vector2(newPos.x + textboxSizeX, newPos.y));

		//create a button with an onclick that will execute showFinished()
		GameObject nextButton = Instantiate(button, interventionScroll.content.transform);
		RectTransform nextButtonTransform = nextButton.GetComponent<RectTransform>();
		panels.Add(nextButton);
		InitiateTextObject(nextButton, "Doorgaan", new Vector2(newPos.x, -(0.5f * scrollviewContent.sizeDelta.y) + nextButtonTransform.sizeDelta.y));
		nextButton.GetComponent<Button>().onClick.AddListener(delegate { ShowFinished(selected); });
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

		player.Analytic += selectedIntervention.Analytic;
		player.Enthousiasm += selectedIntervention.Enthusiasm;
		player.Decisive += selectedIntervention.Decisive;
		player.Empathic += selectedIntervention.Empathic;
		player.Convincing += selectedIntervention.Convincing;
		player.Creative += selectedIntervention.Creative;
		player.ChangeKnowledge += selectedIntervention.ChangeKnowledge;

		//create the standard text element that will be used to instantiate all other text elements in this function
		GameObject aText = Instantiate(text);
		panels.Add(aText);
		RectTransform textRect = aText.GetComponent<RectTransform>();
		textRect.sizeDelta = new Vector2(textRect.sizeDelta.x * 2, textRect.sizeDelta.y / 1.5f);

		//determine the standard position of all assets
		Vector2 newPos = new Vector2(position.x - textboxSizeX / 2 * textCount, position.y);

		//create the text that will tell the player by how much their skills will rise
		GameObject chosenTextObject = Instantiate(aText, interventionScroll.content.transform);
		panels.Add(chosenTextObject);

		RectTransform chosenTextPos = chosenTextObject.GetComponent<RectTransform>();
		chosenTextPos.anchoredPosition = new Vector2(newPos.x / 2, newPos.y);
		Text chosenText = chosenTextObject.GetComponentInChildren<Text>();

		//add the 7 panels that will tell the player by how much their skills will rise
		Vector2 skillpos = new Vector2(newPos.x, newPos.y - chosenTextPos.sizeDelta.y - 10);
		RetrieveAsset.RetrieveAssets();

		string[] skillSpriteNames = { "Analytisch", "Enthousiasmerend", "Besluitvaardig", "Empatisch", "Overtuigend", "Creatief", "Kennis van veranderkunde" };
		List<GameObject> scorePanels = new List<GameObject>();
		for (int i = 0; i < 3; i++)
		{
			scorePanels.Add(Instantiate(skillPanel, interventionScroll.content.transform));
			InitiateTextObject(scorePanels[i * 2], selectedIntervention.ChangeKnowledge.ToString(), skillpos);
			skillpos.x += scorePanels[i * 2].GetComponent<RectTransform>().sizeDelta.x;
			Image[] infoImage = scorePanels[i * 2].GetComponentsInChildren<Image>();

			infoImage[1].sprite = RetrieveAsset.GetSpriteByName(skillSpriteNames[i * 2]);

			scorePanels.Add(Instantiate(skillPanel, interventionScroll.content.transform));
			InitiateTextObject(scorePanels[i * 2 + 1], selectedIntervention.ChangeKnowledge.ToString(), skillpos);
			skillpos.y -= scorePanels[i * 2 + 1].GetComponent<RectTransform>().sizeDelta.y * 1.5f;
			skillpos.x -= scorePanels[i * 2].GetComponent<RectTransform>().sizeDelta.x;

			infoImage = scorePanels[i * 2 + 1].GetComponentsInChildren<Image>();
			infoImage[1].sprite = RetrieveAsset.GetSpriteByName(skillSpriteNames[i * 2 + 1]);
		}
		GameObject skillpanel = Instantiate(skillPanel, interventionScroll.content.transform);

		InitiateTextObject(skillpanel, selectedIntervention.ChangeKnowledge.ToString(), new Vector2(skillpos.x + 2 * scorePanels[2].GetComponent<RectTransform>().sizeDelta.x, skillpos.y + scorePanels[2].GetComponent<RectTransform>().sizeDelta.y * 3));
		Text[] infoText = skillpanel.GetComponentsInChildren<Text>();
		infoText[0].text = selectedIntervention.ChangeKnowledge.ToString();
		RetrieveAsset.RetrieveAssets();

		Image[] veranderkundeImage = skillpanel.GetComponentsInChildren<Image>();
		veranderkundeImage[1].sprite = RetrieveAsset.GetSpriteByName("Kennis van veranderkunde");

		chosenText.text = "Gefeliciteerd " + player.GetPlayerTitle() + "! \n"
			+ "Je hebt level 1 gehaald en daarbij de volgende skills gehaald";

		//determine the standard position of all assets
		newPos = new Vector2(newPos.x + textboxSizeX, newPos.y);
		//create the text that will tell the player what changed with their skills and set that textbox to the correct position
		GameObject pChosenText = Instantiate(aText, interventionScroll.content.transform);
		panels.Add(pChosenText);

		RectTransform pTextPos = pChosenText.GetComponent<RectTransform>();
		pTextPos.anchoredPosition = new Vector2(newPos.x, newPos.y);

		Text playerText = pChosenText.GetComponentInChildren<Text>();
		playerText.text = "Je skills zijn nu zo hoog \n";

		//put pictures from the 7 different skills on the screen and how high the player has trained those skills
		skillpos = new Vector2(newPos.x, newPos.y - chosenTextPos.sizeDelta.y - 10);

		List<GameObject> skillPanels = new List<GameObject>();

		int[] scores = { player.Analytic, player.Enthousiasm, player.Decisive, player.Empathic, player.Convincing, player.Creative, player.ChangeKnowledge };

		for (int i = 0; i < 3; i++)
		{
			skillPanels.Add(Instantiate(skillPanel, interventionScroll.content.transform));
			InitiateTextObject(skillPanels[i * 2], scores[i * 2].ToString(), skillpos);
			skillpos.x += skillPanels[i * 2].GetComponent<RectTransform>().sizeDelta.x;
			Image[] infoImage = skillPanels[i * 2].GetComponentsInChildren<Image>();

			infoImage[1].sprite = RetrieveAsset.GetSpriteByName(skillSpriteNames[i * 2]);

			skillPanels.Add(Instantiate(skillPanel, interventionScroll.content.transform));
			InitiateTextObject(skillPanels[i * 2 + 1], scores[i * 2].ToString(), skillpos);
			skillpos.y -= skillPanels[i * 2 + 1].GetComponent<RectTransform>().sizeDelta.y * 1.5f;
			skillpos.x -= skillPanels[i * 2].GetComponent<RectTransform>().sizeDelta.x;

			infoImage = skillPanels[i * 2 + 1].GetComponentsInChildren<Image>();
			infoImage[1].sprite = RetrieveAsset.GetSpriteByName(skillSpriteNames[i * 2 + 1]);
		}

		GameObject veranderkundepanel = Instantiate(skillPanel, interventionScroll.content.transform);

		InitiateTextObject(veranderkundepanel, scores[6].ToString(), new Vector2(skillpos.x + 2 * scorePanels[2].GetComponent<RectTransform>().sizeDelta.x, skillpos.y + scorePanels[2].GetComponent<RectTransform>().sizeDelta.y * 3));
		Text veranderkundeScore = skillpanel.GetComponentInChildren<Text>();
		veranderkundeScore.text = selectedIntervention.ChangeKnowledge.ToString();
		RetrieveAsset.RetrieveAssets();

		veranderkundeImage = veranderkundepanel.GetComponentsInChildren<Image>();
		veranderkundeImage[1].sprite = RetrieveAsset.GetSpriteByName("Kennis van veranderkunde");

		GameObject confirmButton = Instantiate(button, interventionScroll.content.transform);
		RectTransform confirmButtonTransform = confirmButton.GetComponent<RectTransform>();
		Vector2 sizeDelta = confirmButtonTransform.sizeDelta;
		sizeDelta.x = 1;
		sizeDelta.y = 1;

		panels.Add(confirmButton);
		InitiateTextObject(confirmButton, "Afronden", new Vector2(newPos.x - 200, -(0.6f * scrollviewContent.sizeDelta.y) + confirmButtonTransform.sizeDelta.y));
		confirmButton.GetComponent<Button>().onClick.AddListener(FinishLevel);
	}

	public void FinishLevel()
	{
		isFading = true;
	}

	//a function that will enable or disable the menu 
	public void ShowMenu()
	{
		interventionscreen.SetActive(!interventionscreen.gameObject.activeSelf);
		blockingPanel.blocksRaycasts = !blockingPanel.blocksRaycasts;

		infoButton.interactable = !infoButton.IsInteractable();
		settingsButton.interactable = !settingsButton.IsInteractable();
	}
	//a function to easily set the text and position of a textobject
	private void InitiateTextObject(GameObject initiate, string text, Vector2 position)
	{
		//set the text of the textObject
		Text objectText = initiate.GetComponentInChildren<Text>();
		objectText.text = text;
		//set the positionof the textObject
		RectTransform cTextPos = initiate.GetComponent<RectTransform>();
		cTextPos.anchoredPosition = position;
	}
}