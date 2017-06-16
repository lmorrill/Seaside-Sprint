using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name: Seagull
//Author: Ted Deurloo
//Purpose: Seagull Movement

public class Seagull : MonoBehaviour {
	public float speed = 5;
	public float startTimer = 5;
	private float timer;
    private GameObject spawnManager;

	public Transform spawnPoint;
	public GameObject gullSprite;
	public int damageGiven;
    public bool spawnAllowed = true;

	private float direction;
	public Transform[] flyPoints;
	public GameObject gullPointParent;
	public bool startRight = false;
	GameObject player;
	Vector3 flytowards;
	bool flyBack = false;
	int flyIndex;
	bool checkVisible = false;

	AudioSource audioSource;
	private enum State
	{
		Idle,
		Fly,
		Swoop,
		Dead
	}
	State gullState = State.Idle;
	void Start () 
	{
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
		audioSource = GetComponent<AudioSource> ();

        if (gullPointParent == null)
		{
			gullPointParent = GameObject.Find("GullSpawnParent");
		}
		for(int i =0; i <= flyPoints.Length-1; i++)
		{
			if(flyPoints[i] == null)
			{
				flyPoints [i] = gullPointParent.transform.Find ("GullSpawn" + (i + 1).ToString ());
			}
			
		}
		if (damageGiven <= 0) 
		{
			damageGiven = 20;
		}

		switch (Random.Range (1, 3)) {
		case 1:
			startRight = true;
			flyIndex = flyPoints.Length-1;
			spawnPoint = flyPoints [flyIndex];
			direction = -1;
			gullSprite.transform.localScale = new Vector3 (gullSprite.transform.localScale.x * -1, 
				gullSprite.transform.localScale.y, gullSprite.transform.localScale.z); 
			break;
		case 2:
			startRight = false;
			direction = 1;
			spawnPoint = flyPoints [0];
			flyIndex = 0;
			break;
		}
		timer = 0;
		//Starts at spawnpoint
		this.transform.position = spawnPoint.position;
	}
	
	// Update is called once per frame
	void Update () {
		player = GameObject.FindGameObjectWithTag ("Player");
		switch (gullState) {
		//State.Stop will be taken out for build. Its here for testing
		case State.Idle:
                if (spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canSeagullSpawn)
                {
                    if (timer >= 3)
                    {
                        gullState = State.Fly;
                        audioSource.Play();
                        spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }
			break;
		case State.Fly:
			Fly ();
			break;
		case State.Swoop:
			Swoop ();
			break;
			//Hangs in the air for player reaction
		case State.Dead:
			//Goes down when the shark hits the barrier. Might changed it to phyics later on.
			if (startRight == true)
			{
				transform.Translate ((Vector2.down + Vector2.right) * speed * Time.deltaTime);
			} 
			else if (startRight == false)
			{
				transform.Translate ((Vector2.down + Vector2.left) * speed * Time.deltaTime);
			}
			//Rotates the sprite with moving the entire thing
			gullSprite.transform.Rotate (Vector3.forward * -100 * Time.deltaTime);
			checkVisible = true;
			break;
		default:
			break;
		}
		if (checkVisible == true)
		{
			CheckChildRendererVisible();
		}
		
	}

	private void CheckChildRendererVisible()
	{
		if (gullSprite.GetComponent<SpriteRenderer> ().isVisible == false) 
		{
			switch (Random.Range (1, 3)) {
			case 1:
				startRight = true;
				flyIndex = flyPoints.Length - 1;
				if (spawnPoint == flyPoints [0]) 
				{
					gullSprite.transform.localScale = new Vector3 (gullSprite.transform.localScale.x * -1, gullSprite.transform.localScale.y, gullSprite.transform.localScale.z); 
				}
				spawnPoint = flyPoints [flyIndex];
				break;
			case 2:
				startRight = false;
				flyIndex = 0;
				if (spawnPoint == flyPoints [flyPoints.Length - 1]) {
					gullSprite.transform.localScale = new Vector3 (gullSprite.transform.localScale.x * -1, gullSprite.transform.localScale.y, gullSprite.transform.localScale.z); 
				}
				spawnPoint = flyPoints [flyIndex];				
				break;
			}
			gullState = State.Idle;
            spawnManager.GetComponent<SpawnManager>().DestroyEnemy();
			this.transform.position = spawnPoint.position;
			gullSprite.transform.rotation = spawnPoint.transform.rotation;
			checkVisible = false;
			timer = 0;
		}

	}

	private void OnTriggerEnter (Collider collider)
	{
		var hit = collider.gameObject;
		if (hit.tag == "Larry" && gullState == State.Swoop) {
			var health = hit.GetComponent<Health> ();
			
			if (health != null) {
				health.TakeDamage (damageGiven);
			}
		}
	}

    public void KillBird()
    {
        gullState = State.Dead;
    }

 

	void Fly ()
	{
		if (startRight == true) 
		{
			if (transform.position == flyPoints [flyPoints.Length - 2].position && flyBack == true) 
			{
				gullSprite.transform.localScale = new Vector3 (gullSprite.transform.localScale.x * -1, 
					gullSprite.transform.localScale.y, gullSprite.transform.localScale.z); 
				gullState = State.Swoop;
				flyIndex = flyPoints.Length - 3;
                audioSource.Play();
			} 
			else if (transform.position == flyPoints [flyPoints.Length - 1].position && flytowards !=flyPoints [1].position) 
			{
				flytowards = flyPoints [1].position;

			} 
			else if (transform.position == flyPoints [1].position && flytowards != flyPoints [flyPoints.Length - 2].position) 
			{
				flytowards = flyPoints [flyPoints.Length - 2].position;
				gullSprite.transform.localScale = new Vector3 (gullSprite.transform.localScale.x * -1, 
					gullSprite.transform.localScale.y, gullSprite.transform.localScale.z); 
				flyBack = true;
			}
			else
			{
				transform.position = Vector3.MoveTowards (transform.position, flytowards, speed * Time.deltaTime);
			}

		} 
		else if (startRight == false)
		{
			if (transform.position == flyPoints [1].position && flyBack == true) 
			{
				gullSprite.transform.localScale = new Vector3 (gullSprite.transform.localScale.x * -1, 
					gullSprite.transform.localScale.y, gullSprite.transform.localScale.z); 
				gullState = State.Swoop;
				flyIndex = 2;
			} 
			else if (transform.position == flyPoints [0].position && flytowards != flyPoints [flyPoints.Length - 2].position) 
			{
				flytowards = flyPoints [flyPoints.Length - 2].position;
			} 
			else if (transform.position == flyPoints [flyPoints.Length - 2].position && flytowards != flyPoints [1].position) 
			{
				flytowards = flyPoints [1].position;
				gullSprite.transform.localScale = new Vector3 (gullSprite.transform.localScale.x * -1, 
					gullSprite.transform.localScale.y, gullSprite.transform.localScale.z); 
				flyBack = true;
			}
			else
			{
				transform.position = Vector3.MoveTowards (transform.position, flytowards, speed * Time.deltaTime);
			}
		}
	}

	void Swoop()
	{
		if (startRight == true)
		{
			if (transform.position == flyPoints [0].position)
			{
				gullState = State.Idle;
				checkVisible = true;
				

			}
			else if (transform.position == flyPoints [flyIndex].position)
			{
				flyIndex--;
				
			}
			else
			{
				transform.position = Vector3.MoveTowards (transform.position, flyPoints [flyIndex].position, speed * Time.deltaTime);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, flyPoints [flyIndex].rotation, 100 * Time.deltaTime);
			}
		}
		else if (startRight == false)
		{
			if (transform.position == flyPoints [flyPoints.Length-1].position)
			{
				gullState = State.Idle;
				checkVisible = true;
				

			}
			else if (transform.position == flyPoints [flyIndex].position)
			{
				flyIndex++;
				
			}
			else
			{
				transform.position = Vector3.MoveTowards (transform.position, flyPoints [flyIndex].position, speed * Time.deltaTime);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, flyPoints [flyIndex].rotation, 100 * Time.deltaTime);
			}
		}
	}
}
