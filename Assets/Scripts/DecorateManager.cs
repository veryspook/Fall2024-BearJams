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

    public Customer currentCustomer { get; set; }
    public List<Customer> customerQueue { get; set; } = new List<Customer>();
    public AshPouring ashBag;
    public int currentUrn;
    public Urn urn;
    public Canvas selectUrnUI;
    public Image selectUrnImage;
    public Canvas submitButton;
    public ResultsManager resultsManager;
    public LifeManager lifeManager;

    private enum States
    {
        Empty,
        Selecting,
        Pouring,
        Decorating
    }
    private States currentState = States.Empty;

	public void Enter()
    {
        if (currentCustomer == null && customerQueue.Count > 0)
        {
            currentState = States.Selecting;
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
        } else
        {
			selectUrnUI.enabled = currentState == States.Selecting;
		}
	}

    public void Finish()
    {
        OrderManager om = GameManager.instance.orderManager;
        if (om.pinned)
        {
            currentState = States.Empty;
            Customer c = om.pinned.customer;
            c.decorScore = GetScore(c);
            urn.HideTargets();
            urn.animator.ResetTrigger("Enter");
			urn.animator.SetTrigger("Finish");
            c.pourScore = Mathf.Clamp01(1 - ashBag.amountSpilled * 4 / c.carcassWeight);
            submitButton.enabled = false;
            Destroy(om.pinned.gameObject);
            om.pinned = null;
            float sum = c.layToRestScore + currentCustomer.cookScore + c.pourScore + c.decorScore;
            if (sum / 4 < 0.7)
            {
                AudioManager.instance.PlaySound("Urn Complete Fail");
                Invoke(nameof(LoseLife), 3);
            } else
            {
				AudioManager.instance.PlaySound("Urn Complete Success");
			}
			GameManager.instance.score += (int)Mathf.Ceil(sum * 100);
            resultsManager.DisplayResults(c);
            currentCustomer = null;
        }
	}

    private void LoseLife()
    {
        lifeManager.LoseLife();
	}

	public void Enqueue(Customer customer)
	{
		customerQueue.Add(customer);
	}

	private void OnEnable()
	{
		switch (currentState)
        {
            case States.Selecting:
                break;
            case States.Pouring:
                StartCoroutine(PourCoroutine());
                break;
            case States.Decorating:
                DecoratingEnter();
                break;
            case States.Empty:
				break;
            default:
                break;
        }
	}

	public IEnumerator SelectCoroutine()
    {
		selectUrnUI.enabled = false;
		urn = urns[currentUrn].GetComponent<Urn>();
        urn.HideTargets();
        DestroyDecorations();
		urn.gameObject.SetActive(true);
        urn.animator.SetTrigger("Enter");
		urn.animator.SetBool("Open", true);
        foreach (FlowerSource fs in flowerSources)
            fs.urn = urn;
		ashBag.funnelWidth = urn.funnelWidth;
		ashBag.spillParticles = urn.spillParticles;
        currentState = States.Pouring;
        yield return new WaitForSeconds(0.1f);
		ashBag.gameObject.SetActive(true);
        StartCoroutine(PourCoroutine());
	}

    public IEnumerator PourCoroutine()
    {
		yield return new WaitUntil(() => ashBag.ashRemaining <= 0);
		urn.animator.SetBool("Open", false);
        Invoke(nameof(PlayUrnCloseSound), 0.25f);
        currentState = States.Decorating;
		yield return new WaitForSeconds(0.3f);
		DecoratingEnter();
	}

    private void PlayUrnCloseSound()
    {
        AudioManager.instance.PlaySound("Urn Lid Close");
    }

    public void DecoratingEnter()
    {
		ashBag.gameObject.SetActive(false);
		urn.ShowTargets();
		submitButton.enabled = true;
		foreach (FlowerSource fs in flowerSources)
			fs.draggable = true;
	}

    private void DestroyDecorations()
    {
		foreach (GameObject d in urn.decorations)
        {
			Destroy(d);
		}
        urn.decorations.Clear();
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
		StartCoroutine(SelectCoroutine());
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
        Debug.Log(finalScore);
        return Mathf.Clamp01(finalScore);
    }

    public float CheckAccuracy(Customer c)
    {
        List<Transform> goals = new List<Transform>(urn.targetPoints);
        float totalAccuracy = 0f;
        float extraItems = urn.decorations.Count - goals.Count;

        if (urn.decorations.Count == 0)
        {
            return 0;
        }

        foreach (GameObject deco in urn.decorations)
        {
            if (deco && goals.Count > 0)
            {
                Transform decoTransform = deco.transform;
                // float min = Mathf.Abs(Mathf.Abs(goals[0].position.x) - Mathf.Abs(decoTransform.position.x)) + Mathf.Abs(Mathf.Abs(goals[0].position.y) - Mathf.Abs(decoTransform.position.y));
                Transform best = goals[0];
                float min = (best.position - decoTransform.position).magnitude;
                int bestI = 0;
                for (int i = 0; i < goals.Count; i++)
                {
                    // float accuracy = Mathf.Abs(Mathf.Abs(goals[i].position.x) - Mathf.Abs(decoTransform.position.x)) + Mathf.Abs(Mathf.Abs(goals[i].position.y) - Mathf.Abs(decoTransform.position.y));
                    float accuracy = (goals[i].position - decoTransform.position).magnitude;
					// Debug.Log($"{min} {accuracy}");
					if (accuracy < min)
                    {
                        min = accuracy;
                        best = goals[i];
                        bestI = i;
                    }
                }

                Debug.Log($"Flower {deco.GetComponent<Decoration>().type} best at {bestI}");

                if (deco.GetComponent<Decoration>().type != c.desiredFlowers[bestI])
                {
                    Debug.Log($"Wrong Flower! Expected {c.desiredFlowers[bestI]}, but got {deco.GetComponent<Decoration>().type} at position {bestI}");
                    Debug.Log(c.desiredFlowers);
					totalAccuracy += 3;
				}
				totalAccuracy += Mathf.Max(0, min - 0.05f);
            }

		}
        float finalScore = Mathf.Clamp01((5 - (totalAccuracy / 2)) / 5);
        if (extraItems < 0)
        {
            finalScore *= ((float) urn.decorations.Count) / Customer.DECORATION_COUNT;
        } else if (extraItems > 0)
        {
            finalScore *= ((float) Customer.DECORATION_COUNT) / urn.decorations.Count;
		}
        Debug.Log($"Check Accuracy: {finalScore}, Total Accuracy: {totalAccuracy}");
		return finalScore;
    }

}
