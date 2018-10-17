using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class InitiateDialogue : MonoBehaviour
{
	private RetrieveJson _json;
	private DialogueItem dialogue;

	public SpriteRenderer Partner;
	public Text PartnerName;
	public Text DialogueText;

	public Button PrevButton;
	public Button NextButton;

	public void Initialize(string nameOfPartner, int level, int dialogueCount)
	{
		RetrieveAsset.RetrieveAssets();

		_json = new RetrieveJson();
		dialogue = _json.LoadJson(nameOfPartner, level, dialogueCount);

		NextButton.GetComponentInChildren<Text>().text = dialogue.NextButtonText;
		PrevButton.GetComponentInChildren<Text>().text = dialogue.PreviousButtonText;

		PartnerName.text = dialogue.Speaker;
		DialogueText.text = dialogue.DialogueLines[0];

		Partner.sprite = RetrieveAsset.GetSpriteByName(nameOfPartner);

		PrevButton.interactable = false;
	}

	public void NextLine()
	{
		// Final page of slide, so close dialogue screen
		if (dialogue.IsEndOfDialogue())
		{
			gameObject.SetActive(false);

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
		if (dialogue.IsBeginningOfDialogue())
		{
			PrevButton.interactable = false;
		}
	}
}
