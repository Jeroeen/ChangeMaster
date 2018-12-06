using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Models;
using UnityEngine;

namespace Assets.Scripts.Initialization
{
	public class InitializeBaseview : MonoBehaviour
	{
		[SerializeField] private DialogueHandler dialogueHandler = null;
		[SerializeField] private GameObject dialogue = null;

		public void ShowDialogCaptain(CharacterModel characterModel)
		{
		    dialogueHandler.Initialize(characterModel);
			dialogue.SetActive(true);
		}
	}
}
