using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBody : MonoBehaviour {


	void Start () {

	}


	void Update () {

	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Larry")
        {
            var health = col.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
        }
	}
}
