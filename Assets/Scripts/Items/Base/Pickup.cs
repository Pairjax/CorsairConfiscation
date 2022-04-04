using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerShipController>())
        {
            Debug.Log("Player Detected!");
            PickUp();
        }
    }

    public virtual void PickUp()
    {

    }
}
