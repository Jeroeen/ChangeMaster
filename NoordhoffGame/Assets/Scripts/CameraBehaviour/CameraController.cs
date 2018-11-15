using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngineInternal;

public class CameraController : MonoBehaviour
{
	private float zoomValue;
	public bool CanUse;
    
	[SerializeField] private float zoomSpeed = 2f;
	[SerializeField] private int minZoom = 16;
	[SerializeField] private int maxZoom = 33;

    [SerializeField] private Transform startPosition;

	public ViewportHandler ViewportHandler;
	public TilemapHandler TilemapHandler;
	public Camera Camera;
	public MouseChecker Checker;

	void Start()
	{
	    transform.position = new Vector3(startPosition.position.x, startPosition.position.y, transform.position.z);

        zoomValue = minZoom;

		ViewportHandler.UnitsSize = zoomValue;
	}

	void Update()
	{
		if (Checker.IsPointerOverUI || !CanUse)
		{
			return;
		}

		ExecuteFreeRoam();

		zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);
		ViewportHandler.UnitsSize = zoomValue;
	}
    
	private void ExecuteFreeRoam()
	{
#if UNITY_EDITOR
		UseEditorControls();
#elif UNITY_ANDROID
        UseMobileControls();
#endif

		RestrictCameraToBoundary();
	}

	private void UseEditorControls()
	{
		if (Input.GetMouseButton(0))
		{
			transform.position -= new Vector3(Input.GetAxis("Mouse X") * zoomValue * Time.deltaTime,
											  Input.GetAxis("Mouse Y") * zoomValue * Time.deltaTime,
											  0);
		}

		const int pcZoomMultiplier = 6;

		if (Input.GetKey("up"))
		{
			zoomValue += zoomSpeed * pcZoomMultiplier * Time.deltaTime;
		}
		else if (Input.GetKey("down"))
		{
			zoomValue -= zoomSpeed * pcZoomMultiplier * Time.deltaTime;
		}
	}

	private void UseMobileControls()
	{
		// Swiping with 1 finger to move the Camera in given direction
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			// The best speed for 16 zoomValue is 0.4, for 32 zoomValue it's double that (0.8)
			// 16 / 0.4 = 40, 32 / 0.8 = 40
			float speed = zoomValue / 40f;

			Vector2 deltaPos = Input.GetTouch(0).deltaPosition;
			transform.Translate(-deltaPos.x * speed * Time.deltaTime,
				-deltaPos.y * speed * Time.deltaTime, 0);
		}

		// Pinching to zoom in and out
		if (Input.touchCount == 2)
		{
			Touch firstTouch = Input.GetTouch(0);
			Touch secondTouch = Input.GetTouch(1);

			Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
			Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

			float prevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
			float curPosDifference = (firstTouch.position - secondTouch.position).magnitude;

			float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;

			if (prevPosDifference > curPosDifference)
			{
				zoomValue += zoomModifier * Time.deltaTime;
			}

			if (prevPosDifference < curPosDifference)
			{
				zoomValue -= zoomModifier * Time.deltaTime;
			}
		}
	}

	private void RestrictCameraToBoundary()
	{
		float camHeight = 2f * Camera.orthographicSize;
		float camWidth = camHeight * Camera.aspect;
		Vector3 camMin = new Vector3(transform.position.x - camWidth / 2, transform.position.y - camHeight / 2, transform.position.z);
		Vector3 camMax = new Vector3(transform.position.x + camWidth / 2, transform.position.y + camHeight / 2, transform.position.z);

		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
		{
			if (camMin.x <= TilemapHandler.MinBounds.x)
			{
				transform.Translate(TilemapHandler.MinBounds.x - transform.position.x + camWidth / 2, 0, 0);
			}
			if (camMax.x >= TilemapHandler.MaxBounds.x)
			{
				transform.Translate(TilemapHandler.MaxBounds.x - transform.position.x - camWidth / 2, 0, 0);
			}
			if (camMin.y <= TilemapHandler.MinBounds.y)
			{
				transform.Translate(0, TilemapHandler.MinBounds.y - transform.position.y + camHeight / 2, 0);
			}
			if (camMax.y >= TilemapHandler.MaxBounds.y)
			{
				transform.Translate(0, TilemapHandler.MaxBounds.y - transform.position.y - camHeight / 2, 0);
			}
		}
	}
}
