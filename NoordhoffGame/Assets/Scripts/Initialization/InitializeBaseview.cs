using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Initialization
{
	public class InitializeBaseview : MonoBehaviour
	{
		[SerializeField] private DialogueHandler _dialogueHandler;
		[SerializeField] private GameObject dialogue;

		[SerializeField] private Transition transition;

		[SerializeField] private Button infoButton;
		[SerializeField] private Button settingsButton;

		private bool hasOpenedDialogue;
    
		void Update()
		{
			if (dialogue.activeSelf || !hasOpenedDialogue)
			{
				return;
			}

			if (!transition.transform.gameObject.activeSelf)
			{
				transition.transform.gameObject.SetActive(true);
			}
			else
			{
				if (transition.FadeOut())
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				}
			}
		}

		public void ShowDialogCaptain(CharacterModel characterModel)
		{
		    if (infoButton != null && settingsButton != null)
		    {
		        infoButton.interactable = !infoButton.IsInteractable();
		        settingsButton.interactable = !settingsButton.IsInteractable();
		    }

		    _dialogueHandler.Initialize(characterModel);
			dialogue.SetActive(true);
			hasOpenedDialogue = true;
		}
	}
}
