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

	public void Initialize(string nameOfPartner, int level, int dialogueCount)
	{
		RetrieveAsset.RetrieveAssets();

		_json = new RetrieveJson();
		_dialogue = _json.LoadJson(nameOfPartner, level, dialogueCount);

		NextButton.GetComponentInChildren<Text>().text = _dialogue.nextButtonText;
		PrevButton.GetComponentInChildren<Text>().text = _dialogue.previousButtonText;

		PartnerName.text = _dialogue.speaker;
		DialogueText.text = _dialogue.lines[0];

		Partner.sprite = RetrieveAsset.GetSpriteByName(nameOfPartner);

		PrevButton.interactable = false;
	}

	public void NextLine()
	{
		// Final page of slide, so close dialogue screen
		if (_dialogue.IsEndOfDialogue())
		{
			gameObject.SetActive(false);
			OpenDialogue.IsActive = false;

			return;
		}

		DialogueText.text = _dialogue.NextLine();
		PrevButton.interactable = true;

		// Next page is final page of slide
		if (_dialogue.IsEndOfDialogue())
		{
			NextButton.GetComponentInChildren<Text>().text = _dialogue.confirmButtonText;
		}

	}

	public void PreviousLine()
	{
		DialogueText.text = _dialogue.PreviousLine();
		NextButton.interactable = true;

		NextButton.GetComponentInChildren<Text>().text = _dialogue.nextButtonText;
		if (_dialogue.IsBeginningOfDialogue())
		{
			PrevButton.interactable = false;
		}
	}
}
