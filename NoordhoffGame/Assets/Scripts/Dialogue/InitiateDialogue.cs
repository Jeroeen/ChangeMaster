using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class InitiateDialogue : MonoBehaviour
{
	private RetrieveJson json;
    private DialogueItem dialogue;
	private CharModel charModel;

	public SpriteRenderer Partner;
	public Text PartnerName;
	public Text DialogueText;

	public Button PrevButton;
	public Button NextButton;
    public Button SettingButton;
    public Button InfoButton;

    public void Initialize(CharModel model)
    {
	    charModel = model;
		RetrieveAsset.RetrieveAssets();

		json = new RetrieveJson();
		dialogue = json.LoadJson(model.NameOfPartner, model.Stage, model.DialogueCount);

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
		OpenDialogue.IsActive = false;
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
			if (charModel.DialogueCount > -1)
			{
				charModel.DialogueCount++;
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
