// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Is the swipe controls as it is done differently then touch, swipe can only happen in 3D and with 3D colliders.
// If wanted can be modified with time to work with 2D colliders

// ------------ Notes --------------
// Use in the multiTouchManager this will indicate the direction of a swipe, i will be adding in angle swipe detection shortly
// Angle detection is done by adding up the direction numbers then will give you the correct angle
//if(SwipeController.Instance.IsSwiping(SwipeDirection.LEFT))
//{
//    //Debug.Log("Swipe Left");
//}
//else if (SwipeController.Instance.IsSwiping(SwipeDirection.RIGHT))
//{
//    //Debug.Log("Swipe right");
//}
//else if (SwipeController.Instance.IsSwiping(SwipeDirection.UP))
//{
//    //Debug.Log("Swipe up");
//}
//else if (SwipeController.Instance.IsSwiping(SwipeDirection.DOWN))
//{
//    //Debug.Log("Swipe down");
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    NONE = 0,
    LEFT = 1,
    RIGHT = 2,
    UP = 4,
    DOWN = 8
}

public class SwipeController : MonoBehaviour
{
    public static SwipeController instance { set; get; }
    public static SwipeController Instance { get { return instance; } }

    public SwipeDirection direction { set; get; }

    private Vector3 touchPosition;
    private Vector2 touchPosition2d;
    private float swipeResitanceX = 50.0f;
    private float swipeResitanceY = 50.0f;
    private float minSwipeDistance = 5.0f;

    // Swipe Collision
    private RaycastHit lineCastHit;
    private bool isLarrySwiped = false;
    private bool isFoodSwiped = false;
    public AudioSource swipeAudio;

    // Use this for initialization
    void Start ()
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Will possibly need to be removed after testing or resets after the touch ends
        direction = SwipeDirection.NONE;

		if(Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            touchPosition2d = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 deltaSwipe = touchPosition - Input.mousePosition;
            Vector2 deltaSwipe2d = touchPosition2d = Input.mousePosition;

            if (Mathf.Abs(deltaSwipe2d.x) > swipeResitanceX)
            {
                // Swipe on the X axis
                direction |= (deltaSwipe2d.x < 0) ? SwipeDirection.RIGHT : SwipeDirection.LEFT;
            }

            if (Mathf.Abs(deltaSwipe2d.y) > swipeResitanceY)
            {
                // Swipe on the Y axis
                direction |= (deltaSwipe2d.y < 0) ? SwipeDirection.UP : SwipeDirection.DOWN;
            }

            if (Mathf.Abs (deltaSwipe.x) > swipeResitanceX)
            {
                // Swipe on the X axis
                direction |= (deltaSwipe.x < 0) ? SwipeDirection.RIGHT: SwipeDirection.LEFT;
            }

            if (Mathf.Abs(deltaSwipe.y) > swipeResitanceY)
            {
                // Swipe on the Y axis
                direction |= (deltaSwipe.y < 0) ? SwipeDirection.UP : SwipeDirection.DOWN;
            }

            // Change variables for linecasting and distance check
            Vector3 mouseClick = Input.mousePosition;
            mouseClick.z = 1;
            mouseClick = Camera.main.ScreenToWorldPoint(mouseClick);

            touchPosition.z = 1;
            touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);

            #region SwipeDistanceCheck
            if (Vector3.Distance(touchPosition, mouseClick) < minSwipeDistance)
            {
                //Debug.Log("Did not swipe far enough");
            }
            #endregion

            #region linecasting

            if (Vector3.Distance(touchPosition, mouseClick) > minSwipeDistance)
            {
                int layerMask = 1 << 8;

                layerMask = ~layerMask;

                // Collider must be 3d or linecast will not detect it
                if (Physics.Linecast(touchPosition, mouseClick, out lineCastHit, layerMask))
                {
                    isLarrySwiped = false;
                    isFoodSwiped = false;

                    if (lineCastHit.transform.CompareTag("Larry"))
                    {
                        isLarrySwiped = true;
                    }
                    if (lineCastHit.transform.CompareTag("Shark"))
                    {
                        lineCastHit.transform.GetComponent<SharkJump>().Swiped();
                    }
                    if (lineCastHit.transform.CompareTag("Pelican"))
                    {
                        lineCastHit.transform.GetComponent<Bird>().Swiped();
                    }
                    if (lineCastHit.transform.CompareTag("GoodFood"))
                    {
                        isFoodSwiped = true;
                    }

                    if(isLarrySwiped && !isFoodSwiped)
                    {
                        lineCastHit.transform.GetComponent<LarryControlTest>().Jump();
                    }
                    else if(!isLarrySwiped && isFoodSwiped)
                    {
                        lineCastHit.transform.GetComponent<FoodCollider>().PullOutOfGround(mouseClick);
                    }
                    else if(isLarrySwiped && isFoodSwiped)
                    {
                        lineCastHit.transform.GetComponent<LarryControlTest>().Jump();
                    }
                }
                Debug.DrawLine(touchPosition, mouseClick, Color.black, 10.0f);

                swipeAudio.Play();
            }

            #endregion
        }


    }

    public bool IsSwiping(SwipeDirection SwipedDirection)
    {
        return (direction & SwipedDirection) == SwipedDirection;
    }
}
