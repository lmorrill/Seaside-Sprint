using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Name: Tentacle
//Author: Ted Deurloo
//Purpose: Tentacle Movement

public class Tentacle : MonoBehaviour {

    public GameObject[] tentacleParts = new GameObject[8];
    public float rotationSpeed = 100;
    private int i;
    public float attackRotateZ;
    private GameObject spawnManager;
    private float timer = 0;
    bool allTouched;
	public int damage;
    
    private float traceTimer = 0;

	AudioSource audioSource;
	AudioSource audioSourceParent;

	//States of the tentacle
    public enum State
    {
        Wait,
        Uncurl,
        Pause,
        Attack,
        Raise,
        Dead
    }
    State squidState = State.Wait;

    //Set form the tentacle will move.
    public enum Form
    {
        A,
        B,
        C
    }
    public Form squidForm = Form.A;
	// Use this for initialization
	void Start ()
    {
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
		audioSource = GetComponent<AudioSource> ();
		audioSourceParent = transform.parent.GetComponent<AudioSource> ();
        if (damage <= 0) 
		{
			damage = 20;
		}
        if(attackRotateZ == 0 )
        {
            attackRotateZ = 90;
        }
	    for(i = 0; i < tentacleParts.Length;i++)
        {
            tentacleParts[i] = GameObject.Find("squid" + (i + 1));
            tentacleParts[i].transform.localRotation = Quaternion.Euler(0, 0, 90);
            tentacleParts[i].GetComponent<TentaNode>().tentaTouched = false;
        }
        i = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (squidState)
        {
            case State.Uncurl:
			//When it reaches the end of the array
                if(i >= tentacleParts.Length)
                {
                    squidState = State.Pause;
                    audioSource.Stop();
                    i = 0;
                }
			//each part is set to 90 degrees for easy reference
                if (i < tentacleParts.Length)
                {
                    tentacleParts[i].transform.localRotation = Quaternion.RotateTowards(tentacleParts[i].transform.localRotation, this.transform.rotation, 400 * Time.deltaTime);
                    if (tentacleParts[i].transform.localRotation == this.transform.rotation)
                    {
                        i++;
                    }
                }
                break;
            case State.Pause:
			//checks the 'main' timer first then the trace timer before attacking
                if(timer >= 5)
                {
                    timer = 0;
                    squidState = State.Attack;
                }
                else if (traceTimer > 2.0f)
                {
				//resets tentacle nodes to false
                    foreach (GameObject tentaNode in tentacleParts)
                    {
                        tentaNode.SendMessage("ResetTentacle");
                        traceTimer = 0;
                    }
                }
                if(tentacleParts[i].GetComponent<TentaNode>().tentaTouched==true)
                {
                    traceTimer += Time.deltaTime;
                }
                timer += Time.deltaTime;
                break;
			//Does the attack
            case State.Attack:
                if(this .transform.rotation == Quaternion.Euler(0, 0, attackRotateZ))
                {
                    squidState = State.Raise;
                }
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0, 0, attackRotateZ), 100 * Time.deltaTime);
                break;
			//Goes back to position
            case State.Raise:
                if (this.transform.rotation == Quaternion.Euler(0, 0, 0))

                {
                    squidState = State.Pause;
                }
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0, 0, 0), 100 * Time.deltaTime);
                break;
			//refurls out of sight
		case State.Dead:
			if (i >= 0) 
			{
				tentacleParts [i].transform.localRotation = Quaternion.RotateTowards (tentacleParts [i].transform.localRotation, Quaternion.Euler (0, 0, 90), 400 * Time.deltaTime);
				if (tentacleParts [i].transform.localRotation == Quaternion.Euler (0, 0, 90)) 
				{
					i--;
				}
			} 
			else 
			{
                squidState = State.Wait;
                    audioSource.Stop();
                spawnManager.GetComponent<SpawnManager>().DestroyEnemy();
			}
            break;
         case State.Wait:
			if(spawnManager.GetComponent<SpawnManager>().canMoreEnemiesSpawn && spawnManager.GetComponent<SpawnManager>().canTentacleSpawn)
            {
                if(timer > 5)
                {
                    squidState = State.Uncurl;
					audioSourceParent.Play ();
                    audioSource.Play();
                    spawnManager.GetComponent<SpawnManager>().SpawnEnemy();
                    timer = 0;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
            break;
        }
	}

	private void OnTriggerEnter(Collider hit)
	{
		var health = hit.GetComponent<Health> ();
		if(hit.gameObject.CompareTag("Larry") && squidState == State.Attack)
		{
			if(health != null)
			{
				health.TakeDamage (damage);
			}
		}
	}


	/// <summary>
	/// Trace along the tentacle
	/// </summary>
	/// <param name="TouchedObject">Touched Tentacle.</param>
    public void TentaTrace(GameObject TouchedObject)
    {
		//if the touched tentacle node is the one at the bottom, does thing
		if (TouchedObject == tentacleParts[0]) 
		{
            tentacleParts[0].SendMessage("TouchedTentacle");
			i++;
			
            traceTimer = 0;
		}
		//if the touched tentacle is next on the array, does thing
		else if(TouchedObject == tentacleParts[i])
		{
            tentacleParts[i].SendMessage("TouchedTentacle");
            i++;
			
            traceTimer = 0;
		}
		//if last thing in array has been touched, kills tentacle
        if(tentacleParts[tentacleParts.Length-1].GetComponent<TentaNode>().tentaTouched == true)
        {
            squidState = State.Dead;
            audioSource.Play();
			timer = 0;
			i = tentacleParts.Length - 1;
        }
    }
		
}
