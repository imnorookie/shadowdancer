using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Vector3 Position
    {
        get { return gameObject.transform.position; }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawSphere(gameObject.transform.position, 0.5f);
    //}
}
