using System;
using System.Collections.Generic;
using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Models;
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
		
		[SerializeField] private float zoomSpeed;
		[SerializeField] private float moveSpeed;
		[SerializeField] private int minZoom;
		[SerializeField] private GameObject cutScene;
		[SerializeField] private DialogueHandler dialogueHandler;
		[SerializeField] private GameObject dialogue;
		[SerializeField] private Transition transition;
	
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

					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
				}

				if (!hasDialogueOpened)
				{
					isMoveZoomingCamera = false;
					hasDialogueOpened = true;

					dialogue.SetActive(true);
					CharacterModel model = new CharacterModel
					{
						NameOfPartner = "Kapitein",
						Stage = "0",
						AmountOfDialogues = -1,
						DialogueCount = -1
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
