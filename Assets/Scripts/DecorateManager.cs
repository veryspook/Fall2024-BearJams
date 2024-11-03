using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DecorateManager : MonoBehaviour
{
    public List<Transform> decoTransforms = new List<Transform>();
    public List<Transform> goalTransforms = new List<Transform>();
    public List<GameObject> urns = new List<GameObject>();
    public List<Sprite> urnSprites = new List<Sprite>();

    public Customer currentCustomer;
    public AshPouring ashBag;
    public int currentUrn;
    public Canvas selectUrnUI;
    public Image selectUrnImage;

	private void Start()
	{
        Enter(new Customer());
	}

	public bool Enter(Customer c)
    {
        if (currentCustomer == null)
        {
            currentCustomer = c;
            selectUrnImage.enabled = true;
            ashBag.ashRemaining = currentCustomer.carcassWeight;
            return true;
        }
        else
            return false;
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
        selectUrnUI.enabled = false;
    }

	public void CheckAccuracy() {
        List<Transform> goals = goalTransforms;
        float totalAccuracy = 0f;

        for (int i = 0; i < decoTransforms.Count; i++) {
            if (goals.Count == 1) {
                totalAccuracy = Mathf.Abs(Mathf.Abs(goals[0].position.x) - Mathf.Abs(decoTransforms[i].position.x)) + Mathf.Abs(Mathf.Abs(goals[0].position.y) - Mathf.Abs(decoTransforms[i].position.y));
                //penalty for additional items
                totalAccuracy += 200f * (decoTransforms.Count - i - 1);
                Debug.Log(totalAccuracy);
                break;
            }
            float min = Mathf.Abs(Mathf.Abs(goals[0].position.x) - Mathf.Abs(decoTransforms[i].position.x)) + Mathf.Abs(Mathf.Abs(goals[0].position.y) - Mathf.Abs(decoTransforms[i].position.y)); 
            Transform best = goals[0];
            for (int j = 1; j < goals.Count; j++){
                float accuracy = Mathf.Abs(Mathf.Abs(goals[j].position.x) - Mathf.Abs(decoTransforms[i].position.x)) + Mathf.Abs(Mathf.Abs(goals[j].position.y) - Mathf.Abs(decoTransforms[i].position.y));
                if (accuracy < min) {
                    min = accuracy;
                    best = goals[j];
                }
            }
            
            totalAccuracy += min;
            goals.Remove(best);
        }
        Debug.Log(totalAccuracy);
    }
}
