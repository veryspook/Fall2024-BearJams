using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    public Customer customerToDisplay;
    [SerializeField] private TextMeshProUGUI layToRestScore;
    [SerializeField] private TextMeshProUGUI cookScore;
    [SerializeField] private TextMeshProUGUI pourScore;
    [SerializeField] private TextMeshProUGUI decorScore;
    [SerializeField] private Animator anim;

    public void DisplayResults(Customer c) {
        layToRestScore.text = c.layToRestScore.ToString("0%");
        cookScore.text = c.cookScore.ToString("0%");
        pourScore.text = c.pourScore.ToString("0%");
        decorScore.text = c.decorScore.ToString("0%");
    }

    //public void Test() {
    //    GameManager.Flowers[] flowers = new GameManager.Flowers[1];
    //    Customer c = new Customer(0, GameManager.Urns.Black, flowers);
    //    c.cookScore = 0.3f;
    //    c.layToRestScore = 0.5324f;
    //    c.pourScore = 0.4f;
    //    c.decorScore = 1f;
    //    customerToDisplay = c;

    //    gameObject.SetActive(true);


    //}

}
