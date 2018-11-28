using System.Collections.Generic;
using Assets.Scripts.Cutscene;
using Assets.Scripts.Dialogue;
using Assets.Scripts.GameSaveLoad;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class InterventionScreen : MonoBehaviour
	{
		[SerializeField] private Button interventionButton;
		[SerializeField] private Button infoButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private GameObject interventionscreen;
		[SerializeField] private GameObject textPrefab;
		[SerializeField] private GameObject textImagePrefab;
		[SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GameObject hint;
        [SerializeField] private GameObject skillPanel;
		[SerializeField] private CanvasGroup blockingPanel;
		[SerializeField] private Transition transition;
		[SerializeField] private GameObject interventionWarning;
		[SerializeField] private GameObject confirmInterventionGameObject;
		[SerializeField] private SpriteRenderer chosenInterventionSprite;

		private int clickedElementId;
		private Player player;
		private Vector2 position;
		private int textCount;
		private ScrollRect interventionScrollView;
		private RectTransform scrollviewContent;
		private List<GameObject> uiElements = new List<GameObject>();
		private InterventionList interventions;
		private RetrieveJson json;
		private float textboxSizeX;
		private float textboxSizeY;
		private float elementLimit;
		private bool isFading;
		private Game game;

		// Start is called before the first frame update
		void Start()
		{
			SaveLoadGame.Load();
			game = Game.GetGame();

			if (game.Player == null)
			{
				game.Player = Player.GetPlayer();
				SaveLoadGame.Save();
			}

			json = new RetrieveJson();
			interventions = json.LoadJsonInterventions(SceneManager.GetActiveScene().name);

			interventionScrollView = interventionscreen.GetComponentInChildren<ScrollRect>();
			scrollviewContent = interventionScrollView.content.GetComponent<RectTransform>();
			scrollviewContent.anchoredPosition = new Vector2();

			RectTransform textRect = textImagePrefab.GetComponent<RectTransform>();
			textboxSizeX = textRect.sizeDelta.x;
			textboxSizeY = textRect.sizeDelta.y;

			position = new Vector2(textboxSizeX / GlobalVariablesHelper.TEXTBOX_DIVIDER,
				-textboxSizeX / GlobalVariablesHelper.TEXTBOX_DIVIDER);

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
					SceneManager.LoadScene(GlobalVariablesHelper.BASEVIEW_SCENE_INDEX);
				}
			}
		}

		private void FillScrollView()
		{
			player = Player.GetPlayer();

			for (int i = 0; i < interventions.Interventions.Length; i++)
			{
				uiElements.Add(Instantiate(textImagePrefab, interventionScrollView.content.transform));
				InitiateTextObject(uiElements[i], interventions.Interventions[i].InterventionText, position);

				RetrieveAsset.RetrieveAssets();
				Sprite interventionIcon = RetrieveAsset.GetSpriteByName(interventions.Interventions[i].InterventionImage);

				Image[] objectImage = uiElements[i].GetComponentsInChildren<Image>();

				//objectimage[1] is the image element that is contained within the panel, the one i want to change, 
				//[0] is the image from  the panel containing it
				objectImage[1].sprite = interventionIcon;


				position = new Vector2(position.x + textboxSizeX + textboxSizeX / GlobalVariablesHelper.TEXTBOX_DIVIDER,
					position.y);
				uiElements[i].name = "button " + i;

				EventTrigger trigger = uiElements[i].GetComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };


				int id = i;
				entry.callback.AddListener(eventData => { Confirm(id); });
				trigger.triggers.Add(entry);

                Button hintButton = uiElements[i].GetComponentInChildren<Button>();

                hintButton.onClick.AddListener(delegate () { Showhint(id); });

                textCount++;
				elementLimit = scrollviewContent.sizeDelta.x / textboxSizeX;

                //i starts at 0, so to compensate we subtract 1 from the elementlimit
                if (i >= elementLimit - 1) 
				{
					scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x + textboxSizeX,
						scrollviewContent.sizeDelta.y);
					//when scrollviewContent is made bigger, we have to compensate the position, 
					//so we make it 1/2 the size of textboxSizeX to the right, so it's correct now
					scrollviewContent.anchoredPosition = new Vector2(scrollviewContent.anchoredPosition.x + textboxSizeX / 2,
						scrollviewContent.anchoredPosition.y);
				}
			}
		}

		private void Confirm(int id)
		{
			clickedElementId = id;
			Sprite interventionSprite = RetrieveAsset.GetSpriteByName(interventions.Interventions[clickedElementId].InterventionImage);
			chosenInterventionSprite.sprite = interventionSprite;
			confirmInterventionGameObject.SetActive(true);
		}

        private void Showhint(int id)
        {
            CanvasGroup hintCanvas = hint.GetComponent<CanvasGroup>();
            hintCanvas.blocksRaycasts = true;
            Text hintText= hint.GetComponentInChildren<Text>();
            hintText.text = interventions.Interventions[id].Hint;
            hint.SetActive(true);
        }

		public void ConfirmInterventionChoice()
		{
			confirmInterventionGameObject.SetActive(false);
			
			ClickAdvice(clickedElementId);
		}

		private void ClickAdvice(int selected)
		{
			foreach (GameObject g in uiElements)
			{
				Destroy(g);
			}
			RectTransform textRect = textPrefab.GetComponent<RectTransform>();
			textRect.sizeDelta = new Vector2(textboxSizeX * GlobalVariablesHelper.ADVICE_TEXT_X_MULTIPLIER,
				textboxSizeY + textboxSizeY / GlobalVariablesHelper.ADVICE_TEXT_Y_DIVIDER);

			scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x - (textboxSizeX) * (textCount - 4),
				scrollviewContent.sizeDelta.y);
			scrollviewContent.anchoredPosition = new Vector2(10.0f, 0.0f);
			interventionScrollView.horizontal = false;

			Vector2 newPos = new Vector2(textboxSizeX / GlobalVariablesHelper.TEXTBOX_DIVIDER, position.y);
			textboxSizeX = textboxSizeX * GlobalVariablesHelper.ADVICE_TEXT_X_MULTIPLIER;

			GameObject chosenText = Instantiate(textPrefab, interventionScrollView.content.transform);
			uiElements.Add(chosenText);
			InitiateTextObject(chosenText, "Je hebt gekozen voor de interventie: \n\n"
										   + interventions.Interventions[selected].InterventionText, newPos);

			GameObject adviceText = Instantiate(textPrefab, interventionScrollView.content.transform);
			uiElements.Add(adviceText);
			InitiateTextObject(adviceText, "Het volgende advies hoort bij je gekozen interventie: \n"
										   + interventions.Interventions[selected].Advice,
				new Vector2(newPos.x + textboxSizeX + textboxSizeX / GlobalVariablesHelper.TEXTBOX_DIVIDER, newPos.y));
			GameObject nextButton = Instantiate(buttonPrefab, interventionScrollView.content.transform);
			RectTransform nextButtonTransform = nextButton.GetComponent<RectTransform>();
			uiElements.Add(nextButton);

			InitiateTextObject(nextButton, "Doorgaan", new Vector2(
				-nextButtonTransform.sizeDelta.x * GlobalVariablesHelper.BUTTON_MULTIPLIER,
				nextButtonTransform.sizeDelta.y * GlobalVariablesHelper.BUTTON_MULTIPLIER));

			nextButton.GetComponent<Button>().onClick.AddListener(delegate { ShowFinished(selected); });
		}

		//function to fill the scrollview with the screen for a finished level 
		private void ShowFinished(int selected)
		{
			foreach (GameObject g in uiElements)
			{
				Destroy(g);
			}

			Intervention selectedIntervention = interventions.Interventions[selected];

			player.Analytic += selectedIntervention.Analytic;
			player.Approach += selectedIntervention.Approach;
			player.Ownership += selectedIntervention.Ownership;
			player.Facilitates += selectedIntervention.Facilitates;
			player.Communication += selectedIntervention.Communication;
            game.Player = player;
			SaveLoadGame.Save();

			RectTransform textRect = textPrefab.GetComponent<RectTransform>();

			textRect.sizeDelta = new Vector2(textboxSizeX, textboxSizeY / GlobalVariablesHelper.FINISH_TEXT_Y_DIVIDER);

			Vector2 newStandardPosition = new Vector2(GlobalVariablesHelper.FINISH_STANDARD_POSITION_X, position.y);

			GameObject chosenTextObject = Instantiate(textPrefab, interventionScrollView.content.transform);
			uiElements.Add(chosenTextObject);

			RectTransform chosenTextPos = chosenTextObject.GetComponent<RectTransform>();
			chosenTextPos.anchoredPosition = new Vector2(newStandardPosition.x, newStandardPosition.y);
			Text chosenText = chosenTextObject.GetComponentInChildren<Text>();

			Vector2 skillpos = new Vector2(newStandardPosition.x, newStandardPosition.y - chosenTextPos.sizeDelta.y);
			RetrieveAsset.RetrieveAssets();

			string[] skillSpriteNames =
			{
				"Analytisch", "x button", "x button",
                "x button", "Communiceren"
            };

			int[] interventionScores =
			{
				selectedIntervention.Analytic, selectedIntervention.Approach, selectedIntervention.Ownership,
				selectedIntervention.Facilitates, selectedIntervention.Communication
			};

			showSkills(skillSpriteNames.Length, skillSpriteNames, interventionScores, skillpos);

			chosenText.text = "Gefeliciteerd " + player.GetPlayerTitle() + "! \n"
							  + "Je hebt level " + (game.LastFinishedLevel - GlobalVariablesHelper.BASE_LEVEL_INDEX) + " gehaald en daarbij de volgende skills gehaald";

			newStandardPosition = new Vector2(newStandardPosition.x + textboxSizeX, newStandardPosition.y);

			GameObject pChosenText = Instantiate(textPrefab, interventionScrollView.content.transform);
			uiElements.Add(pChosenText);

			RectTransform pTextPos = pChosenText.GetComponent<RectTransform>();
			pTextPos.anchoredPosition = new Vector2(newStandardPosition.x, newStandardPosition.y);

			Text playerText = pChosenText.GetComponentInChildren<Text>();
			playerText.text = "Je skills zijn nu zo hoog \n";

			skillpos = new Vector2(newStandardPosition.x, newStandardPosition.y - chosenTextPos.sizeDelta.y);

			int[] playerScores =
			{
				player.Analytic, player.Approach, player.Ownership, player.Facilitates, player.Communication
			};
			showSkills(skillSpriteNames.Length, skillSpriteNames, playerScores, skillpos);


			GameObject confirmButton = Instantiate(buttonPrefab, interventionScrollView.content.transform);
			RectTransform confirmButtonTransform = confirmButton.GetComponent<RectTransform>();

			uiElements.Add(confirmButton);
			InitiateTextObject(confirmButton, "Afronden",
				new Vector2(-confirmButtonTransform.sizeDelta.x * GlobalVariablesHelper.BUTTON_MULTIPLIER,
					confirmButtonTransform.sizeDelta.y * GlobalVariablesHelper.BUTTON_MULTIPLIER));
			confirmButton.GetComponent<Button>().onClick.AddListener(FinishLevel);
		}

		public void showSkills(int rows, string[] spriteNames, int[] skillNumbers, Vector2 basePosition)
		{
			int j = new int();
			List<GameObject> scorePanels = new List<GameObject>();

			//divide the rows by 2 because we will be adding 2 skills each loop
			for (int i = 0; i < rows / 2; i++)
			{
				j = i * 2;

				scorePanels.Add(Instantiate(skillPanel, interventionScrollView.content.transform));
				InitiateTextObject(scorePanels[j], skillNumbers[j].ToString(), basePosition);

				Image[] skillImage = scorePanels[j].GetComponentsInChildren<Image>();
				skillImage[1].sprite = RetrieveAsset.GetSpriteByName(spriteNames[j]);

				basePosition.x += scorePanels[j].GetComponent<RectTransform>().sizeDelta.x;

				scorePanels.Add(Instantiate(skillPanel, interventionScrollView.content.transform));
				InitiateTextObject(scorePanels[j + 1], skillNumbers[j + 1].ToString(), basePosition);

				basePosition.y -= scorePanels[j + 1].GetComponent<RectTransform>().sizeDelta.y *
								  GlobalVariablesHelper.SKILL_POSITION_MULTIPIER;
				basePosition.x -= scorePanels[j].GetComponent<RectTransform>().sizeDelta.x;

				skillImage = scorePanels[j + 1].GetComponentsInChildren<Image>();
				skillImage[1].sprite = RetrieveAsset.GetSpriteByName(spriteNames[j + 1]);
			}

			//if there is an uneven amount of rows, add one more, 
			if (rows % 2 == 0)
			{
				return;
			}

			j += 2;
			GameObject skillpanel = Instantiate(skillPanel, interventionScrollView.content.transform);

			InitiateTextObject(skillpanel, skillNumbers[j].ToString(),
				new Vector2(basePosition.x + 2 * scorePanels[2].GetComponent<RectTransform>().sizeDelta.x,
					basePosition.y + scorePanels[2].GetComponent<RectTransform>().sizeDelta.y * 3));

			Text infoText = skillpanel.GetComponentInChildren<Text>();
			infoText.text = skillNumbers[j].ToString();
			RetrieveAsset.RetrieveAssets();

			Image[] changemanagementImage = skillpanel.GetComponentsInChildren<Image>();
			changemanagementImage[1].sprite = RetrieveAsset.GetSpriteByName(spriteNames[j]);
		}

		public void FinishLevel()
		{
			Game.GetGame().AddLevel();
			isFading = true;
		}

		public void ShowMenu()
		{
			game = Game.GetGame();
			if (game.Information != null && !interventionscreen.activeSelf)
			{
				for (int i = 0; i < game.Information.InformationList.Length; i++)
				{
					if (!game.Information.InformationList[i].Found)
					{
						ShowWarning();
						break;
					}
				}
			}
			interventionscreen.SetActive(!interventionscreen.gameObject.activeSelf);
			blockingPanel.blocksRaycasts = !blockingPanel.blocksRaycasts;
			settingsButton.interactable = !settingsButton.IsInteractable();
			infoButton.interactable = !infoButton.IsInteractable();
			interventionButton.interactable = !interventionButton.IsInteractable();
		}

		private void InitiateTextObject(GameObject initiate, string text, Vector2 position)
		{
			//set the text of the textObject
			Text objectText = initiate.GetComponentInChildren<Text>();
			objectText.text = text;
			//set the positionof the textObject
			RectTransform cTextPos = initiate.GetComponent<RectTransform>();
			cTextPos.anchoredPosition = position;
		}

		public void ShowWarning()
		{
			int amountofStakeholders = game.Information.InformationList.Length;
			int stakeholdersFound = 0;

			for (int i = 0; i < amountofStakeholders; i++)
			{
				if (Game.GetGame().Information.InformationList[i].Found)
				{
					stakeholdersFound++;
				}
			}

			//calculate percentage of stakeholders found
			int percentageFound = stakeholdersFound * 100 / amountofStakeholders;
			Text warningText = interventionWarning.GetComponentInChildren<Text>();
			warningText.text = "Weet je zeker dat je een interventie wilt kiezen? Je hebt nog maar " + percentageFound +
							   "% van de stakeholders gevonden.";
			CanvasGroup warningGroup = interventionWarning.GetComponent<CanvasGroup>();
			warningGroup.blocksRaycasts = true;
			interventionWarning.SetActive(true);


		}
	}
}
