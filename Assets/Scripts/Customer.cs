using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer
{
    public GameManager.Urns desiredUrn;
    public float carcassWeight;
    public GameManager.Flowers[] desiredFlowers = new GameManager.Flowers[3];

    public float layToRestScore;
    public float cookScore;
    public float pourScore;
    public float decorScore;
}
