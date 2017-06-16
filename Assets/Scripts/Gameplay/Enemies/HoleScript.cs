using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour {

	//this script is connected to the level controller and makes the level stop moving when the player falls into the hole.
	public GameObject levelController;

	void Update () {

	}
	public void OnTriggerEnter2D(Collider2D col)
	{
		//if the player is in the hole then stop the level.
		if (col.gameObject.tag=="Player")
		{
			levelController.SendMessage("InTheHoleCheck");
		}
	}
	public void OnTriggerExit2D(Collider2D col)
	{
		//if the player leaves the level then start moving the level.
		if (col.gameObject.tag=="Player")
		{
			levelController.SendMessage("OutOfCheckHole");
		}
	}
}
