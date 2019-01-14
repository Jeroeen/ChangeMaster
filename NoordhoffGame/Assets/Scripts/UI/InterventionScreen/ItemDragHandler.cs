using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.InterventionScreen
{
    public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private Vector2 startposition;

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 positionPointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            positionPointer.z = transform.position.z;
            transform.position = positionPointer;        
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.localPosition = startposition;
        }

        public void OnStartDrag()
        {
            startposition = transform.localPosition;
        }
    
    }
}
