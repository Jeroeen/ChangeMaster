using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class DetermineDestination : MonoBehaviour
	{
		private bool isFading;

		[SerializeField] private Text textObject;
		[SerializeField] private GameObject confirmDialogue;
		[SerializeField] private GameObject uiElements;
		[SerializeField] private Transition transition;

		public void DestinationClick(GameObject obj)
		{
			textObject.text = obj.name;
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
				// This will load the next scene once saving and loading the game is implemented
				// Must also save the next destination so it can be used in the future.
				SceneManager.LoadScene(4);
			}

		}
	}
}
