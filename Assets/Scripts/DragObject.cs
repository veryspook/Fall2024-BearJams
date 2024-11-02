using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private bool canDrag = true;
    private DragManager _manager = null;

    private DecorateManager _decoManager = null;

    private Vector2 _centerPoint;
    private Vector2 _worldCenterPoint => transform.TransformPoint(_centerPoint);

    private void Awake()
    {

        _manager = GetComponentInParent<DragManager>();
        _decoManager = GetComponentInParent<DecorateManager>();
        _centerPoint = (transform as RectTransform).rect.center;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(canDrag == true) {
            _manager.RegisterDraggedObject(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_manager.IsWithinBounds(_worldCenterPoint + eventData.delta)&& canDrag == true)
        {
            transform.Translate(eventData.delta);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag == true) {
            canDrag = false;
            _decoManager.decoTransforms.Add(transform);
            _manager.UnregisterDraggedObject(this);
        }
    }
}
