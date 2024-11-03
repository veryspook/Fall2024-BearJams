using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public List<Customer> waitingCustomers;

	public enum Urns
	{
		Black = 0,
		Green = 1,
		Blue = 2
	}
	public enum Flowers
	{
		Blue, Pink, Red, Orange
	}

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
}