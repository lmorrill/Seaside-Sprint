// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Controls the spawning of the food and sets the food to good or bad and sets the instantiated food to proper values.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject eatFoodLocation;
    public GameObject eatBadFoodLocation;
    public GameObject originalSpawnPosition;
    public GameObject followSpawnObject;
    private GameObject instantiatedFood;
    private bool readyToSpawnFood = true;
    private float respawnTimeStamp;
    private float respawnMaxTime = 3.0f;

	// Use this for initialization
	void Start ()
    {

	}

	// Update is called once per frame
	void Update ()
    {
		if(Time.time > respawnTimeStamp + respawnMaxTime && readyToSpawnFood)
        {
            SpawnFood();
        }
	}

    private void SpawnFood()
    {
        int foodQualityNum = Random.Range(0, 3);

        instantiatedFood = Instantiate(foodPrefab, transform.position, transform.rotation);
        //instantiatedFood.transform.parent = gameObject.transform;

        if (foodQualityNum == 1)
        {
            instantiatedFood.GetComponent<FoodController>().isGoodFood = true;
        }
        else if (foodQualityNum == 2)
        {
            instantiatedFood.GetComponent<FoodController>().isGoodFood = false;
        }

        instantiatedFood.GetComponent<FoodController>().spawnArea = gameObject;
        instantiatedFood.GetComponent<FoodController>().eatFoodLocation = eatFoodLocation;
        instantiatedFood.GetComponent<FoodController>().badFoodEatLocation = eatBadFoodLocation;
        instantiatedFood.GetComponent<FoodController>().followTransform = this.gameObject;

        readyToSpawnFood = false;
    }

    public void DestroyFood()
    {
        Destroy(instantiatedFood);
        //instantiatedFood.SetActive(false);
        respawnTimeStamp = Time.time;
        readyToSpawnFood = true;
        transform.position = originalSpawnPosition.transform.position;
    }
}
