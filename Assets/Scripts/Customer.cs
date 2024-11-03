using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer
{
    public const int DECORATION_COUNT = 3;

    public GameManager.Urns desiredUrn;
    [Range(0f, 1f)]
    public float carcassWeight;
    public GameManager.Flowers[] desiredFlowers = new GameManager.Flowers[DECORATION_COUNT];

    public Customer(float weight, GameManager.Urns _desiredUrn, GameManager.Flowers[] flowers)
    {
        carcassWeight = weight;
        desiredUrn = _desiredUrn;
        desiredFlowers = flowers;
    }
    
    public string ToString()
    {
        return desiredFlowers.ToString() + " " + desiredUrn.ToString() + " " + carcassWeight; 
    }

    public static Customer Generate()
    {
        GameManager.Flowers[] flowers = new GameManager.Flowers[DECORATION_COUNT];
        GameManager.Flowers sides = (GameManager.Flowers) Random.Range(0, 4);
        flowers[1] = (GameManager.Flowers) Random.Range(0, 4);
        flowers[0] = sides; flowers[2] = sides;
        return new Customer(Random.Range(0.5f, 1), (GameManager.Urns)Random.Range(0, 3), flowers);
    }

    public float layToRestScore;
    public float cookScore;
    public float pourScore;
    public float decorScore;
}
