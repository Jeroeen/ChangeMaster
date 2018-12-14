using System.Collections.Generic;
using Assets.Scripts.CameraBehaviour;
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
		[SerializeField] private Button interventionButton = null;
		[SerializeField] private Button infoButton = null;
		[SerializeField] private Button settingsButton = null;
		[SerializeField] private Button consequenceButton = null;
		[SerializeField] private Text hintText = null;
		[SerializeField] private GameObject interventionscreen = null;
		[SerializeField] private GameObject textPrefab = null;
		[SerializeField] private GameObject priorityPrefab = null;
		[SerializeField] private GameObject textImagePrefab = null;
		[SerializeField] private GameObject buttonPrefab = null;
        [SerializeField] private GameObject hint = null;
        [SerializeField] private GameObject skillPanel = null;
        [SerializeField] private GameObject skillInfo = null;
		[SerializeField] private CanvasGroup blockingPanel = null;
		[SerializeField] private CanvasGroup warningBlockingPanel = null;
		[SerializeField] private CanvasGroup confirmBlockingPanel = null;
		[SerializeField] private Transition transition = null;
		[SerializeField] private GameObject interventionWarning = null;
		[SerializeField] private GameObject confirmInterventionGameObject = null;
		[SerializeField] private SpriteRenderer chosenInterventionSprite = null;
	    [SerializeField] private ZoomingObject zoomInterventionScreen = null;

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
		private bool isInterventionChosen;
		private Game game;

		public delegate void DoneCallback();

		public static event DoneCallback OnHintButtonClickEvent;
		public static event DoneCallback OnInterventionChooseButtonClickEvent;

		public delegate void WarningCallback(bool allInfoFound);

		public static event WarningCallback AllInformationFound;

		// Start is called before the first frame update
		void Start()
		{
            isInterventionChosen = false;
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
				entry.callback.AddListener(eventData => { ShowConfirm(id); });
				trigger.triggers.Add(entry);

                Button hintButton = uiElements[i].GetComponentInChildren<Button>();

                hintButton.onClick.AddListener(delegate { ShowConsequence(id); });
                
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

		private void ShowConfirm(int id)
		{
			OnInterventionChooseButtonClickEvent?.Invoke();
		    zoomInterventionScreen.enabled = false;
            clickedElementId = id;
			Sprite interventionSprite = RetrieveAsset.GetSpriteByName(interventions.Interventions[clickedElementId].InterventionImage);
            chosenInterventionSprite.sprite = interventionSprite;
			confirmInterventionGameObject.SetActive(true);
            confirmBlockingPanel.blocksRaycasts = true;
            consequenceButton.onClick.AddListener(delegate { ShowHint(clickedElementId); });
		}

        public void HideHint()
        {
            hintText.text = "";
        }

        private void ShowHint(int id)
        {
            hintText.text = interventions.Interventions[id].Hint;
        }

        private void ShowConsequence(int id)
        {
			OnHintButtonClickEvent?.Invoke();
            zoomInterventionScreen.enabled = false;
            CanvasGroup hintCanvas = hint.GetComponent<CanvasGroup>();
            hintCanvas.blocksRaycasts = true;
            Text hintText= hint.GetComponentInChildren<Text>();
            hintText.text = interventions.Interventions[id].Consequence;
            hint.SetActive(true);
        }

		public void ConfirmInterventionChoice()
		{
			confirmInterventionGameObject.SetActive(false);
            confirmBlockingPanel.blocksRaycasts = false;

            ClickAdvice(clickedElementId);
		}

		private void ClickAdvice(int selected)
		{
            isInterventionChosen = true;
            foreach (GameObject g in uiElements)
			{
				Destroy(g);
			}
			RectTransform textRect = textPrefab.GetComponent<RectTransform>();
			textRect.sizeDelta = new Vector2(textboxSizeX * GlobalVariablesHelper.ADVICE_TEXT_X_MULTIPLIER,
				textboxSizeY + textboxSizeY / GlobalVariablesHelper.ADVICE_TEXT_Y_DIVIDER);

			scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x - textboxSizeX * (textCount - 4),
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
			player.Facilitating += selectedIntervention.Facilitating;
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
				"Analytisch", "Besluitvaardig", "Eigenaarschap",
                "x button", "Communiceren"
            };

            string[] skillInfoText =
            {
                "Denkt analytisch denken vanuit het geheel",
                "Kiest een duurzame veranderaanpak",
                "Gericht op eigenaarschap bij alle stakeholders",
                "Faciliteert de verandering",
                "Communiceert open"
            };

			int[] interventionScores =
			{
				selectedIntervention.Analytic, selectedIntervention.Approach, selectedIntervention.Ownership,
				selectedIntervention.Facilitating, selectedIntervention.Communication
			};

			ShowSkills(skillSpriteNames.Length, skillSpriteNames, skillInfoText, interventionScores, skillpos);

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
				player.Analytic, player.Approach, player.Ownership, player.Facilitating, player.Communication
			};
			ShowSkills(skillSpriteNames.Length, skillSpriteNames, skillInfoText, playerScores, skillpos);

			GameObject confirmButton = Instantiate(buttonPrefab, interventionScrollView.content.transform);
			RectTransform confirmButtonTransform = confirmButton.GetComponent<RectTransform>();

			uiElements.Add(confirmButton);
			InitiateTextObject(confirmButton, "Afronden",
				new Vector2(-confirmButtonTransform.sizeDelta.x * GlobalVariablesHelper.BUTTON_MULTIPLIER,
					confirmButtonTransform.sizeDelta.y * GlobalVariablesHelper.BUTTON_MULTIPLIER));
			confirmButton.GetComponent<Button>().onClick.AddListener(showPriorities);
		}

        private void showPriorities()
        {
            foreach (Transform child in interventionScrollView.content.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            PriorityList priorities = json.LoadJsonPriorities(SceneManager.GetActiveScene().name);
            uiElements.Clear();

            List<string> priorityTexts = new List<string>();
            for (int i = 0; i < priorities.Priorities.Length; i++)
            {
                priorityTexts.Add(priorities.Priorities[i].PriorityText);
            }

            int index = priorityTexts.Count;
            while (index > 1)
            {
                index--;
                int randomNr = Random.Range(0, index);
                string value = priorityTexts[randomNr];
                priorityTexts[randomNr] = priorityTexts[index];
                priorityTexts[index] = value;
            }

            elementLimit = scrollviewContent.sizeDelta.y / priorityPrefab.GetComponent<RectTransform>().sizeDelta.y;
            elementLimit -= 2;
            

            List<GameObject> priorityElements = new List<GameObject>();

            //number used so that the priorityprefab is put bout halfway in the interventionscreen
            float priorityPrefabDivider = 1.7f;

            position = new Vector2(priorityPrefab.GetComponent<RectTransform>().sizeDelta.x / priorityPrefabDivider, -priorityPrefab.GetComponent<RectTransform>().sizeDelta.y);

            for (int i = 0; i < priorities.Priorities.Length; i++)
            {

                uiElements.Add(Instantiate(priorityPrefab, interventionScrollView.content.transform));
                priorityElements.Add(uiElements[i]);
                uiElements[i].GetComponent<RectTransform>().anchoredPosition = position;

                Text[] textElements = uiElements[i].GetComponentsInChildren<Text>();

                textElements[0].text = priorities.Priorities[i].PriorityNumber + "";
                textElements[1].text = priorityTexts[i];

                EventTrigger trigger = uiElements[i].GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };

                GameObject currentPriorityPrefab = uiElements[i];

                entry.callback.AddListener(eventData => { SwitchPriorities(priorityElements, currentPriorityPrefab); });
                trigger.triggers.Add(entry);
            
            
                //1.1f instead of just 1, so that there is at least a little bit of space in between this ui element and the next
                position = new Vector2(position.x, position.y - (1.1f * priorityPrefab.GetComponent<RectTransform>().sizeDelta.y));
                interventionScrollView.vertical = true;

                //i starts at 0, so to compensate we subtract 1 from the elementlimit
                if (i >= elementLimit - 1)
                {
                    scrollviewContent.sizeDelta = new Vector2(scrollviewContent.sizeDelta.x,
                        scrollviewContent.sizeDelta.y + priorityPrefab.GetComponent<RectTransform>().sizeDelta.y);
                    //when scrollviewContent is made bigger, we have to compensate the position, 
                    //so we make it 1/2 the size of textboxSizeX to the right, so it's correct now
                    scrollviewContent.anchoredPosition = new Vector2(scrollviewContent.anchoredPosition.x,
                        scrollviewContent.anchoredPosition.y + priorityPrefab.GetComponent<RectTransform>().sizeDelta.y / 2);
                }
            }
            GameObject confirmButton = Instantiate(buttonPrefab, interventionScrollView.content.transform);
            RectTransform confirmButtonTransform = confirmButton.GetComponent<RectTransform>();


            uiElements.Add(confirmButton);
            InitiateTextObject(confirmButton, "Afronden",
                new Vector2(-confirmButtonTransform.sizeDelta.x * GlobalVariablesHelper.BUTTON_MULTIPLIER,
                    confirmButtonTransform.sizeDelta.y * GlobalVariablesHelper.BUTTON_MULTIPLIER));
            confirmButton.GetComponent<Button>().onClick.AddListener((delegate { ShowPrioritiesDone(priorityElements, priorities); }));
        }
       
        private void ShowPrioritiesDone(List<GameObject> priorityObjects, PriorityList priorities)
        {
            foreach (Transform child in interventionScrollView.content.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            uiElements.Clear();
            string[] priorityOrder = new string[4];


            foreach(GameObject priorityObject in priorityObjects)
            {
                Text[] priorityUIText = priorityObject.GetComponentsInChildren<Text>();
                priorityOrder[System.Int32.Parse(priorityUIText[0].text) - 1] = priorityUIText[1].text;
            }
            int amountCorrect = 0;
            foreach(Priority priority in priorities.Priorities)
            {
                if(priorityOrder[priority.PriorityNumber-1] == priority.PriorityText)
                {
                    amountCorrect++;
                }
            }

            GameObject finishText = Instantiate(textPrefab, interventionScrollView.content.transform);
            uiElements.Add(finishText);
            string CongratulateText = "Gefeliciteerd, je hebt het level helemaal afgemaakt. bij de laatste opdracht had je " 
                                       + amountCorrect + " van de " + priorityOrder.Length.ToString() + " goed.";

            InitiateTextObject(finishText, CongratulateText, new Vector2(textboxSizeX / GlobalVariablesHelper.TEXTBOX_DIVIDER,
                                                          -textboxSizeX / GlobalVariablesHelper.TEXTBOX_DIVIDER));

            GameObject finishButton = Instantiate(buttonPrefab, interventionScrollView.content.transform);
            RectTransform nextButtonTransform = finishButton.GetComponent<RectTransform>();
            uiElements.Add(finishButton);
            nextButtonTransform.sizeDelta = new Vector2(nextButtonTransform.sizeDelta.x ,nextButtonTransform.sizeDelta.y * 2);

            InitiateTextObject(finishButton, "Level afmaken", new Vector2(
                -nextButtonTransform.sizeDelta.x * GlobalVariablesHelper.BUTTON_MULTIPLIER,
                nextButtonTransform.sizeDelta.y * GlobalVariablesHelper.BUTTON_MULTIPLIER));

            finishButton.GetComponent<Button>().onClick.AddListener(delegate { FinishLevel(); });

        }

        private void SwitchPriorities(List<GameObject> draggableObjects, GameObject droppedObject)
        {
            foreach (GameObject draggable in draggableObjects)
            {
                RectTransform uiPanel = draggable.GetComponent<RectTransform>();
                
                if (RectTransformUtility.RectangleContainsScreenPoint(uiPanel, Input.mousePosition, Camera.main))
                {
                    Text[] uiPanelTexts = uiPanel.GetComponentsInChildren<Text>();
                    Text[] droppedObjectTexts = droppedObject.GetComponentsInChildren<Text>();

                    string uiPanelPriority = uiPanelTexts[1].text;
                    uiPanelTexts[1].text = droppedObjectTexts[1].text;
                    droppedObjectTexts[1].text = uiPanelPriority;


                }
            }

        }

		public void ShowSkills(int skillAmount, string[] spriteNames, string[] skillInfoStrings, int[] skillNumbers, Vector2 basePosition)
		{
			int showSkillIndex = 0;
			List<GameObject> skillPanels = new List<GameObject>();
			List<GameObject> skillInfoPanels = new List<GameObject>();
            EventTrigger trigger;
			//divide the rows by 2 because we will be adding 2 skills each loop
			for (int i = 0; i < skillAmount / 2; i++)
			{
				showSkillIndex = i * 2;

				skillPanels.Add(Instantiate(skillPanel, interventionScrollView.content.transform));
				InitiateTextObject(skillPanels[showSkillIndex], skillNumbers[showSkillIndex].ToString(), basePosition);

				Image[] skillImage = skillPanels[showSkillIndex].GetComponentsInChildren<Image>();
				skillImage[1].sprite = RetrieveAsset.GetSpriteByName(spriteNames[showSkillIndex]);

                skillInfoPanels.Add(Instantiate(skillInfo, interventionScrollView.content.transform));
                InitiateTextObject(skillInfoPanels[showSkillIndex], skillInfoStrings[showSkillIndex], skillPanels[showSkillIndex].transform.GetComponent<RectTransform>().anchoredPosition);

                trigger = skillPanels[showSkillIndex].GetComponentInChildren<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };

                int skillInfoIndex1 = showSkillIndex;
                entry.callback.AddListener(eventData => { ShowSkillInfo(skillInfoPanels[skillInfoIndex1]); });
                trigger.triggers.Add(entry);
                basePosition.x += skillPanels[showSkillIndex].GetComponent<RectTransform>().sizeDelta.x;

				skillPanels.Add(Instantiate(skillPanel, interventionScrollView.content.transform));
                InitiateTextObject(skillPanels[showSkillIndex + 1], skillNumbers[showSkillIndex + 1].ToString(), basePosition);

				basePosition.y -= skillPanels[showSkillIndex + 1].GetComponent<RectTransform>().sizeDelta.y *
								  GlobalVariablesHelper.SKILL_POSITION_MULTIPIER;
				basePosition.x -= skillPanels[showSkillIndex].GetComponent<RectTransform>().sizeDelta.x;

				skillImage = skillPanels[showSkillIndex + 1].GetComponentsInChildren<Image>();
				skillImage[1].sprite = RetrieveAsset.GetSpriteByName(spriteNames[showSkillIndex + 1]);

                skillInfoPanels.Add(Instantiate(skillInfo, interventionScrollView.content.transform));
                InitiateTextObject(skillInfoPanels[showSkillIndex + 1], skillInfoStrings[showSkillIndex+1], skillPanels[showSkillIndex + 1].transform.GetComponent<RectTransform>().anchoredPosition);

                trigger = skillPanels[showSkillIndex+1].GetComponentInChildren<EventTrigger>();
                EventTrigger.Entry entry2 = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };

                int skillInfoIndex2 = showSkillIndex+1;
                entry2.callback.AddListener(eventData => { ShowSkillInfo(skillInfoPanels[skillInfoIndex2]); });
                trigger.triggers.Add(entry2);
            }

			//if there is an uneven amount of rows, add one more, 
			if (skillAmount % 2 == 0)
			{
				return;
			}

			showSkillIndex += 2;
            skillPanels.Add(Instantiate(skillPanel, interventionScrollView.content.transform));
            InitiateTextObject(skillPanels[showSkillIndex], skillNumbers[showSkillIndex].ToString(),
				new Vector2(basePosition.x + 2 * skillPanels[2].GetComponent<RectTransform>().sizeDelta.x,
					basePosition.y + skillPanels[2].GetComponent<RectTransform>().sizeDelta.y * 3));

			Text infoText = skillPanels[showSkillIndex].GetComponentInChildren<Text>();
			infoText.text = skillNumbers[showSkillIndex].ToString();
			RetrieveAsset.RetrieveAssets();

			Image[] changemanagementImage = skillPanels[showSkillIndex].GetComponentsInChildren<Image>();
			changemanagementImage[1].sprite = RetrieveAsset.GetSpriteByName(spriteNames[showSkillIndex]);

            skillInfoPanels.Add(Instantiate(skillInfo, interventionScrollView.content.transform));
            InitiateTextObject(skillInfoPanels[showSkillIndex], skillInfoStrings[showSkillIndex], skillPanels[showSkillIndex].GetComponent<RectTransform>().anchoredPosition);

            trigger = skillPanels[showSkillIndex].GetComponentInChildren<EventTrigger>();
            EventTrigger.Entry entry3 = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };

            int skillInfoIndex3 = showSkillIndex;
            entry3.callback.AddListener(eventData => { ShowSkillInfo(skillInfoPanels[skillInfoIndex3]); });
            trigger.triggers.Add(entry3);
            for (int i = 0; i < skillInfoPanels.Count; i++)
            {
                skillInfoPanels[i].GetComponent<RectTransform>().SetAsLastSibling();
            }
        }

        public void ShowSkillInfo(GameObject skillInfoPanel)
        {
            skillInfoPanel.SetActive(true);
            skillInfoPanel.GetComponent<Selectable>().Select();
        }

		public void FinishLevel()
		{
			Game.GetGame().AddLevel();
			isFading = true;
		}

		public void ShowMenu()
		{
			game = Game.GetGame();

			bool isAllInformationFound = true;

			if (game.Information != null && !interventionscreen.activeSelf)
			{
				for (int i = 0; i < game.Information.InformationList.Length; i++)
				{
					if (!game.Information.InformationList[i].Found)
					{
					    zoomInterventionScreen.enabled = false;
						ShowWarning();
						isAllInformationFound = false;
						break;
					}
				}
			}

			AllInformationFound?.Invoke(isAllInformationFound);

			interventionscreen.SetActive(!interventionscreen.gameObject.activeSelf);
			blockingPanel.blocksRaycasts = !blockingPanel.blocksRaycasts;
			settingsButton.interactable = !settingsButton.IsInteractable();
			infoButton.interactable = !infoButton.IsInteractable();
			interventionButton.interactable = !interventionButton.IsInteractable();
		}

		private void InitiateTextObject(GameObject initiate, string text, Vector2 anchoredPosition)
		{
			//set the text of the textObject
			Text objectText = initiate.GetComponentInChildren<Text>();
			objectText.text = text;
			//set the position of the textObject
			RectTransform cTextPos = initiate.GetComponent<RectTransform>();
			cTextPos.anchoredPosition = anchoredPosition;
		}

		public void ShowWarning()
		{
            if (!isInterventionChosen)
            {
                int amountofStakeholders = game.Information.InformationList.Length;
                int stakeholdersFound = 0;

                for (int i = 0; i < amountofStakeholders; i++)
                {
                    if (game.Information.InformationList[i].Found)
                    {
                        stakeholdersFound++;
                    }
                }

                //calculate percentage of stakeholders found
                int percentageFound = stakeholdersFound * 100 / amountofStakeholders;
                Text warningText = interventionWarning.GetComponentInChildren<Text>();
                warningText.text = "Weet je zeker dat je een interventie wilt kiezen? Je hebt nog maar " + percentageFound +
                                   "% van de stakeholders gevonden.";

                interventionWarning.SetActive(true);
                warningBlockingPanel.blocksRaycasts = true;
            }
		}

  

    }
}
