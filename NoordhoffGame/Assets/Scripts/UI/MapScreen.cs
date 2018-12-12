using Assets.Scripts.CameraBehaviour;
using Assets.Scripts.Cutscene;
using Assets.Scripts.Dialogue;
using Assets.Scripts.GameSaveLoad;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class MapScreen : MonoBehaviour
	{

		[SerializeField] private RectTransform player = null;
		[SerializeField] private Image playerImage = null;
		[SerializeField] private Transition transition = null;
		[SerializeField] private GameObject mapScreen = null;
		[SerializeField] private GameObject warningScreen = null;
		[SerializeField] private Button interventionButton = null;
		[SerializeField] private Button infoButton = null;
		[SerializeField] private Button settingsButton = null;
		[SerializeField] private Text warningScreenText = null;
		[SerializeField] private ZoomingObject zoomMapScreen = null;

		private Game game;
		private bool isFading;
		private bool canTravelToBaseview = true;
		private bool canTravelToCinema = true;
		private bool canTravelToArcade = true;
		private bool canTravelToLevels = true;
		private int levelIndex;

		void Start()
		{
			SaveLoadGame.Load();
			game = Game.GetGame();
			playerImage.sprite = RetrieveAsset.GetSpriteByName(PlayerPrefs.GetString(GlobalVariablesHelper.CHARACTER_NAME_PLAYERPREFS));
			switch (SceneManager.GetActiveScene().name)
			{
				case "Baseview":
					canTravelToBaseview = false;
					player.anchoredPosition = new Vector2(-625.0f, 220.0f);
					break;
				case "Arcade":
					canTravelToArcade = false;
					player.anchoredPosition = new Vector2(140.0f, 220.0f);
					break;
				case "Cinema":
					canTravelToCinema = false;
					player.anchoredPosition = new Vector2(-625.0f, -280.0f);
					break;
				default:
					canTravelToLevels = false;
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

		public void ShowMap()
		{
			mapScreen.SetActive(!mapScreen.gameObject.activeSelf);
		    if (settingsButton != null)
		    {
		        settingsButton.interactable = !settingsButton.IsInteractable();
		    }
		    if (infoButton != null)
		    {
		        infoButton.interactable = !infoButton.IsInteractable();
            }
		    if (interventionButton != null)
		    {
		        interventionButton.interactable = !interventionButton.IsInteractable();
            }
		}

		public void SwitchScene()
		{
			isFading = true;
		}

		public void TravelBaseview()
		{
			if (canTravelToBaseview)
			{
				zoomMapScreen.enabled = false;
				levelIndex = GlobalVariablesHelper.BASEVIEW_SCENE_INDEX;
				warningScreenText.text = "Weet je zeker dat je naar de brug wilt reizen";
				warningScreen.SetActive(true);
			}
		}

		public void TravelArcade()
		{
			if (canTravelToArcade)
			{
				zoomMapScreen.enabled = false;
				SaveLoadGame.Load();
				levelIndex = GlobalVariablesHelper.ARCADE_SCENE_INDEX;
				warningScreenText.text = "Weet je zeker dat je naar de arcadehal wilt reizen";
				warningScreen.SetActive(true);
			}
		}

		public void TravelCinema()
		{
			if (canTravelToCinema)
			{
				zoomMapScreen.enabled = false;
				SaveLoadGame.Load();
				levelIndex = GlobalVariablesHelper.CINEMA_SCENE_INDEX;
				warningScreenText.text = "Weet je zeker dat je naar de bioscoop wilt reizen";
				warningScreen.SetActive(true);
			}
		}

		public void TravelLevel()
		{
			if (canTravelToLevels && game.CurrentLevelIndex == game.LastFinishedLevel + 1)
			{
				zoomMapScreen.enabled = false;
				levelIndex = game.LastFinishedLevel + 1;
				int currentLevelNr = levelIndex - GlobalVariablesHelper.BASEVIEW_SCENE_INDEX;
				warningScreenText.text = "Weet je zeker dat je naar level " + currentLevelNr + " wilt reizen";
				warningScreen.SetActive(true);
				SaveLoadGame.Save();
			}
		}

	}
}
