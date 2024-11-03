using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public List<Order> orders = new List<Order>();
    public Sprite[] weightIcons = new Sprite[3];
    public List<Sprite> urnIcons;
    public List<GameManager.Urns> urns;
    public List<Sprite> flowerIcons;
    public List<GameManager.Flowers> flowers;
    public Dictionary<GameManager.Urns, Sprite> urnIconDict = new Dictionary<GameManager.Urns, Sprite>();
    public Dictionary<GameManager.Flowers, Sprite> flowerIconDict = new Dictionary<GameManager.Flowers, Sprite>();
    public int orderNum = 1;
    public GameObject orderPrefab;

    public float ticketRopeHeight = 352;
    public float ticketRopeScale = 0.3f;
    public float ticketFocusScale = 0.7f;
    public Vector2 ticketPinPosition;
    public RectTransform pinHitbox;
    public Order pinned;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddOrder(Customer customer)
    {
		GameObject orderObj = Instantiate(orderPrefab);
        orderObj.transform.parent = transform;
		Order order = orderObj.GetComponent<Order>();
        SetPinned(order);
        order.manager = this;
		order.customer = customer;

        order.orderNum.text = orderNum.ToString("#0000");
        orderNum++;

        if (0.5f <= customer.carcassWeight && customer.carcassWeight < 0.6f)
        {
            order.weightClass.sprite = weightIcons[0];
        }
        else if (0.6f <= customer.carcassWeight && customer.carcassWeight < 0.8f)
        {
            order.weightClass.sprite = weightIcons[1];
        }
        else
        {
            order.weightClass.sprite = weightIcons[2];
        }

        order.urn.sprite = urnIconDict[customer.desiredUrn];
        for (int i = 0; i < 3; i++)
        {
            order.flowerSprites[i].sprite = flowerIcons[(int) customer.desiredFlowers[i]];
        }
    }

    public void SetPinned(Order o)
    {
		if (pinned)
		{
			pinned.desiredPos = new Vector3((Screen.width / 2 - 450) / transform.lossyScale.x, ticketRopeHeight, 0);
			pinned.desiredScale = ticketRopeScale;
		}
		pinned = o;
		o.desiredPos = ticketPinPosition;
		o.desiredScale = ticketFocusScale;
	}

	public void TestOrder() {
        AddOrder(new Customer());
    }
    //public void AddOrder(Customer customer) {
    //    GameObject orderObj = Instantiate(orderPrefab);
    //    Order order = orderObj.GetComponent<Order>();
    //    order.customer = customer;

    //    order.orderNum.text = orderNum.ToString("#0000");
    //    orderNum++;

    //    if (0f <= customer.carcassWeight && customer.carcassWeight < 0.3f) {
    //        order.weightClass.sprite = weightIcons[0];
    //    } else if (0.3f <= customer.carcassWeight && customer.carcassWeight < 0.7f) {
    //        order.weightClass.sprite = weightIcons[1];
    //    } else {
    //        order.weightClass.sprite = weightIcons[2];
    //    }

    //    order.urn.sprite = urnIconDict[customer.desiredUrn];
    //    foreach (GameObject flower in order.flowerIcons) {
    //        flower.SetActive(false);
    //    }
    //    for (int i = 0; i < customer.desiredFlowers.Length; i++) {
    //        order.flowerIcons[i].SetActive(true);
    //        flowerIcons[i].GetComponent<Image>().sprite = flowerIconDict[customer.desiredFlowers[i]];
    //    }
        
    //}
}
