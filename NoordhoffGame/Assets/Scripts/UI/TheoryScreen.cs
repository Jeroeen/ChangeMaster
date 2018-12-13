using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.CameraBehaviour;
using Assets.Scripts.Dialogue;
using Assets.Scripts.UI;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject theoryImagePanel = null;
    [SerializeField] private GameObject theoryVideoConfirmPanel = null;
    [SerializeField] private ZoomingObject zoomInfoScreen = null;
    [SerializeField] private Image panelImage = null;

    public delegate void Callback();
    public static event Callback OnTextChooseButtonClickEvent;
    public static event Callback OnImageChooseButtonClickEvent;
    public static event Callback OnVideoChooseButtonClickEvent;


    // Start is called before the first frame update
    void Start()
    {
        ShowTheory();
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

    public void ShowTheoryImages()
    {
        ShowTheoryObject(theory.TheoryListImages);
    }

    public void ShowTheoryTexts()
    {
        ShowTheoryObject(theory.TheoryListTexts);
    }

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

            // Offset used between panels and the window and the panels to create a bit of space between them
            float offset = 30;
            theoryRect.anchoredPosition = new Vector2(offset + i % 3 * (panelWidth + offset), -(offset + yMultiplier * (panelHeight + offset)));

            theoryPanels.Add(Instantiate(theoryPanel, theoryScrollView.content.transform));

            // Needed for the delegate (onClick events)
            int id = i;
            Button showBigscreenButton = theoryPanels[i].GetComponentInChildren<Button>();

            // Set the text when you want to show text and images when you select images or video's (for the thumbnail images)
            if (listObject is TheoryListImages[] || listObject is TheoryListVideos[])
            {
                Image[] panelImage = theoryPanels[i].GetComponentsInChildren<Image>();
                // index is 1 because otherwise it takes the image of the object itself
                if (listObject is TheoryListImages[])
                {
                    panelImage[1].sprite = RetrieveAsset.GetSpriteByName(theory.TheoryListImages[i].Image);

                    showBigscreenButton.onClick.AddListener(delegate { ShowTheoryImage(panelImage[1].sprite); });
                }
                else
                {
                    panelImage[1].sprite = RetrieveAsset.GetSpriteByName(theory.TheoryListVideos[i].Thumbnail);

                    showBigscreenButton.onClick.AddListener(delegate { ShowTheoryVideoConfirmScreen(id); });
                }
                panelImage[1].enabled = true;
            }
            else if (listObject is TheoryListTexts[])
            {
                Text panelText = theoryPanels[i].GetComponentInChildren<Text>();
                panelText.text = theory.TheoryListTexts[i].Title;
                
                showBigscreenButton.onClick.AddListener(delegate { ShowTheoryText(theory.TheoryListTexts[id].Text); });
            }
        }
    }

    private void ShowTheoryText(string text)
    {
        OnTextChooseButtonClickEvent?.Invoke();
        zoomInfoScreen.enabled = false;
        Text panelText = theoryTextPanel.GetComponentInChildren<Text>();
        panelText.text = text;
        theoryTextPanel.SetActive(true);
    }

    private void ShowTheoryImage(Sprite sprite)
    {
        OnImageChooseButtonClickEvent?.Invoke();
        zoomInfoScreen.enabled = false;
        panelImage.sprite = sprite;
        theoryImagePanel.SetActive(true);
    }

    private void ShowTheoryVideoConfirmScreen(int id)
    {
        OnVideoChooseButtonClickEvent?.Invoke();
        zoomInfoScreen.enabled = false;
        selectedMovieId = id;
        theoryVideoConfirmPanel.SetActive(true);
    }

    public void GoToCinemaAndPlayVideo()
    {
        VideoHandler.MovieToLoad = theory.TheoryListVideos[selectedMovieId].URL;
        SceneManager.LoadScene(GlobalVariablesHelper.CINEMA_SCENE_INDEX);
    }
}
