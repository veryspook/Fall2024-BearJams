using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class AshPouring : MonoBehaviour
{
	[SerializeField] private Transform pourPoint;
	[SerializeField] private ParticleSystem ashParticles;
	[SerializeField] private ParticleSystem spillParticles;

	private bool dragging;
	private Vector2 dragStartPosition;
	private float xPositionAtDragStart;
	private Vector2 pourPointStartPos;
	private float dragStartAngle;
	[Header("Controls")]
	[SerializeField] private float startAngle = -10;
	[SerializeField] private float rotationSensitivity = 10f;
	[SerializeField] private float movementSensitivity = 0.7f;
	[SerializeField] private float currentAngle;
	[SerializeField] private Vector2 angleBounds;
	[SerializeField] private Vector2 movementBounds;
	[SerializeField] private float ashRemaining = 0.8f;
	[Header("Ash Pouring")]
	[SerializeField] private float funnelWidth = 0.9f;
	[SerializeField] private AnimationCurve thresholdAngleForNoAsh;
	[SerializeField] private AnimationCurve drainSpeedForOverThreshold;
	[SerializeField] private AnimationCurve pourWidthOverAngle;
	[SerializeField] private AnimationCurve pourWidthOverSpeed;
	[SerializeField] private AnimationCurve pourOffsetOverAngle;
	[Header("Particles")]
	[SerializeField] private AnimationCurve pourParticleForSpeed;
	[SerializeField] private AnimationCurve pourParticleAngleForSpeed;
	[SerializeField] private AnimationCurve pourLifetimeForAngle;

	private void Start()
	{
		pourPointStartPos = pourPoint.position;
		currentAngle = startAngle;
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			dragging = true;
			dragStartPosition = GetMousePos();
			dragStartAngle = currentAngle;
			xPositionAtDragStart = pourPoint.position.x;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			dragging = false;
		}

		if (dragging)
		{
			Vector2 mousePos = GetMousePos();
			Vector2 mouseDelta = (dragStartPosition - mousePos);
			currentAngle = dragStartAngle - mouseDelta.y * rotationSensitivity;
			currentAngle = Mathf.Clamp(currentAngle, angleBounds.x , angleBounds.y);
			pourPoint.position = new Vector2(Mathf.Clamp(xPositionAtDragStart - mouseDelta.x * movementSensitivity, movementBounds.x, movementBounds.y), pourPointStartPos.y);
		}
		pourPoint.rotation = Quaternion.Euler(0, 0, -currentAngle);

		float drainSpeed = drainSpeedForOverThreshold.Evaluate(currentAngle - thresholdAngleForNoAsh.Evaluate(ashRemaining));
		if (ashRemaining <= 0)
			drainSpeed = 0;
		Vector2 pourBounds = new Vector2(
			pourOffsetOverAngle.Evaluate(currentAngle) - pourWidthOverAngle.Evaluate(currentAngle) / 2 - pourWidthOverSpeed.Evaluate(currentAngle) / 2 + pourPoint.position.x,
			pourOffsetOverAngle.Evaluate(currentAngle) + pourWidthOverAngle.Evaluate(currentAngle) / 2 + pourWidthOverSpeed.Evaluate(currentAngle) / 2 + pourPoint.position.x);
		ParticleSystem.EmissionModule em = ashParticles.emission;
		em.rateOverTime = pourParticleForSpeed.Evaluate(drainSpeed);
		ParticleSystem.MainModule m = ashParticles.main;
		m.startLifetime = pourLifetimeForAngle.Evaluate(currentAngle);
		ParticleSystem.ShapeModule s = ashParticles.shape;
		s.arc = pourParticleAngleForSpeed.Evaluate(drainSpeed);
		ashRemaining -= drainSpeed * Time.deltaTime;
		ashRemaining = Mathf.Max(ashRemaining, 0);

		Debug.DrawLine(new Vector3(-funnelWidth/2, 1, 0), new Vector3(funnelWidth/2, 1, 0), Color.cyan);
		Debug.DrawLine(new Vector3(pourBounds.x, 0.8f, 0), new Vector3(pourBounds.y, 0.8f, 0), Color.red);
	}

	private Vector2 GetMousePos()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
