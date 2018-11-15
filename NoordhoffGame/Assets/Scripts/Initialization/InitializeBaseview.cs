using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Initialization
{
	public class InitializeBaseview : MonoBehaviour
	{
		[SerializeField] private DialogueHandler dialogueHandler;
		[SerializeField] private GameObject dialogue;

		public void ShowDialogCaptain(CharacterModel characterModel)
		{
		    dialogueHandler.Initialize(characterModel);
			dialogue.SetActive(true);
		}
	}
}
