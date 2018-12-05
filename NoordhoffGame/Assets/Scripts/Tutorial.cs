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
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button infoButton;
		[SerializeField] private Button interventionScreenButton;
		[SerializeField] private Button resetTutorial;
		[SerializeField] private Button keepLookingButton;
		[SerializeField] private Button chooseInterventionButton;
		[SerializeField] private Button closeHintButton;
		[SerializeField] private Button cancelChosenInterventionButton;
		[SerializeField] private Button confirmChosenInterventionButton;
		[SerializeField] private Button closeInterventionScreen;

		[SerializeField] private GameObject shaderPlane;
		[SerializeField] private GameObject camBTarget;
		[SerializeField] private GameObject camBTargetB;
		[SerializeField] private GameObject mainCamera;
		[SerializeField] private GameObject interventionHintButton;
		[SerializeField] private GameObject interventionImage;

		[SerializeField] private BoxCollider2D captain;
		[SerializeField] private BoxCollider2D helmsman;
		[SerializeField] private BoxCollider2D lookout;
		[SerializeField] private BoxCollider2D bookcase;

		[SerializeField] private CameraController cameraController;

		[SerializeField] private CanvasGroup blocking;

		private bool tutorialActive;
		private bool interventionClickedOn;

		private Vector3 camBTargetStartPosition;
		private Vector3 camBTargetStartPositionB;
		private Vector3 initialCameraposition;
		private Vector3 initialCamBTargetScale;

		void Start()
		{
			initialCameraposition = mainCamera.transform.position;
			tutorialActive = false;
			resetTutorial.interactable = false;

			DisableInteractables();

			DialogueHandler.OnConversationDoneEvent += ConversationDone;
			InterventionScreen.OnHintButtonClickEvent += OnHintClickDone;
			InterventionScreen.OnInterventionChooseButtonClickEvent += OnInterventionChoiceClickDone;
			InterventionScreen.AllInformationFound += ClickOnInterventionDone;

			// Actual: 19.5X, 5.8Y
			// Desired: 16, 6.8Y

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
			if (!tutorialActive)
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
			if (!tutorialActive)
			{
				return;
			}

			// Actual: 11,7X + 5,8Y
			// Desired: 9,5X + 6,6Y
			MoveSpotlight(helmsman.transform.position.x - 2.2f, helmsman.transform.position.y + 0.8f);

			helmsman.enabled = true;

			captain.enabled = false;
		}

		private void FinishedHelmsmanDialogue()
		{
			if (!tutorialActive)
			{
				return;
			}

			MoveSpotlight(bookcase.transform.position.x - 2.5f, bookcase.transform.position.y - 0.5f);

			bookcase.gameObject.SetActive(true);
			bookcase.enabled = true;

			helmsman.enabled = false;
		}

		private void FinishedLookOutDialogue()
		{
			if (!tutorialActive)
			{
				return;
			}

			mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 4, mainCamera.transform.position.z);

			// Actual: 33X, -5Y
			// Desired: 26X, 2Y
			MoveSpotlight(interventionScreenButton.transform.position.x - 7f, interventionScreenButton.transform.position.y + 7f);

			interventionScreenButton.interactable = true;
			lookout.enabled = false;
		}

		private void OnHintClickDone()
		{
			if (!tutorialActive)
			{
				return;
			}

			// Actual: 24.2X, 5.2Y
			// Desired: 19.2X, 9Y
			MoveSpotlight(closeHintButton.transform.position.x - 5f, closeHintButton.transform.position.y);

			camBTargetB.SetActive(true);
			camBTargetB.transform.position = new Vector3(camBTargetB.transform.position.x, camBTargetB.transform.position.y, camBTargetB.transform.position.z);
		}

		private void OnInterventionChoiceClickDone()
		{
			if (!tutorialActive)
			{
				return;
			}

			// Actual: 14.4X, -0.7Y
			// Desired: 11.9X, 1.5Y 
			MoveSpotlight(cancelChosenInterventionButton.transform.position.x - 2.5f, cancelChosenInterventionButton.transform.position.y + 1.2f);

			// Actual: 15.6X, 4.8Y. Scale: 1.1X, 0.3Y
			// Desired: 15.9X, 3.3Y. Scale: 1.15X, 0.6Y
			camBTargetB.SetActive(true);
			camBTargetB.transform.position = new Vector3(camBTargetB.transform.position.x + 0.3f, camBTargetB.transform.position.y - 1.5f, camBTargetB.transform.position.z);
			camBTargetB.transform.localScale = new Vector3(camBTargetB.transform.localScale.x, camBTargetB.transform.localScale.y + 0.3f, camBTargetB.transform.localScale.z);
		}

		public void ObjectInfoDone()
		{
			if (!tutorialActive)
			{
				return;
			}
			// Actual: 31,4X + 12U 
			// Desired: 24,3X + 11Y

			MoveSpotlight(settingsButton.transform.position.x - 7f, settingsButton.transform.position.y - 1f);

			settingsButton.gameObject.SetActive(true);

			bookcase.enabled = false;
		}

		public void SettingsButtonDone()
		{
			if (!tutorialActive)
			{
				return;
			}

			MoveSpotlight(infoButton.transform.position.x + 1f, infoButton.transform.position.y + 3f);

			infoButton.gameObject.SetActive(true);

			settingsButton.interactable = false;
		}

		public void InformationScreenDone()
		{
			if (!tutorialActive)
			{
				return;
			}

			MoveSpotlight(interventionScreenButton.transform.position.x - 7f, interventionScreenButton.transform.position.y + 3f);

			interventionScreenButton.gameObject.SetActive(true);
			infoButton.interactable = false;
		}

		private void ClickOnInterventionDone(bool allInformationFound)
		{
			if (!tutorialActive)
			{
				return;
			}

			if (allInformationFound)
			{
				camBTargetB.SetActive(false);
				// Location of the hint-button
				MoveSpotlight(interventionHintButton.transform.position.x, interventionHintButton.transform.position.y);
			}
			else
			{
				// Actual: 16X + 3.5Y
				// Desired: 13.2X + 5.5Y
				chooseInterventionButton.interactable = false;
				MoveSpotlight(keepLookingButton.gameObject.transform.position.x - 2.8f, keepLookingButton.gameObject.transform.position.y + 2f);
				camBTargetB.SetActive(true);
			}
		}

		public void InterventionScreenDone()
		{
			if (!tutorialActive)
			{
				return;
			}
			// Actual: 15,4X + 7,89Y
			// Desired: 10,27X + 0,95

			MoveSpotlight(keepLookingButton.gameObject.transform.position.x - 5.8f, keepLookingButton.gameObject.transform.position.y - 3f);
			camBTargetB.SetActive(false);

			interventionScreenButton.interactable = false;
			lookout.enabled = true;
			// Lower the camera position to make sure the lookout can be seen
			mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 4, mainCamera.transform.position.z);
		}

		public void HintClosed()
		{
			if (!tutorialActive)
			{
				return;
			}

			camBTargetB.SetActive(false);
			// Location of the generated intervention icon by Interventionscreen.cs
			MoveSpotlight(interventionImage.transform.position.x, interventionImage.transform.position.y);
		}

		public void InitiateTutorial()
		{
			tutorialActive = true;
			mainCamera.transform.position = initialCameraposition;

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

			tutorialActive = false;
		}

		public void DisableShader()
		{
			shaderPlane.SetActive(false);
		}
	}
}
