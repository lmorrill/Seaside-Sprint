//Author: Lucas Morrill
//Email: lmorrill02@gmail.com
//Description: Controls the turtle
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour {
	public GameObject turtle;
	public GameObject shell;
	public GameObject turtleCheckRight;
	public GameObject turtleCheckLeft;
	public GameObject spawn;
	private GameObject spawnManager;
	bool inShell = true;
	int movingDown = 0;
	bool timerStart = false;
	float timer = 3;
	bool turtleHit = false;
	bool moving = false;
	float spawnTimer = 10;

	void Start ()
	{
		spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");

	}

	void Update ()
	{

		if (gameObject.transform.position == spawn.transform.position)
		{
			spawnTimer -= Time.deltaTime;
		}
		if(spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canTurtleSpawn && spawnTimer < 0)
		{
			spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
			moving = true;
			spawnTimer = 10;
		}


		if (moving)
		{
			TimerForShell();

			if (turtleHit)
			{
				transform.Translate(new Vector3(8,0,0) * Time.deltaTime);
				if (gameObject.transform.position.x > turtleCheckRight.transform.position.x)
				{
					turtleHit = false;
					timer = 3;
					TurtleInShell();
					gameObject.transform.position = spawn.transform.position;
				}
			}
			else
			{
				MoveingLeft();
				if (gameObject.transform.position.x < turtleCheckLeft.transform.position.x)
				{

					moving = false;
					spawnManager.GetComponent<SpawnManager>().DestroyEnemy();
					gameObject.transform.position = spawn.transform.position;
				}
			}

			if (!inShell)
			{
				if (shell.transform.position.y > -1f)
				{
					shell.transform.Translate(new Vector3(0,-1f,0) * Time.deltaTime);
				}
			}
			else
			{
				if (shell.transform.position.y <= -0.5f)
				{
					shell.transform.Translate(new Vector3(0,1f,0) * Time.deltaTime);
				}
			}
		}


	}
	void MoveingLeft()
	{
		if (!inShell)
		{
			transform.Translate(new Vector3(-3,0,0) * Time.deltaTime);
		}
		else
		{
			transform.Translate(new Vector3(-4,0,0) * Time.deltaTime);
		}
	}
	void TurtleInShell()
	{
		if (!turtleHit)
		{
			if (timer >= 3)
			{
				timerStart = inShell;
				shell.SetActive(inShell);
				inShell = !inShell;
				turtle.SetActive(inShell);
			}
		}
	}
	void TimerForShell()
	{
		if (timerStart)
		{
			timer -= Time.deltaTime;
			if (timer < 0)
			{
				timer = 3;
				TurtleInShell();
				timerStart = false;
			}
		}
	}
	void TurtleHit()
	{
		turtleHit = true;
		shell.SetActive(true);
		turtle.SetActive(false);
	}
}
