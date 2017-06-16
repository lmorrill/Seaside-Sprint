using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : MonoBehaviour {

	public GameObject turtleMain;
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Feet")
		{
			turtleMain.SendMessage("TurtleHit");
		}
	}
}
