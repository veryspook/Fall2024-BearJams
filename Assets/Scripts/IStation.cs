using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStation
{
    public Customer currentCustomer { get; set; }
    public List<Customer> customerQueue { get; set; }

    /// <summary>
    /// Enter the first customer in the queue
    /// </summary>
    public void Enter();
    public void Enqueue(Customer customer);
}
