// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Used as a middle ground when setting what enemies are allowed to spawn. Main purpose was to set the toggles to what they were previously set to after a game resets.
// Can be removed but will need to be replaced by SpawnManager.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsController : MonoBehaviour
{
    private GameObject spawnManager;
    public Toggle pelicanTGLE;
    public Toggle sharkTGLE;
    public Toggle boulderTGLE;
    public Toggle urchinTGLE;
    public Toggle turtleTGLE;
    public Toggle tentacleTGLE;
    public Toggle seagullTGLE;
    public Slider difficultySlider;
    public Slider masterVolumeSlider;
    public AudioMixer masterAudio;

    private bool canPelicanSpawn;
    private bool canSharkSpawn;
    private bool canBoulderSpawn;
    private bool canUrchinSpawn;
    private bool canTurtleSpawn;
    private bool canTentacleSpawn;
    private bool canSeagullSpawn;

    // Use this for initialization
    void Start ()
    {
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
    }
	
	// Update is called once per frame
	void Update ()
    {
        spawnManager.GetComponent<SpawnManager>().ChangeDifficulty((int)difficultySlider.value);
        masterAudio.SetFloat("MyExposedParam", masterVolumeSlider.value);
    }

    public void SetIfEnemyCanSpawn(int EnemyNumber)
    {
        if (EnemyNumber == 1)
        {
            canPelicanSpawn = !canPelicanSpawn;
        }
        else if (EnemyNumber == 2)
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

        spawnManager.GetComponent<SpawnManager>().SetIfEnemyCanSpawn(EnemyNumber);
    }
}
