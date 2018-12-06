using Assets.Scripts.Cutscene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Splashscreen
{
	public class LevelChooser : MonoBehaviour
	{
		[SerializeField] private Transition transition = null;

		private bool isFading;
		private string levelName;

		public void LoadScene()
		{
			if (!transition.transform.gameObject.activeSelf)
			{
				transition.gameObject.SetActive(true);
			}

			levelName = PlayerPrefs.GetString("LastLevel"); //this assumes you save a string in PlayerPrefs at some point that's the name of the scene with that level
			if (string.IsNullOrEmpty(levelName))
			{
				levelName = "Opening Cutscene"; //the default scene that should be loaded when you play for the first time
			}

			isFading = true;
		}

		void Update()
		{
			if (!isFading)
			{
				return;
			}

			if (transition.FadeOut())
			{
				SceneManager.LoadScene(levelName);
			}
		}
	}
}