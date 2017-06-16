using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name: Fish
//Author: Ted Deurloo
//Purpose: Fish Behaviour when dropped

public class Fish : MonoBehaviour {
    public int damageGiven = 10;
    private void Start()
    {
    }
    void OnTriggerEnter(Collider collider)
    {
        var hit = collider.gameObject;
        if (hit.tag == "Larry")
        {
            var health = hit.GetComponent<Health>();
            //Debug.Log("HIT!");
            if (health != null)
            {
                health.TakeDamage(damageGiven);
				Destroy (this.gameObject);
            }
        }
		if (hit.tag == "LeftHand" || hit.tag == "RightHand")
        {

            Destroy(this.gameObject);
        }
    }
		
}
