using System.Collections.Generic;
using Assets.Scripts.Dialogue;
using Assets.Scripts.GameSaveLoad;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts.UI
{
    public class Infoscreen : MonoBehaviour
    {
        [SerializeField] private Button interventionButton = null;
        [SerializeField] private Button infoButton = null;
        [SerializeField] private Button settingsButton = null;
        [SerializeField] private Button stakeholdersButton = null;
        [SerializeField] private GameObject infoScreen = null;
        [SerializeField] private GameObject stakeholderPanel = null;
        [SerializeField] private GameObject theoryPanel = null;
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
        private List<GameObject> theoryPanels = new List<GameObject>();
        private InfoList information;
        private TheoryList theory;
        private Player player;
        private readonly List<Sprite> imagesStakeholder = new List<Sprite>();
        private Vector2 position;
        private ScrollRect infoScrollview;
        private RetrieveJson json;

        private RectTransform theoryRect;
        private float panelWidth;
        private float panelHeight;
        private ScrollRect theoryScrollView;

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
            ShowTheory();
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

            // Choose the right scrollview for stakeholderpanel
            ScrollRect[] scrollViews;
            scrollViews = infoScreen.GetComponentsInChildren<ScrollRect>();
            infoScrollview = scrollViews[1];

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

                foreach (Info info in information.InformationList)
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
                    infoScrollviewRect.sizeDelta = new Vector2(infoScrollviewRect.sizeDelta.x, infoScrollviewRect.sizeDelta.y + (panelSizeY));
                    //when infoScrollviewRect is made bigger, we have to compensate the position, 
                    //so we make it 1/2 the size of panelSizeY to the bottom, so it's correct now
                    infoScrollviewRect.anchoredPosition = new Vector2(infoScrollviewRect.anchoredPosition.x,
                                                                      infoScrollviewRect.anchoredPosition.y - ((panelSizeY / 2)));
                }

                position = new Vector2(position.x, position.y - (panelSizeY));
            }
        }

        public void ShowTheory()
        {
            json = new RetrieveJson();
            if (theory == null)
            {
                theory = json.LoadJsonTheory(SceneManager.GetActiveScene().name);
            }

            RetrieveAsset.RetrieveAssets();

            theoryRect = theoryPanel.GetComponent<RectTransform>();
            panelWidth = theoryRect.rect.width;
            panelHeight = theoryRect.rect.height;

            // Choose the right scrollview for theoryPanel
            ScrollRect[] scrollViews;
            scrollViews = infoScreen.GetComponentsInChildren<ScrollRect>();
            theoryScrollView = scrollViews[0];
        }

        private void DestroyTheoryPanels()
        {
            foreach (GameObject g in theoryPanels)
            {
                Destroy(g);
            }
            theoryPanels.Clear();
        }

        public void ShowTheoryTexts()
        {
            DestroyTheoryPanels();

            float yMultiplier = 0;
            for (int i = 0; i < theory.TheoryListTexts.Length; i++)
            {
                // modulo 3, because 3 panels are in 1 row
                if (i != 0 && i % 3 == 0)
                {
                    yMultiplier++;
                }

                // Offset used between panels and the window and the panels to create a bit of space between them
                float offset = 50;
                theoryRect.anchoredPosition = new Vector2(offset + i % 3 * (panelWidth + offset), -(offset + yMultiplier * (panelHeight + offset)));

                theoryPanels.Add(Instantiate(theoryPanel, theoryScrollView.content.transform));
                
                Text panelText = theoryPanels[i].GetComponentInChildren<Text>();
                panelText.text = theory.TheoryListTexts[i].Text;
            }
        }

        public void ShowTheoryImages()
        {
            DestroyTheoryPanels();

            float yMultiplier = 0;
            for (int i = 0; i < theory.TheoryListImages.Length; i++)
            {
                // modulo 3, because 3 panels are in 1 row
                if (i != 0 && i % 3 == 0)
                {
                    yMultiplier++;
                }

                // Offset used between panels and the window and the panels to create a bit of space between them
                float offset = 50;
                theoryRect.anchoredPosition = new Vector2(offset + i % 3 * (panelWidth + offset), -(offset + yMultiplier * (panelHeight + offset)));

                theoryPanels.Add(Instantiate(theoryPanel, theoryScrollView.content.transform));

                Image[] panelImage = theoryPanels[i].GetComponentsInChildren<Image>();
                panelImage[1].sprite = RetrieveAsset.GetSpriteByName(theory.TheoryListImages[i].Image);
                panelImage[1].enabled = true;
            }
        }

        public void ShowTheoryVideos()
        {
            DestroyTheoryPanels();

            float yMultiplier = 0;
            for (int i = 0; i < theory.TheoryListVideos.Length; i++)
            {
                // modulo 3, because 3 panels are in 1 row
                if (i != 0 && i % 3 == 0)
                {
                    yMultiplier++;
                }

                // Offset used between panels and the window and the panels to create a bit of space between them
                float offset = 50;
                theoryRect.anchoredPosition = new Vector2(offset + i % 3 * (panelWidth + offset), -(offset + yMultiplier * (panelHeight + offset)));

                theoryPanels.Add(Instantiate(theoryPanel, theoryScrollView.content.transform));

                Image[] panelThumbnail = theoryPanels[i].GetComponentsInChildren<Image>();
                panelThumbnail[1].sprite = RetrieveAsset.GetSpriteByName(theory.TheoryListVideos[i].Thumbnail);
                panelThumbnail[1].enabled = true;
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
