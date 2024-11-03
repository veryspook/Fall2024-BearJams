using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Order : DragObject
{
    public Customer customer;
    public TextMeshProUGUI orderNum;
    public Image weightClass;
    public Image urn;
    public List<GameObject> flowerIcons;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        _manager.RegisterDraggedObject(this);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (_manager.IsWithinBounds(eventData.delta))
        {
            transform.Translate(eventData.delta);
        }
        Debug.Log("not within bounds");
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        _manager.UnregisterDraggedObject(this);
    }

    
}
