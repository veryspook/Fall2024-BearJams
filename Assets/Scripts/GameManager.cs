using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public const int MAX_CUSTOMERS = 5;
	
	public static GameManager instance;
	public OrderManager orderManager;

	public GameObject frontDesk;
	public GameObject layToRest;
	public GameObject burn;
	public GameObject adorn;

	public GameObject currentStation;
	[SerializeField] GameObject bottomBar;
	private Animator animator;
	public TextMeshProUGUI customersInLineText;
	public Image newCustomerClock;
	public LifeManager lifeManager;
	public ResultsManager resultsManager;
	public Canvas gameOver;
	public TextMeshProUGUI gameOverScoreText;
	public TextMeshProUGUI gameOverCustomersText;

	private float nextCustomerTime;
	private float customerCooldown = 30;
	private float gameDuration = 0;
	public float score = 0;

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
		StartCoroutine(CustomerFlow());
	}

	private void Update()
	{
		customersInLineText.text = frontDesk.GetComponent<IStation>().customerQueue.Count + "/" + MAX_CUSTOMERS + " Customers in Line";
		gameDuration += Time.deltaTime;
		nextCustomerTime -= Time.deltaTime;
		newCustomerClock.fillAmount = Mathf.Clamp01(nextCustomerTime / customerCooldown);
	}

	private IEnumerator CustomerFlow()
	{

		while (true)
		{
			customerCooldown = Mathf.Max(7, 30 - Mathf.Sqrt(gameDuration));
			nextCustomerTime = customerCooldown;
			if (frontDesk.GetComponent<IStation>().customerQueue.Count < MAX_CUSTOMERS)
				frontDesk.GetComponent<IStation>().customerQueue.Add(Customer.Generate());
			else
				lifeManager.LoseLife();

			yield return new WaitUntil(() => nextCustomerTime <= 0);
		}
	}

	public void GameOver()
	{
		currentStation.SetActive(false);
		gameOver.enabled = true;
		gameOverScoreText.text = "Final Score: <color=red>" + score;
		gameOverCustomersText.text = "Orders Completed: <color=red>" + orderManager.orderNum;
	}

	private GameObject nextStation;
	public void ChangeStation(GameObject station)
	{
		nextStation = station;
		animator.SetTrigger("Fade");
	}

	public void HideBottomBar()
	{
		bottomBar.SetActive(false);
		resultsManager.gameObject.SetActive(false);
	}

	public void ShowBottomBar()
	{
		bottomBar.SetActive(true);
		resultsManager.gameObject.SetActive(true);
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
		nextStation.GetComponent<IStation>().Enter();
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