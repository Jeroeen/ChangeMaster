using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class SceneLoader : MonoBehaviour
	{
		private bool isFading;

		[SerializeField] private Transition transition;
		[SerializeField] private string sceneToLoad;

		[SerializeField] private Button settingsButton;
		[SerializeField] private Button infoButton;
		[SerializeField] private Button interventionButton;

		void Start()
		{
			ChangeInteractability(false);
		}

		void Update()
		{
			if (!isFading)
			{
				return;
			}

			if (!transition.transform.gameObject.activeSelf)
			{
				transition.transform.gameObject.SetActive(true);
			}

			if (!transition.FadeOut())
			{
				return;
			}

			if (!string.IsNullOrWhiteSpace(sceneToLoad))
			{
				// Load specific scene
				SceneManager.LoadScene(sceneToLoad);
			}
			// Load bridge
			SceneManager.LoadScene(3);
		}

		public void StartFading()
		{
			isFading = true;
		}

		private void ChangeInteractability(bool boolean)
		{
			settingsButton.interactable = boolean;
			infoButton.interactable = boolean;
			interventionButton.interactable = boolean;
		}

		public void Close()
		{
			ChangeInteractability(true);
			gameObject.SetActive(false);
		}
	}
}
