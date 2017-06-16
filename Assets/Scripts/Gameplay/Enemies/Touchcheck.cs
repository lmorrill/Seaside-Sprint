using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchcheck : MonoBehaviour {

	public GameObject cube;


    void Start () {

	}


	void Update ()
	{


    }

	public void BoulderHit()
	{
		cube.SendMessage("PartCollided");
		gameObject.SetActive(false);
	}
	

}
