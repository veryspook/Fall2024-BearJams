using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSource : MonoBehaviour
{
	public Urn urn;
	public GameObject flowerPrefab;
	private GameObject prefabInstance;
	private bool dragging = false;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit && hit.collider.gameObject == gameObject)
			{
				dragging = true;
				prefabInstance = Instantiate(flowerPrefab);
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			dragging = false;
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (prefabInstance && urn && hit && hit.collider && hit.collider.transform.parent == urn.gameObject.transform)
			{
				prefabInstance.transform.parent = hit.collider.gameObject.transform;
				prefabInstance = null;
			} else
			{
				Destroy(prefabInstance);
			}
		}
		if (dragging && prefabInstance)
		{
			Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			prefabInstance.transform.position = new Vector3(p.x, p.y, 0);
		}
	}
}
