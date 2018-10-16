using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDialogue : MonoBehaviour
{
	public GameObject Dialogue;
	private InitiateDialogue _ini;

	void OnMouseDown()
	{
		Dialogue.GetComponent<InitiateDialogue>().Initialize(gameObject.name, 0);
		Dialogue.SetActive(true);
	}
}