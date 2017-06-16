// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: To control Larry's movements, controls ground checking and jumping. Groundcheck is always being called and jump will only be called when Larry has been swiped and is grounded.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarryControlTest : MonoBehaviour
{
    #region Values
    // Jumping
    public float jumpSpeed = 500.0f;
    public bool isGrounded = false;
    private float groundCheckDistance = 0.75f;
    private float groundedCheckTimeStamp;
    private float groundCheckMaxTime = 1.0f;
    #endregion

    void Start()
    {

    }

    void Update()
    {
        // Check if larry is on the ground or is in the air.
        GroundCheck();

        // Used for debugging to allow easy exit of the game.
        // COMMENT OUT in the final build.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Checks if Larry is touching the ground or not
    public void GroundCheck()
    {
        // Does a check every second if Larry is touching the ground, checking faster then every second can cause issues.
        if (Time.time > groundedCheckTimeStamp + groundCheckMaxTime)
        {
            // Creates a layer mask so the ray cast only checks for the ground
            int layerMask = 1 << 8;

            // Raycasts down from Larrys pivot point, it will only go a set distance only checking if it hits the ground.
            // If the ground is hit isGrounded will be set to true.
            if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, layerMask))
            {
                isGrounded = true;
            }
        }

        // For debugging to see if Larry is touching the ground and the raycasting is working properly.
        //Debug.DrawLine(transform.position, Vector3.down * 0.75f, Color.blue, 10.0f);
    }

    // Used when a swipe is done through Larry and going from bottom to up.
    // Jumping will not work if swiped top to bottom, left to right and vice versa.
    public void Jump()
    {
        // Checks the the swipe was from bottom to top and that Larry is grounded.
        if (SwipeController.Instance.IsSwiping(SwipeDirection.UP) && isGrounded)
        {
            // Create needed temp variables to be used.
            Rigidbody playerBody = GetComponent<Rigidbody>();
            float jumpInput = jumpSpeed;

            // Have Larry jump by using the rigidbody allowing gravity to effect Larry.
            // Use velocityChange so that only the force given is applied and nothing extra.
            // Sets isGrounded to false and resets the grounded check time stamp so it does not do a ground check well Larry is jumping.
            playerBody.AddForce(new Vector3(0, jumpInput, 0), ForceMode.VelocityChange);
            isGrounded = false;
            groundedCheckTimeStamp = Time.time;
            //Debug.Log("Jump");
        }
    }
}
