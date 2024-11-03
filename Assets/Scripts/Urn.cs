using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urn : MonoBehaviour
{
    // don't ask
    public float funnelWidth;
    public Animator animator;
	public ParticleSystem spillParticles;

	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
		spillParticles = GetComponent<ParticleSystem>();
	}
}
