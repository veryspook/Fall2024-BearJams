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

	public void OnBeginDrag(PointerEventData eventData)
	{
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
		desiredScale = manager.ticketRopeScale;
		float scale = transform.lossyScale.x;
		Order p = manager.pinned;
		if (manager.pinHitbox.rect.Contains(transform.localPosition - manager.pinHitbox.localPosition))
		{
			manager.SetPinned(this);
		}
		else
		{
			if (p == this)
			{
				manager.pinned = null;
			}
			desiredPos = new Vector3(Mathf.Clamp(desiredPos.x, -(manager.transform as RectTransform).rect.width / 2, manager.pinHitbox.localPosition.x - manager.pinHitbox.rect.width / 2), manager.ticketRopeHeight, 0);
		}
	}

	void Update()
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPos, lerpSpeed * Time.deltaTime);
		transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, desiredScale, lerpSpeed * Time.deltaTime);
	}
}
