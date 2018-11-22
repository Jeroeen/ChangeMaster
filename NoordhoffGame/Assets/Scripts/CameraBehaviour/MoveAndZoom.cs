using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CameraBehaviour
{
    public abstract class MoveAndZoom : MonoBehaviour
    {
        protected float ZoomValue;
        [SerializeField] protected float ZoomSpeed;

        [SerializeField] protected int MinZoom;
        [SerializeField] protected int MaxZoom;

        [SerializeField] protected ViewportHandler ViewportHandler;

        [SerializeField] protected Camera Camera;

        // Movement of an object or camera while on the computer
        protected void ComputerMovement(Func<Vector3, Vector3> zoomOperate)
        {
            if (Input.GetMouseButton(0))
            {
                float speed = ViewportHandler.UnitsSize;
                zoomOperate(new Vector3(Input.GetAxis("Mouse X") * speed * Time.deltaTime,
                    Input.GetAxis("Mouse Y") * speed * Time.deltaTime,
                    0));
            }
        }

        // Zooming in or out of an object or camera while on the computer
        protected void ComputerZoom(Func<float, float> zoomUp, Func<float, float> zoomDown)
        {
            const int pcZoomMultiplier = 6;

            if (Input.GetKey("up"))
            {
                zoomUp(ZoomSpeed * pcZoomMultiplier * Time.deltaTime);
            }
            else if (Input.GetKey("down"))
            {
                zoomDown(ZoomSpeed * pcZoomMultiplier * Time.deltaTime);
            }
        }

        // Movement of an object or camera while on a mobile device
        protected void MobileMovement(Func<float, float> operate)
        {
            // Swiping with 1 finger to move the Image in given direction
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // The best speed for 16 zoomValue is 0.4, for 32 zoomValue it's double that (0.8)
                // 16 / 0.4 = 40, 32 / 0.8 = 40
                float speed = ViewportHandler.UnitsSize / 40f;

                Vector2 deltaPos = Input.GetTouch(0).deltaPosition;
                transform.Translate(operate(deltaPos.x) * speed * Time.deltaTime,
                    operate(deltaPos.y) * speed * Time.deltaTime, 0);
            }
        }

        // Zooming in or out of an object or camera while on a mobile device
        protected void MobileZoom(Func<float, float> zoomUp, Func<float, float> zoomDown)
        {
            // Pinching to zoom in and out
            if (Input.touchCount == 2)
            {
                Touch firstTouch = Input.GetTouch(0);
                Touch secondTouch = Input.GetTouch(1);

                Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
                Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

                float prevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
                float curPosDifference = (firstTouch.position - secondTouch.position).magnitude;

                float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * ZoomSpeed;

                if (prevPosDifference > curPosDifference)
                {
                    zoomDown(zoomModifier * Time.deltaTime);
                }

                if (prevPosDifference < curPosDifference)
                {
                    zoomUp(zoomModifier * Time.deltaTime);
                }
            }
        }
    }
}
