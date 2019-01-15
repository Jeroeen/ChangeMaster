using System;
using System.Collections.Generic;
using Assets.Scripts.CameraBehaviour;
using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Models;
using Assets.Scripts.Initialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Cutscene
{
	public class Cutscene : MonoBehaviour
	{
		private float zoomValue;
		private Vector3 destination = Vector3.zero;
		private readonly Queue<Transform> destinations = new Queue<Transform>();
		private bool isMoveZoomingCamera;
		private bool hasDialogueOpened;

		[SerializeField] private float zoomSpeed = 0;
		[SerializeField] private float moveSpeed = 0;
		[SerializeField] private int minZoom = 0;
		[SerializeField] private GameObject cutScene = null;
		[SerializeField] private DialogueHandler dialogueHandler = null;
		[SerializeField] private GameObject dialogue = null;
		[SerializeField] private Transition transition = null;

		public ViewportHandler ViewportHandler;

		void Start()
		{
			foreach (Transform obj in cutScene.transform)
			{
				destinations.Enqueue(obj);
			}

			Vector3 destination = destinations.Dequeue().position;
			this.destination = new Vector3(destination.x, destination.y, transform.position.z);

			zoomValue = ViewportHandler.UnitsSize;
			isMoveZoomingCamera = true;
		}

		void Update()
		{
			if (destinations.Count == 0 && transform.position == destination && !dialogue.activeSelf)
			{
				if (hasDialogueOpened)
				{
					if (!transition.FadeOut())
					{
						dialogue.SetActive(false);
						return;
					}

					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				}

				if (!hasDialogueOpened)
				{
					isMoveZoomingCamera = false;
					hasDialogueOpened = true;

					dialogue.SetActive(true);
					CharacterModel model = new CharacterModel
					{
						NameOfPartner = "Kapitein",
						Stage = "C",
						AmountOfDialogues = -1,
					};
					dialogueHandler.Initialize(model);
					return;
				}
			}

			if (!isMoveZoomingCamera)
			{
				return;
			}

			MoveZoomCamera();
			ViewportHandler.UnitsSize = zoomValue;
		}

		private void MoveZoomCamera()
		{
			if (transform.position == destination)
			{
				Vector3 nextDestination = destinations.Dequeue().position;
				destination = new Vector3(nextDestination.x, nextDestination.y, transform.position.z);
			}

			transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

			if ((int)Math.Ceiling(zoomValue) != minZoom)
			{
				zoomValue -= zoomSpeed * Time.deltaTime;
			}

		}
	}
}
