using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class InitiateDialogue : MonoBehaviour
{
	private RetrieveJson _json;
	private DialogueItem dialogue;

	private string _nameOfPartner;
	private int _numberOfConversation;

	public RectTransform Partner;
	public Text PartnerName;
	public Text DialogueText;

	public Button PrevButton;
	public Button NextButton;

	public void Initialize(string nameOfPartner, int numberOfConversation)
	{
		_nameOfPartner = nameOfPartner;
		_numberOfConversation = numberOfConversation;

		_json = new RetrieveJson();
		dialogue = _json.LoadJson(_nameOfPartner, _numberOfConversation);

		NextButton.GetComponentInChildren<Text>().text = dialogue.NextButtonText;
		PrevButton.GetComponentInChildren<Text>().text = dialogue.PreviousButtonText;

		PartnerName.text = dialogue.Speaker;
		DialogueText.text = dialogue.DialogueLines[0];

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
