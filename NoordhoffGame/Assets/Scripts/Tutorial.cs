using System.Linq.Expressions;
using Assets.Scripts.CameraBehaviour;
using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Models;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class Tutorial : MonoBehaviour
	{
		[SerializeField] private Button settingsButton = null;
		[SerializeField] private Button infoButton = null;
		[SerializeField] private Button interventionScreenButton = null;
		[SerializeField] private Button resetTutorial = null;
		[SerializeField] private Button keepLookingButton = null;
		[SerializeField] private Button chooseInterventionButton = null;
		[SerializeField] private Button closeHintButton = null;
		[SerializeField] private Button cancelChosenInterventionButton = null;
		[SerializeField] private Button confirmChosenInterventionButton = null;
		[SerializeField] private Button closeInterventionScreen = null;

		[SerializeField] private GameObject shaderPlane = null;
		[SerializeField] private GameObject camBTarget = null;
		[SerializeField] private GameObject camBTargetB = null;
		[SerializeField] private GameObject mainCamera = null;
		[SerializeField] private GameObject interventionHintButton = null;
		[SerializeField] private GameObject interventionImage = null;

		[SerializeField] private BoxCollider2D captain;
		[SerializeField] private BoxCollider2D helmsman;
		[SerializeField] private BoxCollider2D lookout;
		[SerializeField] private BoxCollider2D bookcase;

		[SerializeField] private CameraController cameraController;

		[SerializeField] private CanvasGroup blocking;

		private bool isTutorialActive;
		private bool interventionClickedOn;

		private Vector3 camBTargetStartPosition;
		private Vector3 camBTargetStartPositionB;
		private Vector3 initialCameraPosition;
		private Vector3 initialCamBTargetScale;

		void Start()
		{
			initialCameraPosition = mainCamera.transform.position;
			isTutorialActive = false;
			resetTutorial.interactable = false;

			DisableInteractables();
			
			DialogueHandler.OnConversationDoneEvent += ConversationDone;
			InterventionScreen.OnHintButtonClickEvent += OnHintClickDone;
			InterventionScreen.OnInterventionChooseButtonClickEvent += OnInterventionChoiceClickDone;
			InterventionScreen.AllInformationFound += ClickOnInterventionDone;
			
			camBTargetStartPosition = camBTarget.transform.position;
			camBTargetStartPositionB = camBTargetB.transform.position;
			initialCamBTargetScale = camBTargetB.transform.localScale;
		}

		// Move spotlight to position with specified x and y. Z will stay the same
		private void MoveSpotlight(float x, float y)
		{
			shaderPlane.SetActive(true);
			camBTarget.transform.position = new Vector3(x, y, camBTarget.transform.position.z);
		}

		// Move spotlight to specified position
		private void MoveSpotlight(Vector3 position)
		{
			if (!isTutorialActive)
			{
				return;
			}

			shaderPlane.SetActive(true);
			camBTarget.transform.position = position;
		}

		private void DisableInteractables()
		{
			cameraController.CanUse = false;

			settingsButton.gameObject.SetActive(false);
			infoButton.gameObject.SetActive(false);
			interventionScreenButton.gameObject.SetActive(false);

			confirmChosenInterventionButton.interactable = false;
			closeInterventionScreen.interactable = false;

			helmsman.enabled = false;
			lookout.enabled = false;
			bookcase.enabled = false;
		}

		private void ConversationDone(CharacterModel characterModel)
		{
			if (characterModel.NameOfPartner == "Kapitein")
			{
				FinishedCaptainDialogue();
			}

			if (characterModel.NameOfPartner == "Roerganger")
			{
				FinishedHelmsmanDialogue();
			}

			if (characterModel.NameOfPartner == "Uitkijk")
			{
				FinishedLookOutDialogue();
			}
		}

		private void FinishedCaptainDialogue()
		{
			if (!isTutorialActive)
			{
				return;
			}

			// Actual position: 11,7X + 5,8Y
			// Desired position: 9,5X + 6,6Y
			// Difference: 2,2X & 0,8Y
			MoveSpotlight(helmsman.transform.position.x - 2.2f, helmsman.transform.position.y + 0.8f);

			helmsman.enabled = true;

			captain.enabled = false;
		}

		private void FinishedHelmsmanDialogue()
		{
			if (!isTutorialActive)
			{
				return;
			}

			// Actual position: 15.3X, 10.9Y 
			// Desired position: 12.8X, 10.4Y
			// Difference: -2.5X, -0.5Y
			MoveSpotlight(bookcase.transform.position.x - 2.5f, bookcase.transform.position.y - 0.5f);

			bookcase.gameObject.SetActive(true);
			bookcase.enabled = true;

			helmsman.enabled = false;
		}

		private void FinishedLookOutDialogue()
		{
			if (!isTutorialActive)
			{
				return;
			}

			// Reset the camera to the initial position       
			mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 4, mainCamera.transform.position.z);

			// Actual position: 33X, -5Y
			// Desired position: 26X, 2Y
			// Difference: -7X, 7Y
			MoveSpotlight(interventionScreenButton.transform.position.x - 7f, interventionScreenButton.transform.position.y + 7f);

			interventionScreenButton.interactable = true;
			lookout.enabled = false;
		}

		private void OnHintClickDone()
		{
			if (!isTutorialActive)
			{
				return;
			}

			// Actual position: 24.2X, 5.2Y
			// Desired position: 19.2X, 5.2Y
			// Difference: 5X, 0Y
			MoveSpotlight(closeHintButton.transform.position.x - 5f, closeHintButton.transform.position.y);

			camBTargetB.SetActive(true);
			camBTargetB.transform.position = new Vector3(camBTargetB.transform.position.x, camBTargetB.transform.position.y, camBTargetB.transform.position.z);
		}

		private void OnInterventionChoiceClickDone()
		{
			if (!isTutorialActive)
			{
				return;
			}

			// Actual position: 14.4X, -0.7Y
			// Desired position: 12.9X, 0.5Y 
			// Difference: -2.5X, 1.2Y
			MoveSpotlight(cancelChosenInterventionButton.transform.position.x - 2.5f, cancelChosenInterventionButton.transform.position.y + 1.2f);

			// Actual position: 15.6X, 4.8Y. Scale: 1.15X, 0.3Y
			// Desired position: 15.9X, 3.3Y. Scale: 1.15X, 0.6Y
			// Difference: 0.3X, 1.5Y. Scale: 0.3Y
			camBTargetB.SetActive(true);
			camBTargetB.transform.position = new Vector3(camBTargetB.transform.position.x + 0.3f, camBTargetB.transform.position.y - 1.5f, camBTargetB.transform.position.z);
			camBTargetB.transform.localScale = new Vector3(camBTargetB.transform.localScale.x, camBTargetB.transform.localScale.y + 0.3f, camBTargetB.transform.localScale.z);
		}

		public void ObjectInfoDone()
		{
			if (!isTutorialActive)
			{
				return;
			}
			// Actual position: 31,4X + 12U 
			// Desired position: 24,4X + 11Y
			// Difference: -7X, 1Y
			MoveSpotlight(settingsButton.transform.position.x - 7f, settingsButton.transform.position.y - 1f);

			settingsButton.gameObject.SetActive(true);

			bookcase.enabled = false;
		}

		public void SettingsButtonDone()
		{
			if (!isTutorialActive)
			{
				return;
			}

			// Actual position: 14.4X, 4.9Y
			// Desired position: 15.4X, 7.9Y
			// Difference: 1X, 3Y
			MoveSpotlight(infoButton.transform.position.x + 1f, infoButton.transform.position.y + 3f);

			infoButton.gameObject.SetActive(true);

			settingsButton.interactable = false;
		}

		public void InformationScreenDone()
		{
			if (!isTutorialActive)
			{
				return;
			}

			// Actual position: 19.8X, 13.4Y
			// Desired position: 12.8X, 10.4Y
			// Difference: -7X, 3Y
			MoveSpotlight(interventionScreenButton.transform.position.x - 7f, interventionScreenButton.transform.position.y + 3f);

			interventionScreenButton.gameObject.SetActive(true);
			infoButton.interactable = false;
		}

		private void ClickOnInterventionDone(bool isAllInformationFound)
		{
			if (!isTutorialActive)
			{
				return;
			}

			if (isAllInformationFound)
			{
				camBTargetB.SetActive(false);
				// Location of the hint-button
				MoveSpotlight(interventionHintButton.transform.position.x, interventionHintButton.transform.position.y);
			}
			else
			{
				// Actual position: 16X + 3.5Y
				// Desired position: 13.2X + 5.5Y
				// Difference: -2.8X, 2Y
				chooseInterventionButton.interactable = false;
				MoveSpotlight(keepLookingButton.gameObject.transform.position.x - 2.8f, keepLookingButton.gameObject.transform.position.y + 2f);
				camBTargetB.SetActive(true);
			}
		}

		public void InterventionScreenDone()
		{
			if (!isTutorialActive)
			{
				return;
			}
			// Actual position: 15,4X + 8Y
			// Desired position: 9.6X + 5Y
			// Difference: -5.8X, -3Y
			MoveSpotlight(keepLookingButton.gameObject.transform.position.x - 5.8f, keepLookingButton.gameObject.transform.position.y - 3f);
			camBTargetB.SetActive(false);

			interventionScreenButton.interactable = false;
			lookout.enabled = true;
			// Lower the camera position to make sure the lookout can be seen
			mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 4, mainCamera.transform.position.z);
		}

		public void HintClosed()
		{
			if (!isTutorialActive)
			{
				return;
			}

			camBTargetB.SetActive(false);
			// Location of the generated intervention icon by Interventionscreen.cs
			MoveSpotlight(interventionImage.transform.position.x, interventionImage.transform.position.y);
		}

		public void InitiateTutorial()
		{
			isTutorialActive = true;
			mainCamera.transform.position = initialCameraPosition;

			blocking.blocksRaycasts = false;
			resetTutorial.interactable = false;

			camBTargetB.SetActive(false);
			camBTargetB.transform.position = camBTargetStartPositionB;
			MoveSpotlight(camBTargetStartPosition);
			
			DisableInteractables();
		}

		public void DeactivateTutorial()
		{
			resetTutorial.interactable = true;

			settingsButton.gameObject.SetActive(true);
			infoButton.gameObject.SetActive(true);
			interventionScreenButton.gameObject.SetActive(true);

			shaderPlane.SetActive(false);

			cameraController.CanUse = true;

			infoButton.interactable = true;
			settingsButton.interactable = true;
			confirmChosenInterventionButton.interactable = true;
			closeInterventionScreen.interactable = true;

			helmsman.enabled = true;
			captain.enabled = true;
			bookcase.enabled = true;
			lookout.enabled = true;


			camBTargetB.transform.localScale = initialCamBTargetScale;

			isTutorialActive = false;
		}

		public void DisableShader()
		{
			shaderPlane.SetActive(false);
		}
	}
}
