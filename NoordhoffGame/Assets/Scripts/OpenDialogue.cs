using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenDialogue : MonoBehaviour
{
	private InitiateDialogue _ini;
	public GameObject Dialogue;
	
	[SerializeField] public int Level;
	[SerializeField] public int DialogueCount;
	[SerializeField] public int AmountOfDialogues;

	void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

		var dia = Dialogue.GetComponent<InitiateDialogue>();
		dia.Initialize(gameObject.name, Level, DialogueCount);
		if (DialogueCount >= 0 && DialogueCount < AmountOfDialogues - 1)
		{
			DialogueCount++;
		}
		Dialogue.SetActive(true);
	}
}