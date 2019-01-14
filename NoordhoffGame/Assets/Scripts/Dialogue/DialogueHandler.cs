using Assets.Scripts.Dialogue.JsonItems;
using Assets.Scripts.Dialogue.Models;
using Assets.Scripts.Progress;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Info;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogue
{
	public class DialogueHandler : MonoBehaviour
	{
		private RetrieveJson json;
		private DialogueItem dialogue;
		private CharacterModel characterModel;

		[SerializeField] private Infoscreen infoscreen = null;
		[SerializeField] private SpriteRenderer partner = null;
		[SerializeField] private Text partnerName = null;
		[SerializeField] private Text dialogueText = null;

		[SerializeField] private Button prevButton = null;
		[SerializeField] private Button nextButton = null;
		[SerializeField] private Button closeButton = null;
		[SerializeField] private Button settingButton = null;
		[SerializeField] private Button infoButton = null;
		[SerializeField] private Button interventionButton = null;
		
		public delegate void DoneCallback(CharacterModel characterModel);
		
		public static event DoneCallback OnConversationDoneEvent;

		public void Initialize(CharacterModel model)
		{
		    SaveLoadGame.Load();

			characterModel = model;
			RetrieveAsset.RetrieveAssets();

			json = new RetrieveJson();

			if (Game.GetGame().DialogueRead.ContainsKey(model.NameOfPartner + model.Stage + model.DialogueCount))
			{
				while (Game.GetGame().DialogueRead.ContainsKey(model.NameOfPartner + model.Stage + model.DialogueCount) &&
					Game.GetGame().DialogueRead[model.NameOfPartner + model.Stage + model.DialogueCount] &&
					characterModel.DialogueCount > -1 && characterModel.DialogueCount < characterModel.AmountOfDialogues - 1)
				{
					characterModel.DialogueCount++;
				}
			}

			dialogue = json.LoadJsonDialogue(model.NameOfPartner, model.Stage, model.DialogueCount);

		    if (closeButton != null)
		    {
		        closeButton.interactable = dialogue.DialogueLines.Count > 1;
		    }

            nextButton.GetComponentInChildren<Text>().text = dialogue.NextButtonText;
			prevButton.GetComponentInChildren<Text>().text = dialogue.PreviousButtonText;

			partnerName.text = dialogue.NameOfSpeaker;
			dialogueText.text = dialogue.DialogueLines[0];

			partner.sprite = RetrieveAsset.GetSpriteByName(model.NameOfPartner);

			prevButton.interactable = false;
		}

		public void CloseDialogue()
		{
			gameObject.SetActive(false);
			OpenPopUp.IsActive = false;
			if (infoButton != null && settingButton != null)
			{
				infoButton.interactable = true;
				settingButton.interactable = true;
				interventionButton.interactable = true;
			}
			OnConversationDoneEvent?.Invoke(characterModel);
		}

		public void NextLine()
		{
			// Final page of slide, so close dialogue screen
			if (dialogue.IsEndOfDialogue())
			{
				Game.GetGame().DialogueRead[characterModel.NameOfPartner + characterModel.Stage + characterModel.DialogueCount] = true;
				SaveLoadGame.Save();

				if (infoscreen != null)
				{
					infoscreen.ShowStakeholder(characterModel.NameOfPartner);
					infoscreen.SaveInformation();
				}

				CloseDialogue();

				if (characterModel.AmountOfDialogues >= 0 && characterModel.DialogueCount < characterModel.AmountOfDialogues - 1)
				{
					characterModel.DialogueCount++;
				}

				return;
			}

			dialogueText.text = dialogue.NextLine();
			prevButton.interactable = true;

			// Next page is final page of slide
			if (!dialogue.IsEndOfDialogue())
			{
				return;
			}

			nextButton.GetComponentInChildren<Text>().text = dialogue.ConfirmButtonText;
		    if (closeButton != null)
		    {
		        closeButton.interactable = false;
		    }
		}

		public void PreviousLine()
		{
			dialogueText.text = dialogue.PreviousLine();
			closeButton.interactable = true;
			nextButton.interactable = true;

			nextButton.GetComponentInChildren<Text>().text = dialogue.NextButtonText;
			if (dialogue.IsBeginningOfDialogue())
			{
				prevButton.interactable = false;
			}
		}
	}
}
