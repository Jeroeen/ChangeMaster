using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	[SerializeField] private InitiateDialogue initiateDialogue;
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

				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}

			if (!hasDialogueOpened)
			{
				isMoveZoomingCamera = false;
				hasDialogueOpened = true;

				dialogue.SetActive(true);
				CharModel model = new CharModel()
				{
					NameOfPartner = "Kapitein",
					Stage = "0",
					AmountOfDialogues = -1,
				};
				initiateDialogue.Initialize(model);
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
			var destination = destinations.Dequeue().position;
			this.destination = new Vector3(destination.x, destination.y, transform.position.z);
		}

		transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

		if ((int)Math.Ceiling(zoomValue) != minZoom)
		{
			zoomValue -= zoomSpeed * Time.deltaTime;
		}

	}
}
