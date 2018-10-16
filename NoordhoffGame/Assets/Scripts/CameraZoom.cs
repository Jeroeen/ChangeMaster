using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float zoomValue;
    public float ZoomSpeed = 2f;
    public ViewportHandler ViewportHandler;

    void Start()
    {
        zoomValue = ViewportHandler.UnitsSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (zoomValue + ZoomSpeed > ZoomSpeed)
        {
            zoomValue += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            ViewportHandler.UnitsSize = zoomValue;
        }
    }
}
