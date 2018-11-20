using System.Collections.Generic;
using Assets.Scripts.Dialogue;
using Assets.Scripts.GameSaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class Infoscreen : MonoBehaviour
	{
		[SerializeField] private Button interventionButton;
		[SerializeField] private Button infoButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button stakeholdersButton;
		[SerializeField] private GameObject infoScreen;
		[SerializeField] private GameObject stakeholderPanel;
		[SerializeField] private CanvasGroup blockingPanel;
		[SerializeField] private Text function;
		[SerializeField] private Text playerName;
		[SerializeField] private Text analytic;
		[SerializeField] private Text decisive;
		[SerializeField] private Text creative;
		[SerializeField] private Text empatic;
		[SerializeField] private Text enthusiasm;
		[SerializeField] private Text convincing;
		[SerializeField] private Text changeKnowledge;

		private List<GameObject> panels = new List<GameObject>();
		private InfoList information;
		private Player player;
		private readonly List<Sprite> images = new List<Sprite>();
		private Vector2 position;
		private ScrollRect infoScrollview;
		private RetrieveJson json;

		// Start is called before the first frame update
		void Start()
		{
			json = new RetrieveJson();
			SaveLoadGame.Load();
			if (Game.GetGame().Information == null)
			{
				Game.GetGame().Information = json.LoadJsonInformation(SceneManager.GetActiveScene().name);
                SaveLoadGame.Save();
			}
			information = Game.GetGame().Information;

			ShowStakeholders();
			stakeholdersButton.interactable = false;
			infoScreen.SetActive(false);
		}

		//a function that will fill the stakeholders menu with the stakeholders and their opinion on the current problem
		private void ShowStakeholders()
		{
			position = new Vector2(0.0f, -5.0f);
        
			json = new RetrieveJson();
			if (information == null)
			{
				information = json.LoadJsonInformation(SceneManager.GetActiveScene().name);
			}
        
			RetrieveAsset.RetrieveAssets();
			images.Clear();
			for (int i = 0; i < information.InformationList.Length; i++)
			{
				if (information.InformationList[i].Found)
				{
					images.Add(RetrieveAsset.GetSpriteByName(information.InformationList[i].Image));
				}
			}

			RectTransform panelRect = stakeholderPanel.GetComponent<RectTransform>();
			float panelSizeY = panelRect.sizeDelta.y;

			infoScrollview = infoScreen.GetComponentInChildren<ScrollRect>();

			RectTransform scrollviewContent = infoScrollview.content.GetComponent<RectTransform>();
			scrollviewContent.anchoredPosition = new Vector2(0.0f, scrollviewContent.anchoredPosition.y);

			RectTransform infoScrollviewRect = infoScrollview.content.GetComponent<RectTransform>();
			foreach (GameObject g in panels)
			{
				Destroy(g);
			}

			panels.Clear();
			for (int i = 0; i < images.Count; i++)
			{

				panels.Add(Instantiate(stakeholderPanel, infoScrollview.content.transform));

				foreach (Info info in information.InformationList)
				{
					if (info.Image == images[i].name)
					{
						Text[] infoText = panels[i].GetComponentsInChildren<Text>();
						infoText[0].text = info.Text;
					}
				}
				Image[] infoImage = panels[i].GetComponentsInChildren<Image>();
				infoImage[1].sprite = images[i];

				RectTransform[] infoRectTransform = panels[i].GetComponents<RectTransform>();
				infoRectTransform[0].anchoredPosition = position;
				float elementLimit = scrollviewContent.sizeDelta.y / (panelSizeY + 15);

				if (i > elementLimit)
				{
					infoScrollviewRect.sizeDelta = new Vector2(infoScrollviewRect.sizeDelta.x, infoScrollviewRect.sizeDelta.y + (panelSizeY + 15));
					infoScrollviewRect.anchoredPosition = new Vector2(infoScrollviewRect.anchoredPosition.x, infoScrollviewRect.anchoredPosition.y - ((0.5f * panelSizeY) + 15));
				}

				position = new Vector2(position.x, position.y - (panelSizeY + 10.0f));
			}
		}

		//a function that will set a stakeholder for this problem to show up in the stakeholders menu, depending on the name
		public void ShowStakeholder(string Name)
		{
			json = new RetrieveJson();
			if (information == null)
			{
				information = json.LoadJsonInformation(SceneManager.GetActiveScene().name);
			}

        for (int i = 0; i < information.InformationList.Length; i++)
        {
            if (information.InformationList[i].Image == Name)
            {
                information.InformationList[i].Found = true;
            }
        }
        Game.GetGame().Information = information;
        ShowStakeholders();
    }

		//a function that will enable or disable the menu 
		public void EnableInfo()
		{
			infoScreen.SetActive(!infoScreen.activeSelf);
			blockingPanel.blocksRaycasts = !blockingPanel.blocksRaycasts;
			settingsButton.interactable = !settingsButton.IsInteractable();
			interventionButton.interactable = !interventionButton.IsInteractable();
			infoButton.interactable = !infoButton.IsInteractable();
			FillCharacterInfo();

		}

		//a function that will fill the character info menu
		public void FillCharacterInfo()
		{
			player = Player.GetPlayer();

			analytic.text = player.Analytic.ToString();
			enthusiasm.text = player.Enthousiasm.ToString();
			decisive.text = player.Decisive.ToString();
			empatic.text = player.Empathic.ToString();
			convincing.text = player.Convincing.ToString();
			creative.text = player.Creative.ToString();
			changeKnowledge.text = player.ChangeKnowledge.ToString();

			playerName.text = "Naam: " + player.Name;

			function.text = "Functie: " + player.GetPlayerTitle();
		}

		public void SaveInformation()
		{
			Game.GetGame().Information = information;
		}

		public void ClearInformation()
		{
			Game.GetGame().Information = null;
			SaveLoadGame.Save();
		}
	}
}
