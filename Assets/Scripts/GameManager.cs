using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public List<Customer> waitingCustomers;
	public OrderManager orderManager;

	public GameObject frontDesk;
	public GameObject layToRest;
	public GameObject burn;
	public GameObject adorn;

	public GameObject currentStation;
	private Animator animator;

	public enum Urns
	{
		Black = 0,
		Green = 1,
		Blue = 2
	}
	public Sprite[] urnSprites;
	public enum Flowers
	{
		Blue, Pink, Red, Orange
	}
	public Sprite[] flowerSprites;
	public Sprite[] weightIcons = new Sprite[3];

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		animator = GetComponent<Animator>();
		orderManager = GetComponent<OrderManager>();
	}

	private GameObject nextStation;
	public void ChangeStation(GameObject station)
	{
		nextStation = station;
		station.GetComponent<IStation>().Enter();
		animator.SetTrigger("Fade");
	}
	
	public void ChangeStationToNumber(int num)
	{
		switch (num)
		{
			case 0:
				ChangeStation(frontDesk); break;
			case 1:
				ChangeStation(layToRest); break;
			case 2:
				ChangeStation(burn); break;
			case 3:
				ChangeStation(adorn); break;
			default:
				ChangeStation(frontDesk); break;
		}
	}

	// Used by anim event
	private void SwapStation()
	{
		currentStation.gameObject.SetActive(false);
		nextStation.SetActive(true);
		currentStation = nextStation;
	}

	public Sprite WeightToSprite(float weight)
	{
		if (0.5f <= weight && weight < 0.6f)
		{
			return weightIcons[0];
		}
		else if (0.6f <= weight && weight < 0.8f)
		{
			return weightIcons[1];
		}
		else
		{
			return weightIcons[2];
		}
	}
}