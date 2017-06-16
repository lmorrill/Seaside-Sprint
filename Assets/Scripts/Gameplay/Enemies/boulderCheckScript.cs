using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulderCheckScript : MonoBehaviour
{
	//This script is attached to a MiddleMan object inside of the boulder.
	//The boulder has 4 parts on it that the player as to move their hand through in order to make the boulder spin the other way, this is the count of how many parts have been hit by the player.
	public int partCount = 0;
	public GameObject boulder;
    public GameObject resetLeft;
    public GameObject resetRight;
    public GameObject[] parts;
    void Start () {

	}


	void Update ()
	{
		//if the boulder is between two points that are near its spawn it will reset itself.
        if (boulder.transform.position.x >= resetLeft.transform.position.x && boulder.transform.position.x <= resetRight.transform.position.x)
        {
			//the part count will return to zero and the parts will be active again.
            partCount = 0;
            for (int i = 0; i <=3; i++)
            {
                parts[i].SetActive(true);
            }
        }
        if (partCount >= 4)
		{
			//this sends a message to the boulder telling it that all the parts have been hit by the player and the boulder needs to roll the other way.
			boulder.SendMessage("Roll");
		}

	}


	public void PartCollided()
	{
		//the parts will send a message to this method that will tell this that a part of the boulder has been hit.
		partCount++;
	}
}
