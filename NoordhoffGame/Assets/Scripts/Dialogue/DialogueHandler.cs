using Assets.Scripts.Dialogue.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogue
{
	public class DialogueHandler : MonoBehaviour
	{
		private RetrieveJson json;
		private DialogueItem dialogue;
		private CharacterModel characterModel;

		public SpriteRenderer Partner;
		public Text PartnerName;
		public Text DialogueText;

		public Button PrevButton;
		public Button NextButton;
		public Button SettingButton;
		public Button InfoButton;

		public void Initialize(CharacterModel model)
		{
			characterModel = model;
			RetrieveAsset.RetrieveAssets();

			json = new RetrieveJson();
			dialogue = json.LoadJsonDialogue(model.NameOfPartner, model.Stage, model.DialogueCount);

			NextButton.GetComponentInChildren<Text>().text = dialogue.NextButtonText;
			PrevButton.GetComponentInChildren<Text>().text = dialogue.PreviousButtonText;

			PartnerName.text = dialogue.NameOfSpeaker;
			DialogueText.text = dialogue.DialogueLines[0];

			Partner.sprite = RetrieveAsset.GetSpriteByName(model.NameOfPartner);

			PrevButton.interactable = false;
		}

		public void CloseDialogue()
		{
			gameObject.SetActive(false);
			OpenPopUp.IsActive = false;
			if (InfoButton != null && SettingButton != null)
			{
				InfoButton.interactable = true;
				SettingButton.interactable = true;
			}
		}

		public void NextLine()
		{
			// Final page of slide, so close dialogue screen
			if (dialogue.IsEndOfDialogue())
			{
				CloseDialogue();
				if (characterModel.DialogueCount > -1)
				{
					characterModel.DialogueCount++;
				}
			
				return;
			}

			DialogueText.text = dialogue.NextLine();
			PrevButton.interactable = true;

			// Next page is final page of slide
			if (dialogue.IsEndOfDialogue())
			{
				NextButton.GetComponentInChildren<Text>().text = dialogue.ConfirmButtonText;
			}
		}

		public void PreviousLine()
		{
			DialogueText.text = dialogue.PreviousLine();
			NextButton.interactable = true;

			NextButton.GetComponentInChildren<Text>().text = dialogue.NextButtonText;
			if (dialogue.IsBeginningOfDialogue())
			{
				PrevButton.interactable = false;
			}
		}
	}
}
