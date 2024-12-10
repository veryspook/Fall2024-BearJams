using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSource : MonoBehaviour
{
	public Urn urn;
	private SpriteRenderer sprite;
	public GameObject flowerPrefab;
	private GameObject prefabInstance;
	private bool dragging = false;
	private bool _draggable = false;
	public bool draggable {
		get => _draggable; set {
			SpriteVisibility(value);
			_draggable = value;
		}
	}

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
	}

	private void SpriteVisibility(bool v)
	{
		if (v)
		{
			sprite.color = Color.white;
		}
        else
        {
             sprite.color = new Color(1,1,1,0.5f);
        }
    }

	// Update is called once per frame
	void Update()
    {
		if (draggable && Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit && hit.collider.gameObject == gameObject)
			{
				dragging = true;
				prefabInstance = Instantiate(flowerPrefab);
				AudioManager.instance.PlaySound("Flower Pick");
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			dragging = false;
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (prefabInstance && urn && hit && hit.collider && hit.collider.transform.parent == urn.gameObject.transform)
			{
				prefabInstance.transform.parent = hit.collider.gameObject.transform;
				AudioManager.instance.PlaySound("Flower Place");
				urn.decorations.Add(prefabInstance);
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
