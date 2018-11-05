using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenDialogue : MonoBehaviour
{
	private InitiateDialogue initiateDialogue;
	public static bool IsActive { get; set; }
	public GameObject Dialogue;
    public Button SettingsButton;
    public Button InfoButton;

	public void StartDialogue(CharModel charModel)
	{
	    SettingsButton.interactable = false;
	    InfoButton.interactable = false;
        InitiateDialogue dialogue = Dialogue.GetComponent<InitiateDialogue>();
		dialogue.Initialize(charModel);
		Dialogue.SetActive(true);
	}
}