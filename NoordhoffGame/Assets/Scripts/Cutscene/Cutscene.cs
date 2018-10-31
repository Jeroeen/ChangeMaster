using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
	private float _zoomValue;
	private Vector3 _destination = Vector3.zero;
	private readonly Queue<Transform> _destinations = new Queue<Transform>();
	private bool _isMoveZoomingCamera;
	private bool _hasDialogueOpened;

	[SerializeField] private float _zoomSpeed;
	[SerializeField] private float _moveSpeed;
	[SerializeField] private int _minZoom;
	[SerializeField] private GameObject _cutScene;
	[SerializeField] private InitiateDialogue _initiateDialogue;
	[SerializeField] private GameObject _dialogue;
	[SerializeField] private Transition _transition;
	
	public ViewportHandler ViewportHandler;

	void Start()
	{
		foreach (Transform obj in _cutScene.transform)
		{
			_destinations.Enqueue(obj);
		}

		Vector3 destination = _destinations.Dequeue().position;
		_destination = new Vector3(destination.x, destination.y, transform.position.z);

		_zoomValue = ViewportHandler.UnitsSize;
		_isMoveZoomingCamera = true;
	}

	void Update()
	{
		if (_destinations.Count == 0 && transform.position == _destination && !_dialogue.activeSelf)
		{
			if (_hasDialogueOpened)
			{
				if (!_transition.FadeOut())
				{
					_dialogue.SetActive(false);
					return;
				}

				SceneManager.LoadScene(2);
			}

			if (!_hasDialogueOpened)
			{
				_isMoveZoomingCamera = false;
				_hasDialogueOpened = true;

				_dialogue.SetActive(true);
				_initiateDialogue.Initialize("Kapitein", 0, -1);
				return;
			}
		}

		if (!_isMoveZoomingCamera)
		{
			return;
		}

		MoveZoomCamera();
		ViewportHandler.UnitsSize = _zoomValue;
	}

	private void MoveZoomCamera()
	{
		if (transform.position == _destination)
		{
			var destination = _destinations.Dequeue().position;
			_destination = new Vector3(destination.x, destination.y, transform.position.z);
		}

		transform.position = Vector3.MoveTowards(transform.position, _destination, _moveSpeed * Time.deltaTime);

		if ((int)Math.Ceiling(_zoomValue) != _minZoom)
		{
			_zoomValue -= _zoomSpeed * Time.deltaTime;
		}

	}
}
