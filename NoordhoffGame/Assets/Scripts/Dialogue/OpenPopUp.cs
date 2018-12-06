using Assets.Scripts.Dialogue.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogue
{
	public class OpenPopUp : MonoBehaviour
	{
		[SerializeField] private GameObject dialogue = null;
		[SerializeField] private GameObject objectInfo = null;
		[SerializeField] private Button settingsButton = null;
		[SerializeField] private Button infoButton = null;
	    [SerializeField] private Button interventionButton = null;

        public static bool IsActive { get; set; }

		private void DisableInteractables()
		{
			settingsButton.interactable = false;
			infoButton.interactable = false;
			interventionButton.interactable = false;
		}

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
	}
}