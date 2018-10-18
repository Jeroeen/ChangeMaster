using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class OpenDialogue : MonoBehaviour
{
	private InitiateDialogue _ini;
	public static bool IsActive { get; set; }
	public GameObject Dialogue;
	

	void FixedUpdate()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touchCount == 1)
			{
				if (Input.GetTouch(0).phase == TouchPhase.Began)
				{
					CheckTouch(Input.GetTouch(0).position);
				}
			}
		}
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		{
			if (Input.GetMouseButtonDown(0))
			{
				CheckTouch(Input.mousePosition);
			}
		}
	}

	private void CheckTouch(Vector3 pos)
	{
		Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		Collider2D hit = Physics2D.OverlapPoint(touchPos);

		if (hit && hit.gameObject.tag == "GameController")
		{
			IsActive = true;
			
			InitiateDialogue dia = Dialogue.GetComponent<InitiateDialogue>();

			CharModel characterModel = hit.transform.gameObject.GetComponentInChildren<CharModel>();
			dia.Initialize(hit.transform.name, characterModel.Level, characterModel.DialogueCount);

			if (characterModel.DialogueCount >= 0 && characterModel.DialogueCount < characterModel.AmountOfDialogues - 1)
			{
				characterModel.DialogueCount++;
			}
			
			Dialogue.SetActive(true);
		}
	}
}