using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urn : MonoBehaviour
{
    public float funnelWidth;
    public Animator animator;
	public ParticleSystem spillParticles;
	public List<Transform> targetPoints;
	public List<GameObject> decorations;

	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
		spillParticles = GetComponent<ParticleSystem>();
	}
}
