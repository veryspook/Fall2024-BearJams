using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furnace : MonoBehaviour
{
    public Customer customer;
    public float maxTime = 60f;
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
    public bool taken = false;
    public bool cooking = false;
    AudioSource fireAudio;

	private void Awake()
	{
		fireAudio = GetComponent<AudioSource>();
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
        }
	}

	private void Update()
	{
        if (cooking)
        {
            currTime += Time.deltaTime * (1 / customer.carcassWeight);
            slider.value = currTime / maxTime;
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
        fireAudio.Play();
	}

    public void TakeOutBody() {
        coffinAnim.SetTrigger("Remove");
        trayAnim.SetTrigger("Remove");
        fire.SetActive(false);
        taken = false;
        cooking = false;
        fireAudio.Pause();
		slider.gameObject.SetActive(false);
		insertButton.gameObject.SetActive(false);
		extractButton.gameObject.SetActive(false);

        Debug.Log(readyTime + " " + currTime + " " + burnTime);
		if (readyTime <= currTime && currTime <= burnTime) {
            customer.cookScore = 1;
        } else {
            if (readyTime > currTime) {
                customer.cookScore = 1 - (readyTime - currTime)/readyTime; 
            } else {
                customer.cookScore = Mathf.Max(0, maxTime - currTime)/(maxTime - burnTime);
            }
        }
		currTime = 0f;
        GameManager.instance.adorn.GetComponent<IStation>().Enqueue(customer);
        customer = null;
	}
}
