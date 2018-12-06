using Assets.Scripts.Cutscene;
using Assets.Scripts.GameSaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.World_Map
{
	public class DetermineDestination : MonoBehaviour
	{
		private bool isFading;
		private GameObject destinationObject;

		[SerializeField] private Text textObject;
		[SerializeField] private GameObject confirmDialogue;
		[SerializeField] private GameObject uiElements;
		[SerializeField] private Transition transition;
	    [SerializeField] private ZoomingObject zoomWorldMap;

		public void Activate()
		{
			if (!Game.GetGame().InLevel)
			{
				gameObject.SetActive(true);
			}
		}

		public void DestinationClick(GameObject obj)
		{
		    zoomWorldMap.enabled = false;
            destinationObject = obj;
			textObject.text = destinationObject.name;
			confirmDialogue.SetActive(true);
		}

		public void Confirm()
		{
			isFading = true;
		}

		void Update()
		{
			if (!isFading)
			{
				return;
			}

			if (!transition.transform.gameObject.activeSelf)
			{
				uiElements.SetActive(false);
				transition.transform.gameObject.SetActive(true);
			}
			else if (transition.FadeOut())
			{
				var game = Game.GetGame();
				game.CurrentDestination = destinationObject.name;
				game.InLevel = true;
				SaveLoadGame.Save();

				// Load next level
				SceneManager.LoadScene(game.CurrentLevelIndex);
			}
		}
	}
}
