using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furnace : MonoBehaviour
{
    public Customer customer;
    private float maxTime = 60f;
    [SerializeField]
    private float readyTime = 40f;
    [SerializeField]
    private float burnTime = 50f;
    [SerializeField] private Slider slider;
    [SerializeField] private Button insertButton;
    [SerializeField] private Button extractButton;
    private float currTime = 0f;
    [SerializeField] private GameObject fire;
    public Animator coffinAnim;
    public Animator trayAnim;
    public Coroutine timer;
    public bool taken = false;
    public bool cooking = false;

	IEnumerator Timer() {
        while (currTime < maxTime) {
            yield return new WaitForEndOfFrame();
            currTime += Time.deltaTime * (1 / customer.carcassWeight);
            slider.value = currTime / maxTime;
        }
    }

    float timeOfDisable;
	private void OnDisable()
	{
        timeOfDisable = Time.fixedTime;
	}
	private void OnEnable()
	{
		if (cooking)
        {
            currTime += Time.fixedTime - timeOfDisable;
            timer = StartCoroutine(Timer());
        }
	}

    public void SetBody(Customer c)
    {
        taken = true;
        coffinAnim.SetTrigger("Enter");
        slider.gameObject.SetActive(false);
        insertButton.gameObject.SetActive(true);
        fire.SetActive(false);
        extractButton.gameObject.SetActive(false);
		customer = c;
    }

    public void PutInBody() {
        coffinAnim.SetTrigger("Insert");
        fire.SetActive(true);
        slider.gameObject.SetActive(true);
        insertButton.gameObject.SetActive(false);
        extractButton.gameObject.SetActive(true);
        cooking = true;
        timer = StartCoroutine(Timer());
	}

    public void TakeOutBody() {
        coffinAnim.SetTrigger("Remove");
        trayAnim.SetTrigger("Remove");
        fire.SetActive(false);
        StopCoroutine(timer);
        taken = false;
        cooking = false;

		slider.gameObject.SetActive(false);
		insertButton.gameObject.SetActive(false);
		extractButton.gameObject.SetActive(false);

		if (readyTime <= currTime && currTime <= burnTime) {
            customer.cookScore = 1;
        } else {
            if (readyTime - currTime > 0) {
                customer.cookScore = 1 - Mathf.Abs(currTime - readyTime)/readyTime; 
            } else {
                customer.cookScore = 1 - (currTime - burnTime)/currTime;
            }
        }
		currTime = 0f;
        GameManager.instance.adorn.GetComponent<IStation>().Enqueue(customer);
        customer = null;
	}
}
