// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Whenever needing to change scene just find designated game object and access script and desired method.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {

	}

	// Update is called once per frame
	void Update ()
    {

	}

    // Use these methods to change scenes from the main menu.
    // Script will be added to a Main Menu Controller game object.

    public void StartGame()
    {
        SceneManager.LoadScene("SeasideSprint");
    }

    public void MainMenu()
    {
        if(SceneManager.GetActiveScene().name == "SeasideSprint")
        {
            GameObject spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
            spawnManager.GetComponent<SpawnManager>().ResetValues();
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("ContactInfo");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
