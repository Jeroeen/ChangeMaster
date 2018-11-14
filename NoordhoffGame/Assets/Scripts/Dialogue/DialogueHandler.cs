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

        [SerializeField] private Infoscreen infoscreen;
        [SerializeField] private SpriteRenderer partner;
		[SerializeField] private Text partnerName;
		[SerializeField] private Text dialogueText;

		[SerializeField] private Button prevButton;
		[SerializeField] private Button nextButton;
		[SerializeField] private Button settingButton;
		[SerializeField] private Button infoButton;
	    [SerializeField] private Button interventionButton;

        public void Initialize(CharacterModel model)
		{
            SaveLoadGame.Load();

            characterModel = model;
			RetrieveAsset.RetrieveAssets();

			json = new RetrieveJson();

            if (Game.GetGame().dialogueRead.ContainsKey(model.NameOfPartner + model.Stage + model.DialogueCount))
            {
                while(Game.GetGame().dialogueRead[model.NameOfPartner + model.Stage + model.DialogueCount] && 
                    (characterModel.DialogueCount > -1 && characterModel.DialogueCount < characterModel.AmountOfDialogues - 1))
                {
                    characterModel.DialogueCount++;
                }
            }

            dialogue = json.LoadJsonDialogue(model.NameOfPartner, model.Stage, model.DialogueCount);

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
		}

		public void NextLine()
		{
			// Final page of slide, so close dialogue screen
			if (dialogue.IsEndOfDialogue())
			{
                Game.GetGame().dialogueRead[characterModel.NameOfPartner + characterModel.Stage + characterModel.DialogueCount] = true;
                SaveLoadGame.Save();
                if (infoscreen != null)
                {
                    infoscreen.ShowStakeholder(characterModel.NameOfPartner);
                    infoscreen.SaveInformation();
                }
                CloseDialogue();
				if (characterModel.DialogueCount > -1 && characterModel.DialogueCount < characterModel.AmountOfDialogues - 1)
				{
					characterModel.DialogueCount++;
				}
			
				return;
			}

			dialogueText.text = dialogue.NextLine();
			prevButton.interactable = true;

			// Next page is final page of slide
			if (dialogue.IsEndOfDialogue())
			{
				nextButton.GetComponentInChildren<Text>().text = dialogue.ConfirmButtonText;
			}
		}

		public void PreviousLine()
		{
			dialogueText.text = dialogue.PreviousLine();
			nextButton.interactable = true;

			nextButton.GetComponentInChildren<Text>().text = dialogue.NextButtonText;
			if (dialogue.IsBeginningOfDialogue())
			{
				prevButton.interactable = false;
			}
		}
	}
}
