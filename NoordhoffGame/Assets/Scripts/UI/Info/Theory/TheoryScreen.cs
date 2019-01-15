using System.Collections.Generic;
using Assets.Scripts.CameraBehaviour;
using Assets.Scripts.Dialogue;
using Assets.Scripts.Json;
using Assets.Scripts.Multimedia;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Info.Theory
{
    public class TheoryScreen : MonoBehaviour
    {
        private RetrieveJson json;

        private TheoryList theory;
        private List<GameObject> theoryPanels = new List<GameObject>();

        private RectTransform theoryRect;
        private float panelWidth;
        private float panelHeight;
    
        private int selectedMovieId;

        [SerializeField] private ScrollRect theoryScrollView = null;
        [SerializeField] private GameObject theoryPanel = null;
        [SerializeField] private GameObject theoryTextPanel = null;
        [Tooltip("Should be ObjectInfo prefab")]
        [SerializeField] private GameObject theoryImagePanel = null;
        [Tooltip("Should be ConfirmCinema")]
        [SerializeField] private GameObject theoryVideoConfirmPanel = null;
        [Tooltip("Should be InfoScreen")]
        [SerializeField] private ZoomingObject zoomInfoScreen = null;
        [SerializeField] private Image panelImage = null;

        // Start is called before the first frame update
        void Start()
        {
            ShowTheory();
            ShowTheoryVideos();
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
        }

        /// <summary>
        /// Shows all theory that is images of this level
        /// </summary>
        public void ShowTheoryImages()
        {
            ShowTheoryObject(theory.TheoryListImages);
        }

        /// <summary>
        /// Shows all theory that is text of this level
        /// </summary>
        public void ShowTheoryTexts()
        {
            ShowTheoryObject(theory.TheoryListTexts);
        }

        /// <summary>
        /// Shows all theory that is videos of this level
        /// </summary>
        public void ShowTheoryVideos()
        {
            ShowTheoryObject(theory.TheoryListVideos);
        }

        private void ShowTheoryObject<T>(T[] listObject)
        {
            foreach (GameObject g in theoryPanels)
            {
                Destroy(g);
            }
            theoryPanels.Clear();

            float yMultiplier = 0;
            for (int i = 0; i < listObject.Length; i++)
            {
                // modulo 3, because 3 panels are in 1 row
                if (i != 0 && i % 3 == 0)
                {
                    yMultiplier++;
                }

                // Offset used between a panel and another panel and the offset for a panel and the window that contains the panels to create a bit of space between them
                float offset = 30;
                theoryRect.anchoredPosition = new Vector2(offset + i % 3 * (panelWidth + offset), -(offset + yMultiplier * (panelHeight + offset)));

                theoryPanels.Add(Instantiate(theoryPanel, theoryScrollView.content.transform));

                // Needed for the delegate (onClick events)
                int id = i;
                Button showBigScreenButton = theoryPanels[i].GetComponentInChildren<Button>();

                // Set the text when you want to show text and images when you select images or videos (for the thumbnail images)
                if (listObject is TheoryListImages[] || listObject is TheoryListVideos[])
                {
                    Image[] panelImage = theoryPanels[i].GetComponentsInChildren<Image>();
                    // index is 1 because otherwise it takes the image of the object itself
                    if (listObject is TheoryListImages[])
                    {
                        panelImage[1].sprite = RetrieveAsset.GetSpriteByName(theory.TheoryListImages[i].Image);

                        showBigScreenButton.onClick.AddListener(delegate { ShowTheoryImage(panelImage[1].sprite); });
                    }
                    else
                    {
                        panelImage[1].sprite = RetrieveAsset.GetSpriteByName(theory.TheoryListVideos[i].Thumbnail);

                        showBigScreenButton.onClick.AddListener(delegate { ShowTheoryVideoConfirmScreen(id); });
                    }
                    panelImage[1].enabled = true;
                }
                else if (listObject is TheoryListTexts[])
                {
                    Text panelText = theoryPanels[i].GetComponentInChildren<Text>();
                    panelText.text = theory.TheoryListTexts[i].Title;
                
                    showBigScreenButton.onClick.AddListener(delegate { ShowTheoryText(theory.TheoryListTexts[id].Text); });
                }
            }
        }

        private void ShowTheoryText(string text)
        {
            zoomInfoScreen.enabled = false;
            theoryTextPanel.GetComponentInChildren<Text>().text = text;
            theoryTextPanel.SetActive(true);
        }

        /// <summary>
        /// Sets ObjectInfo sprite
        /// </summary>
        /// <param name="sprite"></param>
        private void ShowTheoryImage(Sprite sprite)
        {
            zoomInfoScreen.enabled = false;
            panelImage.sprite = sprite;
            theoryImagePanel.SetActive(true);
        }

        private void ShowTheoryVideoConfirmScreen(int id)
        {
            zoomInfoScreen.enabled = false;
            selectedMovieId = id;
            theoryVideoConfirmPanel.SetActive(true);
        }

        public void GoToCinemaAndPlayVideo()
        {
            VideoHandler.MovieToLoad = theory.TheoryListVideos[selectedMovieId].Url;
            SceneManager.LoadScene(GlobalVariablesHelper.CINEMA_SCENE_INDEX);
        }
    }
}
