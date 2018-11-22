using Assets.Scripts.Dialogue.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogue
{
	public class OpenPopUp : MonoBehaviour
	{
		[SerializeField] private GameObject dialogue;
		[SerializeField] private GameObject objectInfo;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button infoButton;
	    [SerializeField] private Button interventionButton;

        public static bool IsActive { get; set; }
	

		public void StartDialogue(CharacterModel characterModel)
		{
			DisableInteractables();
			DialogueHandler component = dialogue.GetComponent<DialogueHandler>();
			component.Initialize(characterModel);
			dialogue.SetActive(true);
		}

		public void StartObjectInfo(ObjectModel objectModel)
		{
			DisableInteractables();
            ObjectInfoHandler component = objectInfo.GetComponent<ObjectInfoHandler>();
			component.Initialize(objectModel);
			objectInfo.SetActive(true);
		}

		private void DisableInteractables()
		{
			settingsButton.interactable = false;
			infoButton.interactable = false;
		    interventionButton.interactable = false;

		}
	}
}