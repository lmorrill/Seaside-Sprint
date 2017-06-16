using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name: Tentacle Spawn
//Author: Ted Deurloo
//Purpose: Hides or unhides tentacle for spawning

public class TentacleSpawn : MonoBehaviour {
	private GameObject spawnManager;
	public GameObject tentacle;
	// Use this for initialization
	void Start () 
	{
		tentacle = GameObject.Find ("Tentacle");
		spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        //If spawnmanager allows it to spawn and allows more enemies to spawn, sets it to active. Else sets it to false
		if (spawnManager.GetComponent<SpawnManager> ().canTentacleSpawn && spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn ) 
		{
			tentacle.SetActive (true);
		} 
		else 
		{
			tentacle.SetActive (false);
		}
	}
	
}
