using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private bool canDrag = true;
    public DragManager _manager = null;

    private DecorateManager _decoManager = null;

    public Vector2 _centerPoint;
    public Vector2 _worldCenterPoint => transform.TransformPoint(_centerPoint);

    private void Awake()
    {

        _manager = GetComponentInParent<DragManager>();
        _decoManager = GetComponentInParent<DecorateManager>();
        _centerPoint = (transform as RectTransform).rect.center;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(canDrag == true) {
            _manager.RegisterDraggedObject(this);
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (_manager.IsWithinBounds(_worldCenterPoint + eventData.delta)&& canDrag == true)
        {
            transform.Translate(eventData.delta);
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag == true) {
            canDrag = false;
            // _decoManager.decoTransforms.Add(transform);
            _manager.UnregisterDraggedObject(this);
        }
    }
}
