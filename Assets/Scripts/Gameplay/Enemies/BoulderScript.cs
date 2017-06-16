//Author: Lucas
//Email: lmorrill02@gmail.com
//Description: Controls the Boulder
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderScript : MonoBehaviour
{
	public bool roll = false;
	private GameObject spawnManager;
    public GameObject spawnPoint;
	//the point that the boulder returns too when its reset
    public GameObject endPoint;
	//the point that the boulder will hit if the player doesn't stop it, when it reaches this point it will return to the spawn.
    public GameObject tooFarPoint;
	//the point that the boulder will hit if the player rolls it the other way and rolls past the spawn it will hit this and go back to its spawn.
	public GameObject bottomPoint;
	float spawnTimer = 10;
	bool moving = false;
	bool dropRock = false;
	public AudioSource hitWater;
	void Start ()
	{
		spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
	}


	void Update ()
	{
		if (dropRock)
		{
			Drop();
		}
		
		
		if(spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canBoulderSpawn && spawnTimer < 0)
		{
			spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
			moving = true;
			spawnTimer = 10;
		}

		if (moving)
		{
			//if the boulder goes too far right or left it will go back and reset itself.
	        if ((gameObject.transform.position.x <= endPoint.transform.position.x) || (gameObject.transform.position.x >= tooFarPoint.transform.position.x))
	        {
				spawnManager.GetComponent<SpawnManager>().DestroyEnemy();
				moving = false;
	            roll = false;
                dropRock = false;
                gameObject.transform.position = spawnPoint.transform.position;
	        }
			if (gameObject.transform.position.y < bottomPoint.transform.position.y)
			{
				spawnManager.GetComponent<SpawnManager>().DestroyEnemy();
				moving = false;
                roll = false;
                hitWater.Play();
				dropRock = false;
				gameObject.transform.position = spawnPoint.transform.position;
			}
			//if the 4 boulder parts have been hit by the player then it will roll the other way.
	        if (roll)
			{
				transform.Rotate(new Vector3(0,0,-80) * Time.deltaTime);
	   		 	gameObject.transform.position -= new Vector3(-0.1f,0,0);
			}
			else
			{
				transform.Rotate(new Vector3(0,0,80) * Time.deltaTime);
	   		 	gameObject.transform.position -= new Vector3(0.03f,0,0);
			}
		}
        else
        {
            spawnTimer -= Time.deltaTime;
        }
	}

	void OnTriggerEnter(Collider col)
	{
		//if the boulder collides with larry lower the health of larry.
        if (col.gameObject.tag == "Larry")
        {
			dropRock = true;
            var health = col.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
        }
	}

   void Roll()
   {
	   //this is the method that is called when all the parts are broken and the boulder should roll.
	   roll = true;

   }
   void Drop()
   {
	   gameObject.transform.position -= new Vector3(0,0.1f,0);
   }
}
