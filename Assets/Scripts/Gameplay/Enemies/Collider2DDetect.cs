using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Name: Collider 2D Detect
//Author: Ted Deurloo
//Purpose: Detects 2D colliders
/*
    Due to the way that larry uses both the 3D collider and 2D collider for the body and hands respectfully,
    this script is added on to the child with a 2D collider. This would allow the prefab to detect both 3D colliders and 2D colliders.
     */


public class Collider2DDetect : MonoBehaviour {
	GameObject parent;

	void Start()
	{
		parent = this.transform.parent.gameObject;
	}

    
    //Make it a trigger to allow it to pass through the floor larry is standing on
	void OnTriggerEnter2D(Collider2D collider)
	{
		//Debug.Log ("HIT 2D");
		var hit = collider.gameObject;
		if (hit.CompareTag ("LeftHand") || hit.CompareTag ("RightHand")) 
		{
			//Debug.Log (hit.tag);
			if (parent.CompareTag ("Fish"))
			{
				Destroy (parent);
			}
            if(parent.CompareTag ("Gull"))
            {
                //SendMessageUpwards("KillBird");
				GetComponentInParent<Seagull>().KillBird();
            }
		}
	}
}
