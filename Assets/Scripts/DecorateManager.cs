using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DecorateManager : MonoBehaviour, IStation
{
    public List<GameObject> urns = new List<GameObject>();
    public List<Sprite> urnSprites = new List<Sprite>();
    public List<FlowerSource> flowerSources = new List<FlowerSource>();
    public List<GameObject> decorations;

    public Customer currentCustomer { get; set; }
    public List<Customer> customerQueue { get; set; } = new List<Customer>();
    public AshPouring ashBag;
    public int currentUrn;
    public Urn urn;
    public Canvas selectUrnUI;
    public Image selectUrnImage;
    public Canvas submitButton;
    public ResultsManager resultsManager;


	public void Enter()
    {
        if (currentCustomer == null && customerQueue.Count > 0)
        {
            submitButton.enabled = false;
            currentCustomer = customerQueue[0];
            customerQueue.RemoveAt(0);
            selectUrnUI.enabled = true;
            ashBag.ashRemaining = currentCustomer.carcassWeight;
            ashBag.amountSpilled = 0;
            foreach (GameObject u in urns)
            {
                u.SetActive(false);
            }
            foreach (FlowerSource f in flowerSources)
            {
                f.draggable = false;
            }
        }
	}

    public void Finish()
    {
        OrderManager om = GameManager.instance.orderManager;
        if (om.pinned)
        {
            Customer c = om.pinned.customer;
            c.decorScore = GetScore(c);
            c.pourScore = Mathf.Clamp01(1 - ashBag.amountSpilled * 4 / c.carcassWeight);
            urn.gameObject.SetActive(false);
            submitButton.enabled = false;
            resultsManager.DisplayResults(c);
        }
	}

	public void Enqueue(Customer customer)
	{
		customerQueue.Add(customer);

	}

	public IEnumerator EnterCoroutine()
    {
		selectUrnUI.enabled = false;
		urn = urns[currentUrn].GetComponent<Urn>();
        urn.HideTargets();
        foreach (GameObject d in urn.decorations)
            Destroy(d);
		urn.gameObject.SetActive(true);
		urn.animator.SetBool("Open", true);
        foreach (FlowerSource fs in flowerSources)
            fs.urn = urn;
		ashBag.funnelWidth = urn.funnelWidth;
		ashBag.spillParticles = urn.spillParticles;
        yield return new WaitForSeconds(0.1f);
		ashBag.gameObject.SetActive(true);
		yield return new WaitUntil(() => ashBag.ashRemaining <= 0);
        urn.animator.SetBool("Open", false);
        yield return new WaitForSeconds(0.3f);
        ashBag.gameObject.SetActive(false);
        urn.ShowTargets();
        submitButton.enabled = true;
        foreach (FlowerSource fs in flowerSources)
            fs.draggable = true;
	}

    public void NextUrnSelect()
    {
        currentUrn += 1;
        if (currentUrn >= urnSprites.Count)
            currentUrn = 0;
        selectUrnImage.sprite = urnSprites[currentUrn];
    }
    
	public void PreviousUrnSelect()
	{
		currentUrn -= 1;
		if (currentUrn < 0)
			currentUrn = urnSprites.Count - 1;
		selectUrnImage.sprite = urnSprites[currentUrn];
	}

    public void UrnSelect()
    {
		StartCoroutine(EnterCoroutine());
    }

    public float GetScore(Customer c)
    {
        if (c != currentCustomer)
            return 0;
        float finalScore = 0;
        if ((GameManager.Urns) currentUrn == c.desiredUrn)
        {
            finalScore += 0.3f;
        }
        finalScore += 0.7f * CheckAccuracy(c);
        return Mathf.Clamp01(finalScore);
    }

    public float CheckAccuracy(Customer c)
    {
        List<Transform> goals = urn.targetPoints;
        float totalAccuracy = 0f;
        float extraItems = urn.decorations.Count - goals.Count;

        foreach (GameObject deco in urn.decorations)
        {
            Transform decoTransform = deco.transform;
            float min = Mathf.Abs(Mathf.Abs(goals[0].position.x) - Mathf.Abs(decoTransform.position.x)) + Mathf.Abs(Mathf.Abs(goals[0].position.y) - Mathf.Abs(decoTransform.position.y));
            Transform best = goals[0];
            int bestI = 0;
            for (int i = 0; i < goals.Count; i++)
            {
                float accuracy = Mathf.Abs(Mathf.Abs(goals[i].position.x) - Mathf.Abs(decoTransform.position.x)) + Mathf.Abs(Mathf.Abs(goals[i].position.y) - Mathf.Abs(decoTransform.position.y));
                if (accuracy < min)
                {
                    min = accuracy;
                    best = goals[i];
                    bestI = i;
                }
            }

            if (deco.GetComponent<Decoration>().type != c.desiredFlowers[bestI])
                totalAccuracy += 3;
            totalAccuracy += min;
            goals.Remove(best);
        }
        float finalScore = Mathf.Clamp01((5 - totalAccuracy) / 5);
        if (extraItems < 0)
        {
            finalScore *= urn.decorations.Count / Customer.DECORATION_COUNT;
        } else if (extraItems > 0)
        {
            finalScore *= Customer.DECORATION_COUNT / urn.decorations.Count;
		}
        return finalScore;
    }

}
