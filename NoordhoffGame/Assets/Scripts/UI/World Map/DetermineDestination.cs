using Assets.Scripts.CameraBehaviour;
using Assets.Scripts.Cutscene;
using Assets.Scripts.Progress;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.World_Map
{
	public class DetermineDestination : MonoBehaviour
	{
		private bool isFading;
		private GameObject destinationObject;

		[SerializeField] private Text textObject = null;
		[SerializeField] private GameObject confirmDialogue = null;
		[SerializeField] private GameObject uiElements = null;
		[SerializeField] private Transition transition = null;
	    [SerializeField] private ZoomingObject zoomWorldMap = null;

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
