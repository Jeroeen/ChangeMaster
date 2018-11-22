using Assets.Scripts.Tilemap;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CameraBehaviour
{
    public class CameraController : MoveAndZoom
    {
        public bool CanUse;

        [SerializeField] private Transform startPosition;
        
        public TilemapHandler TilemapHandler;
        public MouseChecker Checker;

        void Start()
        {
            transform.position = new Vector3(startPosition.position.x, startPosition.position.y, transform.position.z);

            ZoomValue = MinZoom;

            ViewportHandler.UnitsSize = ZoomValue;
        }

        void Update()
        {
            if (Checker.IsPointerOverUI || !CanUse)
            {
                return;
            }

#if UNITY_EDITOR
            ComputerMovement(x => transform.position -= x);
            ComputerZoom(x => ZoomValue -= x, y => ZoomValue += y);
#elif UNITY_ANDROID
            MobileMovement(x => -x);
            MobileZoom(x => ZoomValue += x, y => ZoomValue -= y);
#endif
            ZoomValue = Mathf.Clamp(ZoomValue, MinZoom, MaxZoom);
            ViewportHandler.UnitsSize = ZoomValue;

            RestrictCameraToBoundary();
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
}
