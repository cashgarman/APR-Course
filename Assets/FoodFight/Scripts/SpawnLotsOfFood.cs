using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLotsOfFood : MonoBehaviour
{
    private int numFoods;
    public Foodstuff foodPrefab;

    void Update()
    {
        numFoods++;
        Instantiate(foodPrefab, transform.position, transform.rotation);
        Debug.Log($"Number of food: {numFoods}");
    }
}
