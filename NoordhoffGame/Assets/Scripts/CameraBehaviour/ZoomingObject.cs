﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CameraBehaviour;
using UnityEditor;
using UnityEngine;

public class ZoomingObject : MoveAndZoom
{
    void Start()
    {
        zoomValue = 1;
    }

    void Update()
    {
#if UNITY_EDITOR
        ComputerMovement(x => transform.position += x);
        ComputerZoom(x => zoomValue += x, y => zoomValue -= y);
#elif UNITY_ANDROID
        MobileMovement(x => x);
        MobileZoom(x => ZoomValue += x, y => ZoomValue -= y);
#endif
        zoomValue = Mathf.Clamp(zoomValue, zinZoom, zaxZoom);
        transform.localScale = new Vector3(zoomValue, zoomValue, zoomValue);

        RestrictObjectToBoundary();
    }

    void OnDisable()
    {
        zoomValue = 1;
        transform.localPosition = Vector3.zero;
    }

    private void RestrictObjectToBoundary()
    {
        float camHeight = 2f * camera.orthographicSize;
        float camWidth = camHeight * camera.aspect;
        Vector3 camMin = new Vector3(camera.transform.position.x - camWidth / 2, camera.transform.position.y - camHeight / 2, camera.transform.position.z);
        Vector3 camMax = new Vector3(camera.transform.position.x + camWidth / 2, camera.transform.position.y + camHeight / 2, camera.transform.position.z);
        float xOffset = 0;
        float yOffset = 0;
        float offsetValue = 0.5f;

        if (transform.position.x < camMin.x)
        {
            xOffset = offsetValue;
        }
        if (transform.position.x > camMax.x)
        {
            xOffset = -offsetValue;
        }
        if (transform.position.y < camMin.y)
        {
            yOffset = offsetValue;
        }
        if (transform.position.y > camMax.y)
        {
            yOffset = -offsetValue;
        }

        transform.position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
    }
}
