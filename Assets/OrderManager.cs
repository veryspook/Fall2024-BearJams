using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : DragManager
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

        // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < urnIcons.Count; i++) {
            urnIconDict.Add(urns[i], urnIcons[i]);
        }
        for (int j = 0; j < flowerIcons.Count; j++) {
            flowerIconDict.Add(flowers[j], flowerIcons[j]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestOrder() {

    }
    public void AddOrder(Customer customer) {
        GameObject orderObj = Instantiate(orderPrefab);
        Order order = orderObj.GetComponent<Order>();
        order.customer = customer;

        order.orderNum.text = orderNum.ToString("#0000");
        orderNum++;

        if (0f <= customer.carcassWeight && customer.carcassWeight < 0.3f) {
            order.weightClass.sprite = weightIcons[0];
        } else if (0.3f <= customer.carcassWeight && customer.carcassWeight < 0.7f) {
            order.weightClass.sprite = weightIcons[1];
        } else {
            order.weightClass.sprite = weightIcons[2];
        }

        order.urn.sprite = urnIconDict[customer.desiredUrn];
        foreach (GameObject flower in order.flowerIcons) {
            flower.SetActive(false);
        }
        for (int i = 0; i < customer.desiredFlowers.Length; i++) {
            order.flowerIcons[i].SetActive(true);
            flowerIcons[i].GetComponent<Image>().sprite = flowerIconDict[customer.desiredFlowers[i]];
        }
        
    }
}
