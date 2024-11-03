using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furnace : MonoBehaviour
{
    public Customer customer;
    private float maxTime = 30f;
    [SerializeField]
    private float readyTime = 15f;
    [SerializeField]
    private float burnTime = 23f;
    [SerializeField] private Slider slider;
    private float currTime = 0f;
    [SerializeField] private GameObject fire;
    public Animator coffinAnim;
    public IEnumerator timer;
    public enum Status {
        Empty,
        Cooking
    }
    public Status status = Status.Empty;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator Timer() {
        while (currTime < maxTime) {
            yield return new WaitForSeconds(0.1f);
            currTime += 0.1f * customer.carcassWeight;
            slider.value = currTime / maxTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (customer == null) {
            status = Status.Empty;
            fire.SetActive(false);
        } else {
            status = Status.Cooking;
            fire.SetActive(true);
        }
    }

    public void PutInBody() {
        coffinAnim.SetTrigger("Move");
        timer = Timer();
        StartCoroutine(timer);

    }
    public void TakeOutBody() {
        StopCoroutine(timer);
        currTime = 0f;

        if (readyTime <= currTime && currTime <= burnTime) {
            customer.cookScore = 1;
        } else {
            if (readyTime - currTime > 0) {
                customer.cookScore = 1 - Mathf.Abs(currTime - readyTime)/readyTime; 
            } else {
                customer.cookScore = 1 - (currTime - burnTime)/currTime;
            }
        }
    }
}
