    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector2 startposition;

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = Input.GetTouch(0).position;
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
