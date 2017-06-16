using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name Bird.cs
//Author: Ted Deurloo
//Purpose: Pelican Movement



public class Bird : MonoBehaviour {

    #region Variables
    public float speed =5;
    private GameObject spawnManager;
    public Transform fishSpawn;
    public GameObject fish1;
    public GameObject fish2;
    GameObject fish;

    GameObject fishClone;
    GameObject beak;
    float direction;
    public GameObject birdBody;
    public GameObject birdSpawnParent;
    public GameObject[] birdSpawnPoints = new GameObject[2];
    bool checkVisible = false;
    Transform birdSpawnStart;
    public bool startRight;

    bool beakOpen = false;
    bool swiped = false;
    float timer;

    Vector2 deathDirection;
    Vector3 deathRotation;
    float deathSpeed;

    public bool spawnAllowed;
    public float hoverLimit = 20;
	AudioSource audioSource;
	//0 is Wing Flap, 1 is Death Squak
	//public AudioClip[] audioClips = new AudioClip[2];
    #endregion

	//State the bird is in during gameplay
    enum State
    {
        Stop,
        Flying,
        Hover,
        DropFish,
		Dead
    }

    State birdState = State.Stop;

	void Start ()
    {
        //finds the object with the swipecontroller

        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
		audioSource = GetComponent<AudioSource> ();

        //fills the array with spawnpoints for the bird if they haven't already
        if (birdSpawnParent == null)
        {
            birdSpawnParent = GameObject.Find("BirdSpawnPoints");
        }
        for(int i =0; i <= birdSpawnPoints.Length-1; i++)
        {
            if(birdSpawnPoints[i] == null)
            {
                birdSpawnPoints[i] = birdSpawnParent.transform.Find("birdspawn" + i.ToString()).gameObject;
            }
            //Debug.Log("birdspawn" + i.ToString());
        }
		//grabs the children containing the sprites
        beak = transform.Find("Pelican/BeakTack").gameObject;
        birdBody = transform.Find("Pelican").gameObject;

		//randomly starts at spawnpoint
        switch(Random.Range(1,3))
        {
            case 1:
                startRight = false;
                direction = 1;
				speed *= -1;
                birdSpawnStart = birdSpawnPoints[0].transform;
                break;
            case 2:
				birdSpawnStart = birdSpawnPoints[1].transform;
                this.transform.localScale = new Vector3(this.transform.localScale.x * -1, 
                    this.transform.localScale.y, this.transform.localScale.z);
                startRight = true;
                direction = -1;
                break;
        }
        transform.position = birdSpawnStart.position;
        deathSpeed = speed * 2;
	}

	void Update ()
    {
        switch(birdState)
        {
            case State.Stop:
                //Checks SpawnManager before appearing on screen
                if (spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canPelicanSpawn)
                {
                    if (timer >= 5)
                    {
                        birdState = State.Flying;
						audioSource.Play ();
                        timer = 0;
                        spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }
                break;
                //Flies to the other side of the screen
            case State.Flying:
                Vector2 viewpos = Camera.main.WorldToViewportPoint(transform.position);
			if (((viewpos.x >= 0.48 && viewpos.x <= 0.54) && fishClone == null) && swiped == false)
                {
                    birdState = State.Hover;
                    timer = 0;
                    checkVisible = true;
                }
                if(beakOpen == true)
                {
                    CloseBeak();
                }
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                break;
			//open beaks to drop the fish if near center of the screen
		    case State.Hover:
			    if (timer >= hoverLimit) 
			    {
				    if (beakOpen == true) 
				    {
					    birdState = State.DropFish;
				    } 
				    else if (beakOpen == false) 
				    {
					    birdState = State.Flying;
				    }

                    switch(Random.Range(1,3))
                    {
                        case 1:
                            fish = fish1;
                            break;
                        case 2:
                            fish = fish2;
                            break;
                        default:
                            fish = fish1;
                            break;
                    }
			    }
			
			    if (swiped == true)
			    {
				    birdState = State.Flying;
			    }
                else if (swiped == false) 
			    {
				    OpenBeak ();
			    } 
                timer += Time.deltaTime;
                break;
                //drops fish
            case State.DropFish:
                fishClone = Instantiate(fish, fishSpawn.position, fishSpawn.rotation);
                fishClone.transform.localScale = new Vector3(fish.transform.localScale.x * direction, fish.transform.localScale.y, fish.transform.localScale.z);
                birdState = State.Flying;
                break;
		    case State.Dead:
                transform.Translate(deathDirection * deathSpeed * Time.deltaTime);
                birdBody.transform.Rotate(deathRotation * -100 * Time.deltaTime);
                break;

            default:
                break;
        }
        //Runs function if statement is true
        if (checkVisible == true)
        {
            CheckChildRendererVisible();
        }
	}

	//Opens the beak sprite
    void OpenBeak()
    {
        Quaternion targetRotation;
        if (startRight == false)
        {
            targetRotation = Quaternion.Euler(0,0,340); //Rotates 20 degrees
        }
        else
        {
            targetRotation = Quaternion.Euler(0, 0, 20);
        }
        beak.transform.rotation = Quaternion.RotateTowards(beak.transform.rotation, targetRotation, 180 * Time.deltaTime);
        if(beak.transform.rotation == targetRotation)
        {
            beakOpen = true;
        }
        
    }
    //Closes the beak sprite
    void CloseBeak()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        beak.transform.rotation = Quaternion.RotateTowards(beak.transform.rotation, targetRotation, 180 * Time.deltaTime);
        if (beak.transform.rotation == targetRotation)
        {
            beakOpen = false;
        }
        checkVisible = true;
    }

	//Things that happen when swiped.
    public void Swiped()
    {
		//closes beak if its open
		if (birdState == State.Hover && beakOpen == true) {
			swiped = true;
		} 
		//kills pelican
		else 
		{
            if(SwipeController.Instance.IsSwiping(SwipeDirection.LEFT))
            {
                deathDirection = Vector2.right;
                deathRotation = Vector3.back;
            }
            else if(SwipeController.Instance.IsSwiping(SwipeDirection.RIGHT))
            {
                deathDirection = Vector2.left;
                deathRotation = Vector3.forward;
            }
			birdState = State.Dead;
			audioSource.Stop ();
			swiped = true;
            checkVisible = true;
		}
    }
	//checks if the spriterendener in the child
    private void CheckChildRendererVisible()
    {
        if (birdBody.GetComponent<SpriteRenderer>().isVisible == false)
        {
            switch (Random.Range(1, 3))
            {
                case 1:
                    startRight = true;
                    direction = -1;
                    if (birdSpawnStart == birdSpawnPoints[0].transform)
                    {
                        speed *= -1;
                        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
                    }
                    birdSpawnStart = birdSpawnPoints[1].transform;
                    break;
                case 2:
                    startRight = false;
                    direction = 1;
                    if (birdSpawnStart == birdSpawnPoints[1].transform)
                    {
                        speed *= -1;
                        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
                    }
                    birdSpawnStart = birdSpawnPoints[0].transform;
                    break;
            }
            birdState = State.Stop;
            audioSource.Stop();
            spawnManager.GetComponent<SpawnManager>().DestroyEnemy();
            this.transform.position = birdSpawnStart.position;
            birdBody.transform.rotation = birdSpawnStart.transform.rotation;
            checkVisible = false;
            swiped = false;
            timer = 0;
        }

    }
}
