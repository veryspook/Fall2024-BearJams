using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayToRest : MonoBehaviour, IStation
{
	public Customer currentCustomer { get; set; }
	public List<Customer> customerQueue { get; set; } = new List<Customer>();

	[SerializeField] private GameObject corpsePrefab;
	[SerializeField] private float corpseScale = 1.5f;
	[SerializeField] private GameObject coffin;
	[SerializeField] private Vector2 startPosition;

	public void Enqueue(Customer customer)
	{
		customerQueue.Add(customer);
		
	}

	public void Enter()
	{
		if (currentCustomer == null && customerQueue.Count > 0)
		{
			currentCustomer = customerQueue[0];
			DragCorpse corpse = Instantiate(corpsePrefab, (Vector3) startPosition, Quaternion.identity, coffin.transform).GetComponent<DragCorpse>();
			coffin.SetActive(true);
			corpse.manager = this;
			corpse.Enter(currentCustomer);
			customerQueue.RemoveAt(0);
		}
	}
}
