using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class Order : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private const float lerpSpeed = 20f;

	public Customer customer;
	
	public Vector3 desiredPos = Vector3.zero;
	public float desiredScale = 1f;
	public OrderManager manager;

	public Image weightClass;
	public Image urn;
	public TextMeshProUGUI orderNum;
	public Image[] flowerSprites;

	private bool dragging;

	private void Start()
	{
		desiredPos = transform.localPosition;
		desiredScale = manager.ticketFocusScale;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		dragging = true;
		desiredScale = manager.ticketFocusScale;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector3 d = eventData.delta;
		Vector3 ls = manager.transform.lossyScale;
		//desiredPos += (Vector3) eventData.delta * (Vector3.one - transform.lossyScale);
		desiredPos += new Vector3(d.x / ls.x, d.y / ls.y, 0);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		dragging = false;
		desiredScale = manager.ticketRopeScale;
		float managerScale = manager.transform.lossyScale.x;
		Order p = manager.pinned;
		if (manager.pinHitbox.rect.Contains(transform.localPosition - manager.pinHitbox.localPosition))
		{
			if (p)
			{
				p.desiredPos = new Vector3((Screen.width / 2 - 450) / managerScale, manager.ticketRopeHeight, 0);
				p.desiredScale = manager.ticketRopeScale;
			}
			manager.pinned = this;
			desiredPos = manager.ticketPinPosition;
			desiredScale = manager.ticketFocusScale;
		}
		else
		{
			if (p == this)
			{
				manager.pinned = null;
			}
			desiredPos = new Vector3(Mathf.Clamp(desiredPos.x, -Screen.width / 2 / managerScale, (Screen.width / 2 - 450) / managerScale), manager.ticketRopeHeight, 0);
		}
	}

	void Update()
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPos, lerpSpeed * Time.deltaTime);
		transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, desiredScale, lerpSpeed * Time.deltaTime);
	}
}
