using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class InitiateDialogue : MonoBehaviour
{
	private RetrieveJson _json;
    private DialogueItem _dialogue;

	public SpriteRenderer Partner;
	public Text PartnerName;
	public Text DialogueText;

	public Button PrevButton;
	public Button NextButton;
    public Button SettingButton;
    public Button InfoButton;

    public void Initialize(string nameOfPartner, string stage, int dialogueCount)
	{
		RetrieveAsset.RetrieveAssets();

		_json = new RetrieveJson();
		_dialogue = _json.LoadJson(nameOfPartner, stage, dialogueCount);

		NextButton.GetComponentInChildren<Text>().text = _dialogue.NextButtonText;
		PrevButton.GetComponentInChildren<Text>().text = _dialogue.PreviousButtonText;

		PartnerName.text = _dialogue.NameOfSpeaker;
		DialogueText.text = _dialogue.DialogueLines[0];

		Partner.sprite = RetrieveAsset.GetSpriteByName(nameOfPartner);

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
		if (_dialogue.IsEndOfDialogue())
		{
			CloseDialogue();

		    return;
		}

		DialogueText.text = _dialogue.NextLine();
		PrevButton.interactable = true;

		// Next page is final page of slide
		if (_dialogue.IsEndOfDialogue())
		{

			NextButton.GetComponentInChildren<Text>().text = _dialogue.ConfirmButtonText;
		}

	}

	public void PreviousLine()
	{
		DialogueText.text = _dialogue.PreviousLine();
		NextButton.interactable = true;

		NextButton.GetComponentInChildren<Text>().text = _dialogue.NextButtonText;
		if (_dialogue.IsBeginningOfDialogue())
		{
			PrevButton.interactable = false;
		}
	}
}
