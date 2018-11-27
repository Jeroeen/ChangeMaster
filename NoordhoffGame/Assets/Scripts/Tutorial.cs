using System.Linq.Expressions;
using Assets.Scripts.CameraBehaviour;
using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class Tutorial : MonoBehaviour
	{
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button infoButton;
		[SerializeField] private Button interventionScreenButton;

		[SerializeField] private GameObject coinCount;
		[SerializeField] private GameObject shaderPlane;
		[SerializeField] private GameObject camBTarget;

		[SerializeField] private BoxCollider2D kapitein;
		[SerializeField] private GameObject worldMap;
		[SerializeField] private BoxCollider2D roerganger;
		[SerializeField] private BoxCollider2D wegwijspiet;
		[SerializeField] private BoxCollider2D boekenkast;
		[SerializeField] private GameObject deur;
		

		[SerializeField] private CameraController cameraController;

		private bool settingsButtonStage;
		private bool informationButtonStage;
		private bool tutorialActive;

		void Start()
		{
			tutorialActive = true;

			cameraController.CanUse = false;

			DialogueHandler.OnConversationDoneEvent += ConversationDone;
		}

		public void InterventionScreenDone()
		{
			wegwijspiet.enabled = true;
			cameraController.CanUse = true;

			infoButton.interactable = true;
			settingsButton.interactable = true;

			roerganger.enabled = true;
			kapitein.enabled = true;
			boekenkast.enabled = true;
			
			deur.SetActive(true);

			tutorialActive = false;
		}

		public void InformationScreenDone()
		{
			if (!tutorialActive)
			{
				return;
			}

			camBTarget.transform.position = new Vector3(interventionScreenButton.transform.position.x - 7, interventionScreenButton.transform.position.y + 3, shaderPlane.transform.position.z);

			interventionScreenButton.gameObject.SetActive(true);
			shaderPlane.SetActive(true);
			informationButtonStage = true;
			infoButton.interactable = false;
		}

		private void ConversationDone(CharacterModel characterModel)
		{
			if (!tutorialActive)
			{
				return;
			}

			if (characterModel.NameOfPartner == "Kapitein")
			{
				FinishedCaptainDialogue();
			}

			if (characterModel.NameOfPartner == "Roerganger")
			{
				FinishedHelmsmanDialogue();
			}
		}

		public void SettingsButtonDone()
		{
			if (!tutorialActive)
			{
				return;
			}

			camBTarget.transform.position = new Vector3(infoButton.transform.position.x + 1f,
				infoButton.transform.position.y + 3f, shaderPlane.transform.position.z);

			infoButton.gameObject.SetActive(true);
			shaderPlane.SetActive(true);

			settingsButtonStage = true;
			settingsButton.interactable = false;
		}

		public void ObjectInfoDone()
		{
			if (!tutorialActive)
			{
				return;
			}
			// Actual: 31,4X + 12U 
			// Desired: 24,3X + 11Y

			camBTarget.transform.position = new Vector3(settingsButton.transform.position.x - 7f, settingsButton.transform.position.y - 1f, shaderPlane.transform.position.z);

			settingsButton.gameObject.SetActive(true);
			shaderPlane.SetActive(true);

			boekenkast.enabled = false;
		}

		private void FinishedCaptainDialogue()
		{
			if (!tutorialActive)
			{
				return;
			}
			// Actual: 11,7X + 5,8Y
			// Desired: 9,5X + 6,6Y

			camBTarget.transform.position = new Vector3(roerganger.transform.position.x - 2.2f, roerganger.transform.position.y + 0.8f, shaderPlane.transform.position.z);

			roerganger.enabled = true;
			shaderPlane.SetActive(true);

			kapitein.enabled = false;
		}

		private void FinishedHelmsmanDialogue()
		{
			if (!tutorialActive)
			{
				return;
			}

			camBTarget.transform.position = new Vector3(boekenkast.transform.position.x - 2.5f, boekenkast.transform.position.y - 0.5f, shaderPlane.transform.position.z);

			boekenkast.gameObject.SetActive(true);
			boekenkast.enabled = true;
			shaderPlane.SetActive(true);

			roerganger.enabled = false;
		}

		public void DisableShader()
		{
			if (!tutorialActive)
			{
				return;
			}

			shaderPlane.SetActive(false);
		}
	}
}
