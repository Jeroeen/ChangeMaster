using Assets.Scripts.Cutscene;
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
        switch (SceneManager.GetActiveScene().name)
        {
            case "Baseview":
                baseview = false;
                break;
            case "Arcade":
                arcade = false;
                break;
            case "cinema":
                cinema = false;
                break;
            default:
                levels = false;
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
            levelIndex = GlobalVariablesHelper.BRIDGE_SCENE_INDEX;
            warningMessage = "Weet je zeker dat je naar de brug wilt reizen";
            warningScreenText.text = warningMessage;
            warningScreen.SetActive(true);
        }
    }
    public void TravelArcade()
    {
        if (arcade)
        {
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
            levelIndex = GlobalVariablesHelper.CINEMA_SCENE_INDEX;
            warningMessage = "Weet je zeker dat je naar de bioscoop wilt reizen";
            warningScreenText.text = warningMessage;
            warningScreen.SetActive(true);
        }
    }
    public void TravelLevel()
    {
        if (levels)
        {
            SaveLoadGame.Load();
            game = Game.GetGame();
            Debug.Log(game.CurrentLevelNumber);
            levelIndex = game.CurrentLevelIndex;
            warningMessage = "Weet je zeker dat je naar level " + game.CurrentLevelNumber + " wilt reizen";
            warningScreenText.text = warningMessage;
            warningScreen.SetActive(true);
        }
    }

}
