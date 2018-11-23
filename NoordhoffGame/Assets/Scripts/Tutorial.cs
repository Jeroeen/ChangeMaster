using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Models;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	[SerializeField] private GameObject settingsButton;
	[SerializeField] private GameObject infoButton;
	[SerializeField] private GameObject interventionScreenButton;

	[SerializeField] private GameObject coinCount;
	[SerializeField] private GameObject shaderPlane;
	[SerializeField] private GameObject camBTarget;

	[SerializeField] private GameObject kapitein;
	[SerializeField] private GameObject worldMap;
	[SerializeField] private GameObject roerGanger;
	[SerializeField] private GameObject wegwijsPiet;

	void Start()
	{
		settingsButton.SetActive(false);
		infoButton.SetActive(false);
		interventionScreenButton.SetActive(false);
		coinCount.SetActive(false);

		worldMap.SetActive(false);
		roerGanger.SetActive(false);
		wegwijsPiet.SetActive(false);

		DialogueHandler.OnConversationDoneEvent += FinishedCaptainDialogue();
	}

	public void Captain()
	{
		shaderPlane.SetActive(false);
	}

	public DialogueHandler.DeadCallback FinishedCaptainDialogue()
	{
		roerGanger.SetActive(true);
		shaderPlane.SetActive(true);

		// 11,7 + 5,8
		// 9,5 + 6,6

		Vector3 dest = new Vector3(roerGanger.transform.position.x - 2.2f, roerGanger.transform.position.y + 0.8f, shaderPlane.transform.position.z);
		camBTarget.transform.position = dest;
		return null;
	}
	

	public void FinishedZooTypeDialogue()
	{
		
	}
}
