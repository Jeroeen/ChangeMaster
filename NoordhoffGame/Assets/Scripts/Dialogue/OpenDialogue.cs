using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenDialogue : MonoBehaviour
{
	private InitiateDialogue _initiateDialogue;
	public static bool IsActive { get; set; }
	public GameObject Dialogue;
	public MouseChecker Checker;
    public Button SettingsButton;
    public Button InfoButton;


	void Update()
	{
		if (EventSystem.current.IsPointerOverGameObject() || Checker.IsPointerOverUI)
		{
			return;
		}

		if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			&& Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			CheckTouch(Input.GetTouch(0).position);
		}
		else if ((Application.platform == RuntimePlatform.WindowsEditor ||
				  Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer)
				 && Input.GetMouseButtonDown(0))
		{
			CheckTouch(Input.mousePosition);
		}
	}

	private void CheckTouch(Vector3 position)
	{
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(position);
		Vector2 touchPosition = new Vector2(worldPoint.x, worldPoint.y);
		Collider2D hit = Physics2D.OverlapPoint(touchPosition);

		if (!hit || hit.gameObject.tag != "Character")
		{
			return;
		}

		IsActive = true;
        SettingsButton.interactable = false;
        InfoButton.interactable = false;
        InitiateDialogue dialogue = Dialogue.GetComponent<InitiateDialogue>();

		CharModel characterModel = hit.transform.gameObject.GetComponentInChildren<CharModel>();
		dialogue.Initialize(hit.transform.name, characterModel.Level, characterModel.DialogueCount);

		if (characterModel.DialogueCount >= 0 && characterModel.DialogueCount < characterModel.AmountOfDialogues - 1)
		{
			characterModel.DialogueCount++;
		}

		Dialogue.SetActive(true);
	}
}