// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Controls all the values of the food, all values will be set when spawned.
// When pulled out of the ground the 2D food collider will be deactivated allowing for the 3D collider to only be detected

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodController : MonoBehaviour
{
    public bool hitMaxScaleSize = false;
    public bool isGoodFood;
    public GameObject spawnArea;
    public GameObject eatFoodLocation;
    public GameObject badFoodEatLocation;
    public GameObject followTransform;

    // UI
    public GameObject badFoodTimerCanvas;
    public Image badFoodTimerImage;

    // Food
    public List<GameObject> goodFood;
    private GameObject larry;

    // Bad food values
    public List<GameObject> badFood;
    public bool isFoodBeingDragged = false;
    private bool isBadFoodShakenOff = false;
    private bool wasFoodShakenRight = false;
    private bool wasFoodShakenLeft = false;
    private float minShakeDistance = 2.0f;
    private Vector2 originalPulledOutPosition;
    private int foodShakenCount = 0;
    private int maxFoodShakenCount = 4;

    // Life values
    private float stayAliveTimeStamp;
    private float maxTimeAlive = 10.0f;

    // Collider settings
    private bool isColliderSettingsSet = false;
    public GameObject foodCollider;

    // Audio
    public AudioSource badFoodAudio;

    private enum FoodStages
    {
        GROWING,
        SHRINKING,
        GOODFOOD,
        BADFOOD
    }

    private FoodStages foodStages = FoodStages.GROWING;

	// Use this for initialization
	void Start ()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        FoodQualityCheck();
        stayAliveTimeStamp = Time.time;

        larry = GameObject.FindGameObjectWithTag("Larry");
    }
	
	// Update is called once per frame
	void Update ()
    {
		switch(foodStages)
        {
            case FoodStages.GROWING:
                transform.position = Vector3.MoveTowards(transform.position, followTransform.transform.position, 3.0f * Time.deltaTime);
                break;

            case FoodStages.SHRINKING:
                if (transform.localScale.x <= 1f)
                {
                    // do a check to find if it is bad food or good food
                    // if good food increase health
                    // if bad decrease health
                    // keep set active or change to destroy depending on whats needed

                    if(isGoodFood)
                    {
                        // Increase Health
                        larry.GetComponent<Health>().IncreaseHealth(10);
                    }

                    spawnArea.GetComponent<FoodSpawner>().DestroyFood();
                }

                break;

            case FoodStages.GOODFOOD:

                break;

            case FoodStages.BADFOOD:
                BadFoodTimerSet();

                if(isFoodBeingDragged)
                {
                    CheckFoodShakeOffStatus();
                }

                if (Time.time > stayAliveTimeStamp + maxTimeAlive && !isBadFoodShakenOff)
                {
                    larry.GetComponent<Health>().TakeDamage(10);
                }

                break;
        }

        if(Time.time > stayAliveTimeStamp + maxTimeAlive)
        {
            spawnArea.GetComponent<FoodSpawner>().DestroyFood();
        }

	}

    // Gets the quality of the food, selects if it will be good or bad food and then sets the game object to the correct image based on the quality.
    private void FoodQualityCheck()
    {
        // Do a check to find if food is good or bad.
        int goodOrBadCheck = Random.Range(1, 3);

        if (goodOrBadCheck == 1)
        {
            // Good food
            isGoodFood = true;
            // If good do a random selection of food sprites and active selected sprite.
            goodOrBadCheck = Random.Range(0, goodFood.Count);

            for (int index = 0; index < goodFood.Count; index++)
            {
                if (goodOrBadCheck != index)
                {
                    goodFood[index].SetActive(false);
                }
                else if (goodOrBadCheck == index)
                {
                    goodFood[index].SetActive(true);
                }
            }

            foreach (GameObject BadFood in badFood)
            {
                BadFood.SetActive(false);
            }
        }
        else if (goodOrBadCheck == 2)
        {
            // Bad food
            isGoodFood = false;
            // If bad do the same as good food but go through the bad food array.
            goodOrBadCheck = Random.Range(0, badFood.Count);

            for (int index = 0; index < badFood.Count; index++)
            {
                if (goodOrBadCheck != index)
                {
                    badFood[index].SetActive(false);
                }
                else if (goodOrBadCheck == index)
                {
                    badFood[index].SetActive(true);
                }
            }

            foreach (GameObject GoodFood in goodFood)
            {
                GoodFood.SetActive(false);
            }
        }
    }

    public void PullOutOfGround(Vector2 EndPosition)
    {
        if (SwipeController.Instance.IsSwiping(SwipeDirection.UP))
        {
            if(isGoodFood)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 0);
                transform.position = eatFoodLocation.transform.position;
                foodStages = FoodStages.SHRINKING;
                stayAliveTimeStamp = Time.time;
            }
            else if(!isGoodFood)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 0);
                transform.position = EndPosition;
                foodStages = FoodStages.BADFOOD;
                //isColliderSettingsSet = false;
                stayAliveTimeStamp = Time.time;
                maxTimeAlive = 2.5f;
                originalPulledOutPosition = transform.position;

                GetComponent<ObjectTouchControl>().ScaleEnabled = false;
                GetComponent<ObjectTouchControl>().DragEnabled = true;
                badFoodTimerCanvas.SetActive(true);

                badFoodAudio.Play();
            }

            foodCollider.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void BadFoodTimerSet()
    {
        float divisionAmount = maxTimeAlive * 150;
        //float divisionAmount = 0.0066666667f;
        float foodTimerLossTime = maxTimeAlive / divisionAmount;
        if (badFoodTimerImage.fillAmount > 0)
        {
            badFoodTimerImage.fillAmount -= foodTimerLossTime;
        }

    }

    private void CheckFoodShakeOffStatus()
    {
        if(transform.position.x > originalPulledOutPosition.x + minShakeDistance && !wasFoodShakenRight)
        {
            // Food has been dragged right
            wasFoodShakenRight = true;
            wasFoodShakenLeft = false;
            foodShakenCount++;
        }

        if (transform.position.x < originalPulledOutPosition.x - minShakeDistance && !wasFoodShakenLeft)
        {
            // Food has been dragged left
            wasFoodShakenRight = false;
            wasFoodShakenLeft = true;
            foodShakenCount++;
        }

        if(foodShakenCount >= maxFoodShakenCount)
        {
            isBadFoodShakenOff = true;
            spawnArea.GetComponent<FoodSpawner>().DestroyFood();
        }
    }

    //// Use for build Debugging
    //void OnGUI()
    //{
    //    GUI.color = Color.black;

    //    GUI.Label(new Rect(10, 20, 200, 20), "Shake R" + wasFoodShakenRight);
    //    GUI.Label(new Rect(10, 40, 200, 20), "Shake L" + wasFoodShakenLeft);
    //    GUI.Label(new Rect(10, 60, 200, 20), "" + foodShakenCount);
    //}
}
