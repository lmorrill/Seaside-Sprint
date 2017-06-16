using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name: Tenta Node
//Author: Ted Deurloo
//Purpose: Sets bool if that tentacle node been touched or not.

public class TentaNode : MonoBehaviour {
    public bool tentaTouched;
	// Use this for initialization
	void Awake ()
    {
        tentaTouched = false;
	}

    //changes to true
    public void TouchedTentacle()
    {
        tentaTouched = true;
    }

    //Resets back to false
    public void ResetTentacle()
    {
        tentaTouched = false;
    }
		



}
