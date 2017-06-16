// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Controls the rotation of Larry's hands, requires both hands to be touched to rotate. This DOES NOT handle collision with the hand.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LarryHandController : MonoBehaviour
{
    // Line created from the current touch positions to each other
    // Used for rotation and scaling
    private struct Line
    {
        Vector2 start;
        Vector2 end;
        public float slope;
        public float length;

        public Line(Vector2 _start, Vector2 _end)
        {
            start = _start;
            end = _end;

            length = Vector2.Distance(start, end);
            slope = (float)((Math.Atan2((start.y - end.y), (start.x - end.x))) * 180 / Math.PI);
        }
    }

    #region Values
    // Hands
    public bool isLeftHandSelected = false;
    public bool isRightHandSelected = false;

    // Movement
    public float RotationMultiplier = 1.0f;

    // Touch
    private List<Touch> currentTouchPoints;
    private List<Touch> previousTouchPoints;
    #endregion

    void Start()
    {
        currentTouchPoints = new List<Touch>();
        previousTouchPoints = new List<Touch>();
    }

    void Update()
    {
        // Does a check that there is 2 touch positions, then creates the lines from each touch position.
        if (currentTouchPoints.Count == 2)
        {
            Line currentLine = new Line(currentTouchPoints[0].position, currentTouchPoints[1].position);
            Line previousLine = new Line(previousTouchPoints[0].position, previousTouchPoints[1].position);

            // Checks that the both hands have been selected, then allows the user to rotate the hands.
            // The rotation is based off of the 2 touch positions and the line created from the positions.
            if (isLeftHandSelected && isRightHandSelected)
            {
                // Allow to rotate the hands now
                // Checks that a movement has happened between the touches, meaning the user is attempting to rotate the hands.
                if (currentLine.slope != previousLine.slope)
                {
                    RotateObject(currentLine.slope - previousLine.slope);
                }
            }
        }

        // Reset hands values, they will not reset if the hand is still being touched but only when it is not being touched
        isLeftHandSelected = false;
        isRightHandSelected = false;

        // Set previous touches to current touches then reset current touches
        previousTouchPoints = currentTouchPoints;
        currentTouchPoints = new List<Touch>();
    }

    // Used to rotate the hands, uses and passed in angle to calculate the angle of the rotation.
    private void RotateObject(float angle)
    {
        // Create the angle needed for the rotation.
        float rotationAngle = transform.rotation.z + (angle * RotationMultiplier);

        // Apply the rotation angle to the hands but only rotate on the Z axis.
        transform.Rotate(0, 0, rotationAngle);
    }


    // Use for build Debugging
    //void OnGUI()
    //{
    //    GUI.color = Color.black;

    //    GUI.Label(new Rect(10, 20, 200, 20), "Left:    " + isLeftHandSelected);
    //    GUI.Label(new Rect(10, 40, 200, 20), "Right: " + isRightHandSelected);
    //    GUI.Label(new Rect(10, 60, 200, 20), "" + currentTouchPoints.Count);
    //}

    // Used when a hand is being touched to add the touch point to current touches.
    public void AddTouchPoint(Touch touch)
    {
        // Add the touch to current touch point.
        currentTouchPoints.Add(touch);
    }
}
