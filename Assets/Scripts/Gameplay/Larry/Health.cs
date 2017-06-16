// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: To control Larry's health, this also manages the player hearts on the canvas and the gameover text

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private int maxHealthValue = 70;
    public int currentHealthValue;
    public int currentHeart = 1;

    public GameObject heart01;
    public GameObject heart02;
    public GameObject heart03;
    public GameObject gameOverTXT;

    public AudioSource gainHealthAudio;
    public AudioSource loseHealthAudio;
    public AudioSource deathAudioSource;

    void Start()
    {
        currentHealthValue = maxHealthValue;   
    }

    // Update is called once per frame
    void Update()
    {
        // Health debugging tools
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    TakeDamage(10);
        //}

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    IncreaseHealth(10);
        //}

        HealthCheck();
    }

    // Set the heart images to display the proper hearts to larrys health
    private void HealthCheck()
    {

        if (currentHealthValue <= 70 && currentHealthValue > 60)
        {
            heart01.GetComponent<HeartController>().firstPartActive = false;
            heart01.GetComponent<HeartController>().secondPartActive = false;
            heart02.GetComponent<HeartController>().firstPartActive = false;
            heart02.GetComponent<HeartController>().secondPartActive = false;
            heart03.GetComponent<HeartController>().firstPartActive = false;
            heart03.GetComponent<HeartController>().secondPartActive = false;
        }
        else if (currentHealthValue <= 60 && currentHealthValue > 50)
        {
            heart01.GetComponent<HeartController>().firstPartActive = true;
            heart01.GetComponent<HeartController>().secondPartActive = false;
            heart02.GetComponent<HeartController>().firstPartActive = false;
            heart02.GetComponent<HeartController>().secondPartActive = false;
            heart03.GetComponent<HeartController>().firstPartActive = false;
            heart03.GetComponent<HeartController>().secondPartActive = false;
        }
        else if(currentHealthValue <= 50 && currentHealthValue > 40)
        {
            heart01.GetComponent<HeartController>().firstPartActive = false;
            heart01.GetComponent<HeartController>().secondPartActive = true;

            heart02.GetComponent<HeartController>().firstPartActive = false;
            heart02.GetComponent<HeartController>().secondPartActive = false;

            heart03.GetComponent<HeartController>().firstPartActive = false;
            heart03.GetComponent<HeartController>().secondPartActive = false;
        }
        else if (currentHealthValue <= 40 && currentHealthValue > 30)
        {
            heart01.GetComponent<HeartController>().firstPartActive = false;
            heart01.GetComponent<HeartController>().secondPartActive = true;

            heart02.GetComponent<HeartController>().firstPartActive = true;
            heart02.GetComponent<HeartController>().secondPartActive = false;

            heart03.GetComponent<HeartController>().firstPartActive = false;
            heart03.GetComponent<HeartController>().secondPartActive = false;
        }
        else if (currentHealthValue <= 30 && currentHealthValue > 20)
        {
            heart01.GetComponent<HeartController>().firstPartActive = false;
            heart01.GetComponent<HeartController>().secondPartActive = true;

            heart02.GetComponent<HeartController>().firstPartActive = false;
            heart02.GetComponent<HeartController>().secondPartActive = true;

            heart03.GetComponent<HeartController>().firstPartActive = false;
            heart03.GetComponent<HeartController>().secondPartActive = false;
        }
        else if (currentHealthValue <= 20 && currentHealthValue > 10)
        {
            heart01.GetComponent<HeartController>().firstPartActive = false;
            heart01.GetComponent<HeartController>().secondPartActive = true;

            heart02.GetComponent<HeartController>().firstPartActive = false;
            heart02.GetComponent<HeartController>().secondPartActive = true;

            heart03.GetComponent<HeartController>().firstPartActive = true;
            heart03.GetComponent<HeartController>().secondPartActive = false;
        }
        else if (currentHealthValue <= 10 && currentHealthValue > 0)
        {
            heart01.GetComponent<HeartController>().firstPartActive = false;
            heart01.GetComponent<HeartController>().secondPartActive = true;

            heart02.GetComponent<HeartController>().firstPartActive = false;
            heart02.GetComponent<HeartController>().secondPartActive = true;

            heart03.GetComponent<HeartController>().firstPartActive = false;
            heart03.GetComponent<HeartController>().secondPartActive = true;
        }
        else if(currentHealthValue <= 0)
        {
            deathAudioSource.Play();
            gameOverTXT.SetActive(true);
            GameObject spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
            spawnManager.GetComponent<SpawnManager>().ResetValues();
            SceneManager.LoadScene("MainMenu");

            //Debug.Log("Game Over");
        }

        if (currentHealthValue <= 70 && currentHealthValue > 50)
        {
            // Heart 1
            currentHeart = 1;
        }
        else if (currentHealthValue <= 50 && currentHealthValue > 30)
        {
            // Heart 2
            currentHeart = 2;
        }
        else if (currentHealthValue <= 30 && currentHealthValue > 10)
        {
            // Heart 3
            currentHeart = 3;
        }
    }

    public void IncreaseHealth(int HealthIncrease)
    {
        if(currentHealthValue < maxHealthValue)
        {
            currentHealthValue += HealthIncrease;
        }

        if(currentHeart == 1)
        {
            heart01.GetComponent<HeartController>().ActivateHeartGainParticle();
        }
        else if(currentHeart == 2)
        {
            heart02.GetComponent<HeartController>().ActivateHeartGainParticle();
        }
        else if(currentHeart == 3)
        {
            heart03.GetComponent<HeartController>().ActivateHeartGainParticle();
        }

        gainHealthAudio.Play();
    }

    public void TakeDamage(int DamageDone)
    {
        currentHealthValue -= DamageDone;

        if(currentHealthValue <= 0)
        {
            currentHealthValue = 0;
        }

        if (currentHeart == 1)
        {
            heart01.GetComponent<HeartController>().ActivateHeartLossParticle();
        }
        else if (currentHeart == 2)
        {
            heart02.GetComponent<HeartController>().ActivateHeartLossParticle();
        }
        else if (currentHeart == 3)
        {
            heart03.GetComponent<HeartController>().ActivateHeartLossParticle();

        }

        loseHealthAudio.Play();
    }
}
