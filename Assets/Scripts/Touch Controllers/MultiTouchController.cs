// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: When a designated object is touched this will control what object has been touched and what to start doing with that object now that it has been touched.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchController : MonoBehaviour
{
    // General
    private RaycastHit2D touchRayHit;
    private Vector2 worldTouchPoint;
    private string touchTestMessage;
    private string previousTouchMessage;

    // Larry
    private GameObject larryHandPivot;

    // Use this for initialization
    void Start()
    {
        larryHandPivot = GameObject.FindGameObjectWithTag("LarryHand");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            //Translates touch space on screen to world space
            worldTouchPoint = Camera.main.ScreenToWorldPoint(touch.position);

            //Casts a ray straight down into the scene and returns the object hit
            touchRayHit = Physics2D.Raycast(worldTouchPoint, Vector2.zero);

            // Check for objects that have been touched and that they are the correct object being touched.
            if (touchRayHit.collider.tag == "GoodFood")
            {
                touchRayHit.collider.gameObject.GetComponent<ObjectTouchControl>().AddTouchPoint(touch);
            }

            if(touchRayHit.collider.tag == "LeftHand")
            {
                larryHandPivot.GetComponent<LarryHandController>().isLeftHandSelected = true;
                larryHandPivot.GetComponent<LarryHandController>().AddTouchPoint(touch);
            }

            if(touchRayHit.collider.tag == "RightHand")
            {
                larryHandPivot.GetComponent<LarryHandController>().isRightHandSelected = true;
                larryHandPivot.GetComponent<LarryHandController>().AddTouchPoint(touch);
            }
            if(touchRayHit.collider.tag == "Boulder")
            {
                touchRayHit.collider.gameObject.SendMessage("BoulderHit");
            }
            if (touchRayHit.collider.tag == "Urchin")
            {
                touchRayHit.collider.gameObject.GetComponent<UrchinMovement>().UrchinTouched();
            }
            if(touchRayHit.collider.tag == "TurtleBody")
            {
                touchRayHit.collider.gameObject.SendMessage("TurtleInShell");
            }
			if (touchRayHit.collider.tag == "Squid") 
			{
				touchRayHit.collider.gameObject.SendMessageUpwards ("TentaTrace", touchRayHit.collider.gameObject);
			}
        }
    }

    //void OnGUI()
    //{
    //    GUI.color = Color.white;

    //    GUI.Label(new Rect(10, 20, 200, 20), "Message:    " + touchTestMessage);
    //    GUI.Label(new Rect(10, 40, 200, 20), "Message: " + previousTouchMessage);
    //}
}
