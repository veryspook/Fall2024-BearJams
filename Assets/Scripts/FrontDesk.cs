using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDesk : MonoBehaviour, IStation
{
    private Animator animator;

	public Customer currentCustomer { get; set; }
	public List<Customer> customerQueue { get; set; } = new List<Customer>();

	public Canvas startOrderUI;
	public float urnScale = 0.5f;
	public float weightScale = 2.5f;
	public float flowerScale = 2.5f;
	public SpriteRenderer speechSprite;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		Enqueue(Customer.Generate());
	}

	private void OnEnable()
	{
		if (customerQueue.Count > 0)
			startOrderUI.gameObject.SetActive(true);
		else
			startOrderUI.gameObject.SetActive(false);
	}

	public void Enter()
	{
		StartCoroutine(CustomerOrder());
	}

	private IEnumerator CustomerOrder()
	{
		startOrderUI.gameObject.SetActive(false);
		animator.SetTrigger("Walk");
		currentCustomer = customerQueue[0];
		customerQueue.RemoveAt(0);
		yield return new WaitForSeconds(1.5f);
		speechSprite.sprite = GameManager.instance.WeightToSprite(currentCustomer.carcassWeight);
		speechSprite.transform.localScale = Vector3.one * weightScale;
		animator.SetTrigger("Speech");
		yield return new WaitForSeconds(0.9f);
		speechSprite.sprite = GameManager.instance.urnSprites[(int)currentCustomer.desiredUrn];
		speechSprite.transform.localScale = Vector3.one * urnScale;
		animator.SetTrigger("Speech");
		yield return new WaitForSeconds(0.9f);
		for (int i = 0; i < Customer.DECORATION_COUNT; i++)
		{
			speechSprite.sprite = GameManager.instance.flowerSprites[(int)currentCustomer.desiredFlowers[i]];
			speechSprite.transform.localScale = Vector3.one * flowerScale;
			animator.SetTrigger("Speech");
			yield return new WaitForSeconds(0.9f);
		}
		GameManager.instance.orderManager.AddOrder(currentCustomer);
		GameManager.instance.layToRest.GetComponent<IStation>().Enqueue(currentCustomer);
		yield return new WaitForSeconds(0.3f);
		GameManager.instance.ChangeStation(GameManager.instance.layToRest);
	}

	public void Enqueue(Customer customer)
	{
		customerQueue.Add(customer);
	}
}
