//Author: Lucas
//Email: lmorrill02@gmail.com
//Description: Controls the level
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
	public GameObject[] floorCount;
    public GameObject[] checkAndSpawn;
	//Check and spawn is an array that should only have 2 things a check object and a spawnPoint object.  if one of these level parts goes into the same x position it will move it to the spawn object.
	//the checkandspawn will need its [0] position to be the check object and the [1] will be the spawn object.
	bool stopWorld;
	float levelSpeed = 0;
	public float unchangingSpeed = 5f;
	//unchangingSpeed is the maxSpeed that the level will reach.
	public float decayingSpeed = 0.1f;
	//decayingSpeed is the speed that will be lost 60 times a second when the player falls into the hole and the speed that the level uses to get back to its max speed.
    public bool moveLeft = true;
	//if the level is not moving left then its moving right
	void Start ()
	{
		stopWorld = false;
		//for (int i = 0; i < floorCount.Length; i++)
		//{
			//floorCount[i].transform.position = new Vector3(i, 0, 0);
		//}
	}

	void Update ()
	{
		MoveBackAndForward();
		LevelSpeedManager();
	}
	void MoveBackAndForward()
	{
		//this goes though all of the objets in the array of level parts and moves them etiher left or right depending on if moveLeft is true.
		for (int i = 0; i <= floorCount.Length-1; i++)
		{
			//if you want the level to moveleft then make moveLeft true. if its false the level will move right.
			//if the level reaches the Check object then it will be moved to the Spawn object.
            if (moveLeft)
            {
                floorCount[i].transform.position -= new Vector3(levelSpeed, 0, 0);
                if (floorCount[i].transform.position.x <= checkAndSpawn[0].transform.position.x)
                {
                    floorCount[i].transform.position = checkAndSpawn[1].transform.position;
                }
            }
            else
            {
                floorCount[i].transform.position += new Vector3(levelSpeed, 0, 0);
                if (floorCount[i].transform.position.x >= checkAndSpawn[0].transform.position.x)
                {
                    floorCount[i].transform.position = checkAndSpawn[1].transform.position;
                }
            }

		}
	}
	public void InTheHoleCheck()
	{
		//if the player is in the hole stop the world from moving.
		stopWorld = true;
	}
	public void OutOfCheckHole()
	{
		//if the player jumps out of the hole move the world again
		stopWorld = false;
	}
	void LevelSpeedManager()
	{
		//this method manages the speed of the world if you want to edit the max speed change the unchaging speed.
		//this also makes the world get to speed smoothly when the player enters the hole.
		if (stopWorld)
		{
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
