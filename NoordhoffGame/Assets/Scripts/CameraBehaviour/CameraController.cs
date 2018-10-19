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
    private bool isPlayingCutscene;
    private bool canFreeRoam;
    private Vector3 Destination = Vector3.zero;

    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int minZoom = 16;
    [SerializeField] private int maxZoom = 33;

    public ViewportHandler ViewportHandler;
    public TilemapHandler TilemapHandler;
    public Camera Camera;
	public MouseChecker Checker;

	void Start()
    {
        zoomValue = ViewportHandler.UnitsSize;

        // Delete this To start cutscene first
        canFreeRoam = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z);
        zoomValue = minZoom;
    }
    
    void Update()
    {
        if (!isPlayingCutscene && !canFreeRoam)
        {
            Destination = new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z);
            isPlayingCutscene = true;
        }

	    if (Checker.IsPointerOverUI) return;

	    if (isPlayingCutscene)
	    {
		    ExecuteCutscene();
	    }
	    else if (!isPlayingCutscene && canFreeRoam)
	    {
		    ExecuteFreeRoam();
	    }

	    zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);
	    ViewportHandler.UnitsSize = zoomValue;
    }

    private void ExecuteCutscene()
    {
        zoomValue -= zoomSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Destination, moveSpeed * Time.deltaTime);

        if ((int)Math.Ceiling(zoomValue) == minZoom)
        {
            isPlayingCutscene = false;
            canFreeRoam = true;
        }
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
            transform.position -= new Vector3(Input.GetAxis("Mouse X") * zoomValue * Time.deltaTime, Input.GetAxis("Mouse Y") * zoomValue * Time.deltaTime, 0);
        }

        if (Input.GetKey("up"))
        {
            zoomValue += zoomSpeed * 6 * Time.deltaTime;
        }
        else if (Input.GetKey("down"))
        {
            zoomValue -= zoomSpeed * 6 * Time.deltaTime;
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

            float touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
            {
                zoomValue += zoomModifier * Time.deltaTime;
            }

            if (touchesPrevPosDifference < touchesCurPosDifference)
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
