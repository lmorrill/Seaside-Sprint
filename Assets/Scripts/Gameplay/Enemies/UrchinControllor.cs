// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: To do collision detection for the urchin 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinControllor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Larry")
        {
            //Debug.Log("Larry hit");
            other.GetComponent<Health>().TakeDamage(10);
        }
    }
}
