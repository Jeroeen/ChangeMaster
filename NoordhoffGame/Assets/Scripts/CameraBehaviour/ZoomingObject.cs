using Assets.Scripts.CameraBehaviour;
using UnityEngine;

public class ZoomingObject : MoveAndZoom
{
    [SerializeField] private bool canMove = false;

    void Start()
    {
        zoomValue = 1;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (canMove)
        {
            ComputerMovement(x => transform.position += x);
        }
        ComputerZoom(x => zoomValue += x, y => zoomValue -= y);
#elif UNITY_ANDROID
        if (canMove)
        {
            MobileMovement(x => x);
        }
        MobileZoom(x => zoomValue += x, y => zoomValue -= y);
#endif
        zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);
        transform.localScale = new Vector3(zoomValue, zoomValue, zoomValue);

        RestrictObjectToBoundary();
    }

    void OnDisable()
    {
        if (!gameObject.activeSelf)
        {
            zoomValue = 1;
            transform.localPosition = Vector3.zero;
        }
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
