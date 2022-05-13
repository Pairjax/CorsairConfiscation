using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public bool isSalvagable;
    public bool isThrown;
    public GameObject objectSprite;
    public GameObject thrownObject;
    public float weight;
    public Rigidbody2D rb2d;

    public void Start()
    {
        if(isThrown)
        {
            // throw in direction
        }
    }

}
