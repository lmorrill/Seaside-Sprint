// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Controls the text being displayed for the introduction, allows swapping between differnt controls text.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionController : MonoBehaviour
{
    // UI
    // List on panels or game information that is to be displayed to player.
    public List<GameObject> controlPanels;
    private int activeControlsPanel = 0;

	// Use this for initialization
	void Start ()
    {
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // Use to change panels for game information.
    // Add to empty game object/
    // Access methods through a button
    // Have the button pass in the needed bool that will ether go the next panel if true and the previous panel if false.
    public void NextControlPanel(bool increaseOrDecrease)
    {
        if(increaseOrDecrease)
        {
            activeControlsPanel++;
        }
        else if(!increaseOrDecrease)
        {
            activeControlsPanel--;
        }

        if(activeControlsPanel >= controlPanels.Count)
        {
            activeControlsPanel = 0;
        }

        if(activeControlsPanel < 0)
        {
            activeControlsPanel = controlPanels.Count - 1;
        }

        for(int index = 0; index < controlPanels.Count; index++)
        {
            if(index == activeControlsPanel)
            {
                controlPanels[index].SetActive(true);
            }
            else if(index != activeControlsPanel)
            {
                controlPanels[index].SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
