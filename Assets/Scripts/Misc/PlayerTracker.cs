using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public Transform target;

    void FixedUpdate()
    {
        if(target != null)
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
