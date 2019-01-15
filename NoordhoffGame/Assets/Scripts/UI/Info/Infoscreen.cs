using System.Collections.Generic;
using Assets.Scripts.Dialogue;
using Assets.Scripts.Json;
using Assets.Scripts.Json.JsonItems;
using Assets.Scripts.Progress;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts.UI.Info
{
    public class Infoscreen : MonoBehaviour
    {
        [SerializeField] private Button interventionButton = null;
        [SerializeField] private Button infoButton = null;
        [SerializeField] private Button settingsButton = null;
        [SerializeField] private Button stakeholdersButton = null;
        [SerializeField] private GameObject infoScreen = null;
        [SerializeField] private GameObject stakeholderPanel = null;
        [SerializeField] private CanvasGroup blockingPanel = null;
        [SerializeField] private Text function = null;
        [SerializeField] private Text playerName = null;
        [SerializeField] private Text analytic = null;
        [SerializeField] private Text ownership = null;
        [SerializeField] private Text facilitates = null;
        [SerializeField] private Text approach = null;
        [SerializeField] private Text communication = null;

        private bool isScheduleShown;
        private bool hasFocus;
        private List<GameObject> stakeholderPanels = new List<GameObject>();
        private InfoList information;
        
        private Player player;
        private readonly List<Sprite> imagesStakeholder = new List<Sprite>();
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

            // Choose the right scrollview for stakeholderpanel
            ScrollRect[] scrollViews;
            scrollViews = infoScreen.GetComponentsInChildren<ScrollRect>();
            infoScrollview = scrollViews[1];
            infoScrollview.content.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            ShowStakeholders();
            
            stakeholdersButton.interactable = false;
            infoScreen.SetActive(false);
        }

        //a function that will fill the stakeholders menu with the stakeholders and their opinion on the current problem
        private void ShowStakeholders()
        {
            position = new Vector2();

            json = new RetrieveJson();
            if (information == null)
            {
                information = json.LoadJsonInformation(SceneManager.GetActiveScene().name);
            }

            RetrieveAsset.RetrieveAssets();
            imagesStakeholder.Clear();
            for (int i = 0; i < information.InformationList.Length; i++)
            {
                if (information.InformationList[i].Found)
                {
                    imagesStakeholder.Add(RetrieveAsset.GetSpriteByName(information.InformationList[i].Image));
                }
            }

            RectTransform panelRect = stakeholderPanel.GetComponent<RectTransform>();
            float panelSizeY = panelRect.sizeDelta.y;

            

            RectTransform scrollviewContent = infoScrollview.content.GetComponent<RectTransform>();


            RectTransform infoScrollviewRect = infoScrollview.content.GetComponent<RectTransform>();
            foreach (GameObject g in stakeholderPanels)
            {
                Destroy(g);
            }

            stakeholderPanels.Clear();
            for (int i = 0; i < imagesStakeholder.Count; i++)
            {
                stakeholderPanels.Add(Instantiate(stakeholderPanel, infoScrollview.content.transform));

                foreach (Json.JsonItems.Info info in information.InformationList)
                {
                    if (info.Image == imagesStakeholder[i].name)
                    {
                        Text[] infoText = stakeholderPanels[i].GetComponentsInChildren<Text>();
                        infoText[0].text = info.Text;
                    }
                }
                Image[] infoImage = stakeholderPanels[i].GetComponentsInChildren<Image>();
                infoImage[1].sprite = imagesStakeholder[i];

                RectTransform[] infoRectTransform = stakeholderPanels[i].GetComponents<RectTransform>();
                infoRectTransform[0].anchoredPosition = position;
                float elementLimit = scrollviewContent.sizeDelta.y / (panelSizeY);

                if (i > elementLimit - 1)
                {
                    infoScrollviewRect.sizeDelta = new Vector2(infoScrollviewRect.sizeDelta.x, infoScrollviewRect.sizeDelta.y + panelSizeY);
                    //when infoScrollviewRect is made bigger, we have to compensate the position, 
                    //so we make it 1/2 the size of panelSizeY to the bottom, so it's correct now
                    infoScrollviewRect.anchoredPosition = new Vector2(infoScrollviewRect.anchoredPosition.x,
                                                                      infoScrollviewRect.anchoredPosition.y - (panelSizeY / 2));
                }

                position = new Vector2(position.x, position.y - (panelSizeY));
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
            SaveLoadGame.Save();
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
            approach.text = player.Approach.ToString();
            ownership.text = player.Ownership.ToString();
            facilitates.text = player.Facilitating.ToString();
            communication.text = player.Communication.ToString();

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
