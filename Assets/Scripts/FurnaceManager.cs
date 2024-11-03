using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceManager : MonoBehaviour, IStation
{
    public Furnace[] furnaces = new Furnace[3];

	public Customer currentCustomer { get; set; }
	public List<Customer> customerQueue { get; set; } = new List<Customer>();

	public void Enqueue(Customer customer)
	{
		customerQueue.Add(customer);
	}

	public void Enter()
	{
		Debug.Log(customerQueue);
		foreach (Furnace furnace in furnaces)
		{
			if (!furnace.cooking && customerQueue.Count > 0)
			{
				Debug.Log(customerQueue.Count);
				furnace.SetBody(customerQueue[0]);
				customerQueue.RemoveAt(0);
				if (customerQueue.Count <= 0)
					break;
			}
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
