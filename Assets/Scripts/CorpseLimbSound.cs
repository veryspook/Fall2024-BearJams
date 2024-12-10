using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseLimbSound : MonoBehaviour
{
    new AudioSource audio;
	public AudioClip clip;
	Rigidbody2D rb;
    public const float volumeCoefficient = 0.03f;
	public float maxVolume = 0.5f;

	private void Awake()
	{
		audio = gameObject.AddComponent<AudioSource>();
		audio.clip = clip;
		audio.volume = 0;
		audio.loop = true;
		audio.enabled = true;
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		audio.Play();
	}

	private void Update()
	{
		audio.volume = Mathf.Min(Mathf.Sqrt(rb.velocity.magnitude) * volumeCoefficient, maxVolume);
	}
}
