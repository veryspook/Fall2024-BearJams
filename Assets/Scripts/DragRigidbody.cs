using UnityEngine;

public class DragAll : MonoBehaviour
{
	private Transform dragging = null;
	private Vector3 offset;
	[SerializeField] private float dragSpeed = 10;
	[SerializeField] private float scrambleStrength;
	[SerializeField] private LayerMask movableLayers;
	public Collider2D coffin;

	Rigidbody2D[] rbList;

	private void Awake()
	{
		rbList = GetComponentsInChildren<Rigidbody2D>();
	}

	private void Start()
	{
		Scramble();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			// Cast our own ray.
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
												 float.PositiveInfinity, movableLayers);
			if (hit)
			{
				// If we hit, record the transform of the object we hit.
				dragging = hit.transform;
				// And record the offset.
				offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			// Stop dragging.
			dragging = null;
		}

		if (dragging != null)
		{
			// Move object, taking into account original offset.
			// dragging.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
			Rigidbody2D rb = dragging.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				rb.AddForce(dragSpeed * ((Vector2) (Camera.main.ScreenToWorldPoint(ClampedMouse()) + offset) - rb.position));
			}
		}

		Debug.Log(GetPercentCovered());
	}

	private void Scramble()
	{
		foreach (Rigidbody2D rb in rbList)
		{
			rb.AddForce(RandomForce(scrambleStrength), ForceMode2D.Impulse);
		}
	}

	private Vector3 ClampedMouse()
	{
		return new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height));
	}

	private Vector2 RandomForce(float strength)
	{
		return new Vector2(Random.Range(-strength, strength), Random.Range(-strength, strength));
	}

	public Bounds GetBoundingBox()
	{
		Bounds b = new Bounds();
		foreach (Rigidbody2D rb in rbList)
		{
			Collider2D[] cs = new Collider2D[rb.attachedColliderCount];
			rb.GetAttachedColliders(cs);
			foreach (Collider2D c in cs)
			{
				b.Encapsulate(c.bounds);
			}
		}
		return b;
	}

	public float GetPercentCovered()
	{
		Bounds b = GetBoundingBox();
		Bounds c = coffin.bounds;
		if (!b.Intersects(c))
			return 0;
		float area = b.size.x * b.size.y;
		Vector3 insideMin = c.ClosestPoint(b.min);
		Vector3 insideMax = c.ClosestPoint(b.max);
		Vector3 diff = insideMax - insideMin;
		float areaInside = diff.x * diff.y;
		return areaInside / area;
	}
}