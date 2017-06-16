using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name: Shark Jump
//Author:Ted Deurloo
//Purpose: Shark Movement

public class SharkJump : MonoBehaviour
{

	//Variables

	#region Variables

	public float speed = 5;
	public float startTimer = 5;
	private float timer;

    private GameObject spawnManager;
	public Transform spawnPoint;
    public bool spawnAllowed=true;

	//grabs the child which has the sprite renderer
	public GameObject sharkSprite;
	public float jumpMax = 5;
	public int damageGiven;
	private float direction;
	public Transform[] swimPoints = new Transform[6];
	public bool startRight = false;
	GameObject player;
	int swimIndex;

    bool checkVisible = false;

	AudioSource audioSource;
	//Audio Clip[0] should be bite sound, Audio Clip [1] should be death sound
	#endregion

	//States the shark will be in during gameplay
	private enum State
	{
		Idle,
		Jump,
		Attack,
		Stop,
		Dead,
		Swim
	}

	private State sharkState = State.Stop;

	private void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        //Gives the default damage if nothing is given or somehow they made the value negative
        if (damageGiven <= 0) {
			damageGiven = 20;
		}
        if (spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canSharkSpawn)
        {
            sharkState = State.Swim;
            spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
        }
        switch (Random.Range (1, 3)) {
		case 1:
			startRight = true;
			spawnPoint = swimPoints [5];
			swimIndex = swimPoints.Length-1;
            direction = -1;
            break;
		case 2:
			startRight = false;
            direction = 1;
            spawnPoint = swimPoints [0];
			swimIndex = 0;
			sharkSprite.transform.localScale = new Vector3 (sharkSprite.transform.localScale.x * -1, 
                sharkSprite.transform.localScale.y, sharkSprite.transform.localScale.z); 
			break;
		}
        timer = 0;
		//Starts at spawnpoint
		this.transform.position = spawnPoint.position;
		//set up how high the shark will jump depending where the spawn is position
		jumpMax = jumpMax + spawnPoint.position.y;
	}

	private void Update ()
	{
        //Searches for Larry in game scene. Right now its not being used.
        //player = GameObject.FindGameObjectWithTag ("Larry");
        switch (sharkState) {
            //State.Stop will be taken out for build. Its here for testing
            case State.Stop:
                if (spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canSharkSpawn)
                {
                    if (timer >= 3)
                    {
                        sharkState = State.Swim;
                        spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
                        timer = 0;
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                } 
			break;
		case State.Swim:
			Swim ();
			break;
		//Goes up to where the jumpmax set above the spawnpoint
		case State.Jump:
			if (this.transform.position.y >= jumpMax)
            {
				timer = startTimer;
				sharkState = State.Idle;
                checkVisible = false;
			}
			transform.Translate ((Vector2.up * speed) * Time.deltaTime);
			break;
		//Comes at an angle towards the player
		case State.Attack:
			Vector3 target = new Vector3 (2, 0, 0);
			if (startRight == false) {
				transform.Translate ((Vector3.down + (target*-1)) * speed * Time.deltaTime);
			} 
			else {
				transform.Translate ((Vector3.down + target) * speed * Time.deltaTime);
			}
			break;
		//Hangs in the air for player reaction
		case State.Idle:
			if (timer <= 0)
            {
				sharkState = State.Attack;
                audioSource.Play();
                checkVisible = true;
			}

			sharkSprite.transform.rotation = Quaternion.RotateTowards (sharkSprite.transform.rotation, this.transform.rotation, 180 * Time.deltaTime);
			timer -= Time.deltaTime;
			break;
		case State.Dead:
                //Goes down when the shark hits the barrier. Might changed it to phyics later on.
			if (startRight == false) 
			{
				transform.Translate ((Vector2.down + Vector2.right) * speed * Time.deltaTime);
			} 
			else if (startRight == true) 
			{
				transform.Translate ((Vector2.down + Vector2.left) * speed * Time.deltaTime);
			}
                //Rotates the sprite with moving the entire thing
			sharkSprite.transform.Rotate (Vector3.forward * -100 * Time.deltaTime);
			break;
		default:
			break;
		}
        if (checkVisible == true)
        {
            
            CheckChildRendererVisible();
        }
		//ColorChange();
	}

	//For when the shark goes off screen regardless of success of attack. Can't use OnInvisible as the spriteRenderer is on the child.
	private void CheckChildRendererVisible()
	{
		if (sharkSprite.GetComponent<SpriteRenderer> ().isVisible == false) 
		{
			switch (Random.Range (1, 3)) {
			case 1:
				startRight = true;
				swimIndex = swimPoints.Length - 1;
				if (spawnPoint == swimPoints [5]) 
				{
					sharkSprite.transform.localScale = new Vector3 (sharkSprite.transform.localScale.x * -1, sharkSprite.transform.localScale.y, sharkSprite.transform.localScale.z); 
				}
				spawnPoint = swimPoints [5];
				break;
			case 2:
				startRight = false;
				swimIndex = 0;
				if (spawnPoint == swimPoints [0]) 
				{
					sharkSprite.transform.localScale = new Vector3 (sharkSprite.transform.localScale.x * -1, sharkSprite.transform.localScale.y, sharkSprite.transform.localScale.z); 
				}
				spawnPoint = swimPoints [0];				
				break;
			}
			sharkState = State.Stop;
            spawnManager.GetComponent<SpawnManager>().DestroyEnemy();
            this.transform.position = spawnPoint.position;
			sharkSprite.transform.rotation = spawnPoint.transform.rotation;
            
            checkVisible = false;
            timer = 0;
		}

	}

	//Delete this when custom sprites are made.
	/*private void ColorChange()
    {
        SpriteRenderer sprite = sharkSprite.GetComponent<SpriteRenderer>();
        switch(sharkState)
        {
            case State.Stop:
                sprite.color = Color.white;
                break;
            case State.Jump:
                sprite.color = Color.green;
                break;
            case State.Idle:
                sprite.color = Color.yellow;
                break;
            case State.Dead:
                sprite.color = Color.gray;
                break;
            case State.Attack:
                sprite.color = Color.red;
                break;
            case State.Swim:
                sprite.color = Color.cyan;
                break;
        }
    }
*/
	//Detects player and barrier collision while passing through them
	private void OnTriggerEnter (Collider collider)
	{
		var hit = collider.gameObject;
		if (hit.tag == "Larry" && sharkState == State.Attack) {
			var health = hit.GetComponent<Health> ();
			
			if (health != null) {
				health.TakeDamage (damageGiven);
			}
		}
		//change to whatever tag the 'barrier' is used if nessecarry
		if ((hit.tag == "LeftHand" || hit.tag == "RightHand") && sharkState == State.Attack) {
			
			sharkState = State.Dead;
		}
	}
		
	//Actions when its swiped
	public void Swiped ()
	{
		if (sharkState == State.Attack || sharkState == State.Idle) {
			sharkState = State.Dead;
            audioSource.Play();
            checkVisible = true;
		}
	}


	//Does the swim movement before jumping. 
	void Swim ()
	{
		if (startRight == true) {
			if (transform.position == swimPoints [0].position) {
				sharkState = State.Jump;
				swimPoints [0].GetComponentInChildren<ParticleSystem> ().Play ();
				swimPoints [0].GetComponent<AudioSource> ().Play ();
				sharkSprite.transform.localScale = new Vector3 (sharkSprite.transform.localScale.x * -1, 
					sharkSprite.transform.localScale.y, sharkSprite.transform.localScale.z); 
				sharkSprite.transform.rotation = Quaternion.Euler (0, 0, 90);
			} else if (transform.position == swimPoints [swimIndex].position) {
				swimIndex--;
			} else {
				transform.position = Vector3.MoveTowards (transform.position, swimPoints [swimIndex].position, speed * Time.deltaTime);
			}

		} 
		else if (startRight == false) {
			if (transform.position == swimPoints [5].position) {
				sharkState = State.Jump;
				swimPoints [5].GetComponentInChildren<ParticleSystem> ().Play ();
                swimPoints [5].GetComponent<AudioSource> ().Play ();
				sharkSprite.transform.localScale = new Vector3 (sharkSprite.transform.localScale.x * -1, 
					sharkSprite.transform.localScale.y, sharkSprite.transform.localScale.z); 
				sharkSprite.transform.rotation = Quaternion.Euler (0, 0, -90);
			} else if (transform.position == swimPoints [swimIndex].position) {
				swimIndex++;
			} else {
				transform.position = Vector3.MoveTowards (transform.position, swimPoints [swimIndex].position, speed * Time.deltaTime);
			}

		}
	}
}