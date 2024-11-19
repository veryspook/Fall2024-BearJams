using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayToRest : MonoBehaviour, IStation
{
	public Customer currentCustomer { get; set; }
	public List<Customer> customerQueue { get; set; } = new List<Customer>();

	[SerializeField] private GameObject corpsePrefab;
	private float corpseScale;
	[SerializeField] private GameObject coffin;
	[SerializeField] private Vector2 startPosition;

	public bool reset = false;
	DragCorpse currentCorpse = null;

	private void Awake()
	{
		corpseScale = corpsePrefab.transform.localScale.x;
	}

	public void Enqueue(Customer customer)
	{
		customerQueue.Add(customer);
	}

	public void Enter()
	{
		if (reset)
		{
			coffin.GetComponent<Animator>().SetTrigger("Exit");
			reset = false;
		}
		if (currentCustomer == null && customerQueue.Count > 0)
		{
			Invoke("CreateCustomer", 0.017f);
		}
	}

	private void CreateCustomer()
	{
		currentCustomer = customerQueue[0];
		currentCorpse = Instantiate(corpsePrefab, (Vector3)startPosition, Quaternion.identity, coffin.transform).GetComponent<DragCorpse>();
		coffin.SetActive(true);
		currentCorpse.manager = this;
		currentCorpse.Enter(currentCustomer);
		customerQueue.RemoveAt(0);
	}
}
