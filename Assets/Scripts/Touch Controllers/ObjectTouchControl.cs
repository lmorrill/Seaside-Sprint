// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: To be put on any object that will be allowed to be touched, in the inspector you will need to select what touch motions are allowed on the game object.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectTouchControl : MonoBehaviour
{
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

    // General
    public bool AllEnabled = false;

    public bool DragEnabled = false;
    public bool RotateEnabled = false;
    public bool ScaleEnabled = false;

    private List<Touch> currentTouchPoints;
    private List<Touch> previousTouchPoints;

    // Scaling and rotation
    public float ScaleMultiplier = .01f;
    public float RotationMultiplier = 1.0f;

    private string message;

    // Use this for initialization
    void Start()
    {
        currentTouchPoints = new List<Touch>();
        previousTouchPoints = new List<Touch>();

        if (AllEnabled)
        {
            DragEnabled = true;
            RotateEnabled = true;
            ScaleEnabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DragEnabled)
        {
            if (currentTouchPoints.Count == 1)
            {
                if (currentTouchPoints[0].phase == TouchPhase.Moved)
                {
                    DragObject(currentTouchPoints[0]);
                }
            }
        }

        if (currentTouchPoints.Count == 2)
        {
            Line currentLine = new Line(currentTouchPoints[0].position, currentTouchPoints[1].position);
            Line previousLine = new Line(previousTouchPoints[0].position, previousTouchPoints[1].position);

            if (previousTouchPoints.Count == 0)
            {

            }

            else
            {
                // Object needs to have a bas scale of 1 or the way touch input is done will not detect the object.
                // Larry will need to be bigger in scale so his hands can have a scale of 1 and should be detected then.
                if (RotateEnabled)
                {
                    if (currentLine.slope != previousLine.slope)
                    {
                        RotateObject(currentLine.slope - previousLine.slope);
                    }
                }

                if (ScaleEnabled)
                {
                    float scalar = currentLine.length - previousLine.length;

                    ScaleObject(scalar);
                    message = "Scaling";
                }
            }
        }

        previousTouchPoints = currentTouchPoints;
        currentTouchPoints = new List<Touch>();
    }

    //void OnGUI()
    //{
    //    GUI.color = Color.black;

    //    GUI.Label(new Rect(10, 20, 200, 20), "Message:    " + message);
    //}

    private void DragObject(Touch touch)
    {
        Vector3 touchPoint = touch.position;
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPoint.x, touchPoint.y, transform.position.z));

        //reset z position else z will end up being the z for the camera.
        newPosition.z = transform.localPosition.z;

        transform.localPosition = newPosition;

        if (transform.tag == "GoodFood")
        {
            GetComponent<FoodController>().isFoodBeingDragged = true;
        }
    }

    private void RotateObject(float angle)
    {
        float rotationAngle = transform.rotation.z + (angle * RotationMultiplier);

        transform.Rotate(0, 0, rotationAngle);
    }

    /// <summary>
    /// Use to scale the designated game object
    /// </summary>
    /// <param name="scalar">The size at which to be scaled at, is done by using the current and previous lines.</param>
    private void ScaleObject(float scalar)
    {
        if (gameObject.tag == "GoodFood")
        {
            // Check that the object is between min and max scale size to stop from over and under scaleing
            // Allow object to be scaled if between the min and the max.
            // The food will handle any other actions that scaling effects.
            if(transform.localScale.x <= 1.5f && transform.localScale.x >= 1)
            {
                transform.localScale += (new Vector3(scalar * ScaleMultiplier, scalar * ScaleMultiplier, 0));

                if(transform.localScale.x > 1.5f)
                {
                    transform.localScale = new Vector3(1.5f, 1.5f, 0);
                }

                if(transform.localScale.x < 1)
                {
                    transform.localScale = new Vector3(1, 1, 0);
                }
            }
        }
        else
        {
            transform.localScale += (new Vector3(scalar * ScaleMultiplier, scalar * ScaleMultiplier, 0));
        }
    }

    public void AddTouchPoint(Touch touch)
    {
        currentTouchPoints.Add(touch);
    }
}
