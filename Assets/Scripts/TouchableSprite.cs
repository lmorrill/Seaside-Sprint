// --------- Notes ----------
// the original script given by Micheal for touch input use as reference ONLY

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TouchableSprite : MonoBehaviour {

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
            slope = (float)((Math.Atan2((start.y - end.y), (start.x - end.x))) * 180/Math.PI);
        }
    }

    #region Touch Variables
    public float ScaleMultiplier = 1.0f;
    public  float RotationMultiplier = 1.0f;

    public bool AllEnabled = false;

    public bool DragEnabled = false;
    public bool RotateEnabled = false;
    public bool ScaleEnabled = false;

    private List<Touch> currentTouchPoints;
    private List<Touch> previousTouchPoints;
    #endregion
    
    // Use this for initialization
    void Start ()
    {
        #region Touch Initialization
        currentTouchPoints = new List<Touch>();
        previousTouchPoints = new List<Touch>();

        if (AllEnabled)
        {
            DragEnabled = true;
            RotateEnabled = true;
            ScaleEnabled = true;
        }
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        #region Touch Controls

        #region Drag
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
        #endregion


        if (currentTouchPoints.Count == 2)
        {
            Line currentLine = new Line(currentTouchPoints[0].position, currentTouchPoints[1].position);
            Line previousLine = new Line(previousTouchPoints[0].position, previousTouchPoints[1].position);

            if (previousTouchPoints.Count == 0)
            {

            }

            else
            {
                #region Rotate
                if (RotateEnabled)
                {
                    if (currentLine.slope != previousLine.slope)
                    {
                        RotateObject(currentLine.slope - previousLine.slope);
                    }
                }
                #endregion

                #region Scale
                if (ScaleEnabled)
                {
                    float scalar = currentLine.length - previousLine.length;

                    ScaleObject(scalar);
                }
                #endregion
            }
        }

        previousTouchPoints = currentTouchPoints;
        currentTouchPoints = new List<Touch>();
        #endregion
    }

    private void DragObject(Touch touch)
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(touch.position);

        //reset z position else z will end up being the z for the camera.
        newPosition.z = transform.localPosition.z;

        transform.localPosition = newPosition;
    }

    private void RotateObject(float angle)
    {
        float rotationAngle = transform.rotation.z + (angle * RotationMultiplier);

        transform.Rotate(0, 0, rotationAngle);
    }

    private void ScaleObject(float scalar)
    {
        transform.localScale += (new Vector3(scalar * ScaleMultiplier, scalar * ScaleMultiplier, 0));
    }

    public void AddTouchPoint(Touch touch)
    {
        currentTouchPoints.Add(touch);
    }
}
