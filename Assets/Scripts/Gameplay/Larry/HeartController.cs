// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Controller for each individual heart, in the Health script when a change on a heart happens you will call this script and set the proper section of the heart to active or not active.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    public bool firstPartActive = false;
    public bool secondPartActive = false;

    public GameObject firstTape;
    public GameObject secondTape;
    public ParticleSystem heartGainParticle;
    public ParticleSystem heartLossParticle;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(firstPartActive)
        {
            firstTape.SetActive(true);
            secondTape.SetActive(false);
        }
        else if(secondPartActive)
        {
            firstTape.SetActive(false);
            secondTape.SetActive(true);
        }
        else if(!firstPartActive && !secondPartActive)
        {
            firstTape.SetActive(false);
            secondTape.SetActive(false);
        }
	}

    public void ActivateHeartGainParticle()
    {
        heartGainParticle.Play();
    }

    public void ActivateHeartLossParticle()
    {
        heartLossParticle.Play();
    }
}
