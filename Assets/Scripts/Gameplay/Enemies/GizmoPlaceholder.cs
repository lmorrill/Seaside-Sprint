using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoPlaceholder : MonoBehaviour {
    //Gizmos use as place holder for game objects without any renderers
    public Color gizmoColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
