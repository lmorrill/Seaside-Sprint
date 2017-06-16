// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: To control the amount of enemies allowed to spawn and which enemies are allowed to be spawned. 
// When spawning an enemy will need to call the gameobject with the spawn manager on it, when calling it check if enemies can spawn and if the proper enemy is allowed to spawn.
// After spawning an enemy call the spawn manager SpawnEnemy method and when destroying an enemie cal the DestroyEnemy Method

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int currentEnemiesOnScreen = 0;
    public int maxEnemiesAllowed = 2;
    public bool canMoreEnemiesSpawn = true;
    public bool canPelicanSpawn;
    public bool canSharkSpawn;
    public bool canBoulderSpawn;
    public bool canUrchinSpawn;
    public bool canTurtleSpawn;
    public bool canTentacleSpawn;
    public bool canSeagullSpawn;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(currentEnemiesOnScreen >= maxEnemiesAllowed)
        {
            canMoreEnemiesSpawn = false;
        }
        
        if(currentEnemiesOnScreen < maxEnemiesAllowed)
        {
            canMoreEnemiesSpawn = true;
        }	
	}

    public void SetIfEnemyCanSpawn(int EnemyNumber)
    {
        if(EnemyNumber == 1)
        {
            canPelicanSpawn = !canPelicanSpawn;
        }
        else if(EnemyNumber == 2)
        {
            canSharkSpawn = !canSharkSpawn;
        }
        else if (EnemyNumber == 3)
        {
            canBoulderSpawn = !canBoulderSpawn;
        }
        else if (EnemyNumber == 4)
        {
            canUrchinSpawn = !canUrchinSpawn;
        }
        else if (EnemyNumber == 5)
        {
            canTurtleSpawn = !canTurtleSpawn;
        }
        else if (EnemyNumber == 6)
        {
            canTentacleSpawn = !canTentacleSpawn;
        }
        else if (EnemyNumber == 7)
        {
            canSeagullSpawn = !canSeagullSpawn;
        }
    }

    public void SpawnEnemy()
    {
        currentEnemiesOnScreen++;
    }

    public void DestroyEnemy()
    {
        currentEnemiesOnScreen--;
    }

    public void ResetValues()
    {
        currentEnemiesOnScreen = 0;
        canPelicanSpawn = true;
        canSharkSpawn = true;
        canBoulderSpawn = true;
        canUrchinSpawn = true;
        canTurtleSpawn = true;
        canTentacleSpawn = true;
        canSeagullSpawn = true;
    }

    public void ChangeDifficulty(int Difficulty)
    {
        maxEnemiesAllowed = Difficulty;
    }
}
