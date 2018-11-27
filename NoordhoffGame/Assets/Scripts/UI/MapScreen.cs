using Assets.Scripts.Cutscene;
using Assets.Scripts.Dialogue;
using Assets.Scripts.GameSaveLoad;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{

    [SerializeField] private RectTransform player;
    [SerializeField] private Image playerImage;
    [SerializeField] private Transition transition;
    [SerializeField] private GameObject mapScreen;
    [SerializeField] private GameObject warningScreen;
    [SerializeField] private Button interventionButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Text warningScreenText;

    private Game game;
    private bool isFading;
    private bool baseview = true;
    private bool cinema = true;
    private bool arcade = true;
    private bool levels = true;
    private int levelIndex;
    private string warningMessage;

    void Start()
    {
        SaveLoadGame.Load();
        game = Game.GetGame();
        playerImage.sprite = RetrieveAsset.GetSpriteByName(PlayerPrefs.GetString(GlobalVariablesHelper.CHARACTER_NAME_PLAYERPREFS));
        switch (SceneManager.GetActiveScene().name)
        {
            case "Baseview":
                baseview = false;
                Debug.Log("Baseview");

                player.anchoredPosition = new Vector2(-625.0f, 220.0f);
                break;
            case "Arcade":
                arcade = false;
                Debug.Log("arcade");

                player.anchoredPosition = new Vector2(140.0f, 220.0f);
                break;
            case "Cinema":
                cinema = false;
                Debug.Log("cinema");
                player.anchoredPosition = new Vector2(-625.0f, -280.0f);
                break;
            default:
                levels = false;
                Debug.Log("levels");

                player.anchoredPosition = new Vector2(140.0f, -280.0f);
                break;
        }

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
                SceneManager.LoadScene(levelIndex);
            }
        }
    }
    
    public void showMap()
    {

        mapScreen.SetActive(!mapScreen.gameObject.activeSelf);
        if (settingsButton != null)
        {
            settingsButton.interactable = !settingsButton.IsInteractable();
            infoButton.interactable = !infoButton.IsInteractable();
            interventionButton.interactable = !interventionButton.IsInteractable();
        }

    }

    public void SwitchScene()
    {
        isFading = true;
    }

    public void TravelBaseview()
    {
        if (baseview)
        {
            levelIndex = GlobalVariablesHelper.BASEVIEW_SCENE_INDEX;
            warningMessage = "Weet je zeker dat je naar de brug wilt reizen";
            warningScreenText.text = warningMessage;
            warningScreen.SetActive(true);
        }
    }
    public void TravelArcade()
    {
        if (arcade)
        {
            SaveLoadGame.Load();
            levelIndex = GlobalVariablesHelper.ARCADE_SCENE_INDEX;
            warningMessage = "Weet je zeker dat je naar de arcadehal wilt reizen";
            warningScreenText.text = warningMessage;
            warningScreen.SetActive(true);
        }
    }
    public void TravelCinema()
    {
        if (cinema)
        {
            SaveLoadGame.Load();
            levelIndex = GlobalVariablesHelper.CINEMA_SCENE_INDEX;
            warningMessage = "Weet je zeker dat je naar de bioscoop wilt reizen";
            warningScreenText.text = warningMessage;
            warningScreen.SetActive(true);
        }
    }
    public void TravelLevel()
    {
        if (levels && game.CurrentLevelIndex == game.LastFinishedLevel +1)
        {
            levelIndex = game.LastFinishedLevel + 1;
            Debug.Log(levelIndex);
            int currentLevelNr = levelIndex - GlobalVariablesHelper.BASEVIEW_SCENE_INDEX;
            warningMessage = "Weet je zeker dat je naar level " + currentLevelNr + " wilt reizen";
            warningScreenText.text = warningMessage;
            warningScreen.SetActive(true);
            SaveLoadGame.Save();
        }
    }

}
