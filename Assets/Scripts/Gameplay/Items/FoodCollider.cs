// Author: Luigi Esposito
// Date: April 12/ 2017
// Description: Is put on an empty game object with a 2D collider, this game object will be a child of the food. This is only to allow swiping of the food to pull out of the ground.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollider : MonoBehaviour
{
    public GameObject colliderParent;

    public void PullOutOfGround(Vector2 EndPosition)
    {
        colliderParent.GetComponent<FoodController>().PullOutOfGround(EndPosition);
    }
}
