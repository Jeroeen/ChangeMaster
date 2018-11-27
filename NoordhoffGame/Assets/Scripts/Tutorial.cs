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
		[SerializeField] private Button resetTutorial;
		
		[SerializeField] private GameObject shaderPlane;
		[SerializeField] private GameObject camBTarget;
		[SerializeField] private GameObject mainCamera;

		[SerializeField] private BoxCollider2D kapitein;
		[SerializeField] private BoxCollider2D roerganger;
		[SerializeField] private BoxCollider2D uitkijk;
		[SerializeField] private BoxCollider2D boekenkast;
		
		[SerializeField] private CameraController cameraController;

		[SerializeField] private CanvasGroup blocking;
		
		private bool tutorialActive;

		private Vector3 camBTargetStartPosition;
		private Vector3 initialCameraposition;

		void Start()
		{
			initialCameraposition = mainCamera.transform.position;
			tutorialActive = false;
			resetTutorial.interactable = false;

			DisableInteractables();
			
			DialogueHandler.OnConversationDoneEvent += ConversationDone;

			// Actual: 19.5X, 5.8Y
			// Desired: 16, 6.8Y

			camBTargetStartPosition = new Vector3(kapitein.gameObject.transform.position.x - 3.5f, 
													kapitein.gameObject.transform.position.y + 1f,
													camBTarget.transform.position.z);
		}

		public void InitiateTutorial()
		{
			mainCamera.transform.position = initialCameraposition;

			blocking.blocksRaycasts = false;

			resetTutorial.interactable = false;

			camBTarget.transform.position = camBTargetStartPosition;

			DisableInteractables();
			
			shaderPlane.SetActive(true);
			tutorialActive = true;
		}

		private void DisableInteractables()
		{
			cameraController.CanUse = false;

			settingsButton.gameObject.SetActive(false);
			infoButton.gameObject.SetActive(false);
			interventionScreenButton.gameObject.SetActive(false);
			
			roerganger.enabled = false;
			uitkijk.enabled = false;
			boekenkast.enabled = false;
		}

		public void DeactivateTutorial()
		{
			resetTutorial.interactable = true;

			settingsButton.gameObject.SetActive(true);
			infoButton.gameObject.SetActive(true);
			interventionScreenButton.gameObject.SetActive(true);

			kapitein.enabled = true;
			roerganger.enabled = true;
			infoButton.interactable = true;

			uitkijk.enabled = true;
			cameraController.CanUse = true;

			infoButton.interactable = true;
			settingsButton.interactable = true;

			roerganger.enabled = true;
			kapitein.enabled = true;
			boekenkast.enabled = true;
			
			tutorialActive = false;
		}

		public void InterventionScreenDone()
		{
			DeactivateTutorial();
		}

		public void InformationScreenDone()
		{
			if (!tutorialActive)
			{
				return;
			}

			camBTarget.transform.position = new Vector3(interventionScreenButton.transform.position.x - 7, interventionScreenButton.transform.position.y + 3, camBTarget.transform.position.z);

			interventionScreenButton.gameObject.SetActive(true);
			shaderPlane.SetActive(true);
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
				infoButton.transform.position.y + 3f, camBTarget.transform.position.z);

			infoButton.gameObject.SetActive(true);
			shaderPlane.SetActive(true);
			
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
			
			camBTarget.transform.position = new Vector3(settingsButton.transform.position.x - 7f, settingsButton.transform.position.y - 1f, camBTarget.transform.position.z);

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

			camBTarget.transform.position = new Vector3(roerganger.transform.position.x - 2.2f, roerganger.transform.position.y + 0.8f, camBTarget.transform.position.z);

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

			camBTarget.transform.position = new Vector3(boekenkast.transform.position.x - 2.5f, boekenkast.transform.position.y - 0.5f, camBTarget.transform.position.z);

			boekenkast.gameObject.SetActive(true);
			boekenkast.enabled = true;
			shaderPlane.SetActive(true);

			roerganger.enabled = false;
		}

		public void DisableShader()
		{
			shaderPlane.SetActive(false);
		}
	}
}
