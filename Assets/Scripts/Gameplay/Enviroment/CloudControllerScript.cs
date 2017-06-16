//Author: Lucas
//Email: lmorrill02@gmail.com
//Description: Controls the clouds(they used to be platforms so i needed a differnt script at the time)
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudControllerScript : MonoBehaviour {
	public GameObject[] cloudCount;
    public GameObject[] checkAndSpawn;
	bool stopWorld;
	float levelSpeed = 0;
	public float unchangingSpeed = 5f;
	public float decayingSpeed = 0.1f;

	//float timerReset = 5;
	//float timer;
	void Start ()
	{
		//timer = timerReset;
		stopWorld = false;
		//for (int i = 0; i < cloudCount.Length; i++)
		//{
			//cloudCount[i].transform.position = new Vector3(i, 0, 0);
		//}
	}


	void Update ()
	{
		MoveBackAndForward();
		LevelSpeedManager();
	}
	void MoveBackAndForward()
	{
			//goes through all the clouds and makes them move to the left
		//timer -= Time.deltaTime;
		//if (timer < 0)
		//{
			for (int i = 0; i <= cloudCount.Length-1; i++)
			{

	                cloudCount[i].transform.position -= new Vector3(levelSpeed, 0, 0);
	                if (cloudCount[i].transform.position.x <= checkAndSpawn[0].transform.position.x)
	                {
	                    cloudCount[i].transform.position = checkAndSpawn[1].transform.position;
						//timer = timerReset;
	                }
			}

		//}

	}
	void LevelSpeedManager()
	{
		if (stopWorld)
		{
			//this method manages the speed of the world if you want to edit the max speed change the unchaging speed.
			//this also makes the world get to speed smoothly when the player enters the hole.
			levelSpeed -= decayingSpeed;
			if (levelSpeed <= 0)
			{
				levelSpeed = 0;
			}
		}
		else
		{
			levelSpeed += decayingSpeed;
			if (levelSpeed >= unchangingSpeed)
			{
				levelSpeed = unchangingSpeed;
			}

		}
	}
}
