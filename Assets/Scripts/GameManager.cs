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
		Green, Blue, Black
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