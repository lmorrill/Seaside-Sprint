// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: To control the movement of the urchin enemy, this controls the touching of the urchin and the spawning of it also.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinMovement : MonoBehaviour
{
    // Spawn Control
    private GameObject spawnManager;
    private float spawnResetTimeStamp;
    private float spawnMaxTime = 3.0f;

    private Vector3 parentOriginalPosition;
    public GameObject urchinDestination;

    private float movementSpeed = 3.0f;
    private bool isUrchinShrunk = false;

    private float maxScaleSize = .65f;
    private float minScaleSize = 0.4f;

    private float beenTouchedTimeStamp;
    private float maxTimeTilNextTouch = 3.5f;

    private float shrunkenTimeStamp;
    private float maxTimeAllowedShrunken = 3.5f;

    public AudioSource extendAudio;

    private enum UrchinStates
    {
        WAITING,
        MOVING,
        SPAWNING
    }

    private UrchinStates currentState = UrchinStates.WAITING;

    // Use this for initialization
    void Start()
    {
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        parentOriginalPosition = transform.position;

        if(spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canUrchinSpawn)
        {
            currentState = UrchinStates.SPAWNING;
            spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case UrchinStates.SPAWNING:
                transform.position = parentOriginalPosition;
                currentState = UrchinStates.MOVING;
                break;

            case UrchinStates.MOVING:
                if (isUrchinShrunk && Time.time > shrunkenTimeStamp + maxTimeAllowedShrunken)
                {
                    isUrchinShrunk = false;
                    transform.localScale = new Vector3(maxScaleSize, maxScaleSize, maxScaleSize);
                    extendAudio.Play();
                }

                transform.position = Vector3.MoveTowards(transform.position, urchinDestination.transform.position, movementSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, urchinDestination.transform.position) < 1)
                {
                    //transform.position = parentOriginalPosition;
                    spawnResetTimeStamp = Time.time;
                    currentState = UrchinStates.WAITING;
                    spawnManager.GetComponent<SpawnManager>().DestroyEnemy();
                }
                break;

            case UrchinStates.WAITING:
                if (spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canUrchinSpawn &&
                    Time.time > spawnResetTimeStamp + spawnMaxTime)
                {
                    currentState = UrchinStates.SPAWNING;
                    spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
                }
                break;
        }
    }

    public void UrchinTouched()
    {
        if (Time.time > beenTouchedTimeStamp + maxTimeTilNextTouch)
        {
            if (!isUrchinShrunk)
            {
                transform.localScale = new Vector3(minScaleSize, minScaleSize, minScaleSize);
                isUrchinShrunk = true;
                shrunkenTimeStamp = Time.time;
            }
            else
            {
                transform.localScale = new Vector3(maxScaleSize, maxScaleSize, maxScaleSize);
                isUrchinShrunk = false;
            }

            beenTouchedTimeStamp = Time.time;
        }
    }
}
