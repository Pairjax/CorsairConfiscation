using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public bool isSalvagable;
    public bool isThrown = false;
    public GameObject objectSprite;
    public GameObject thrownObject;
    public float weight;
    public Rigidbody2D rb2d;

    public void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    /*public void OnThrow()
    {
        isThrown = true;

        gameObject.AddComponent<Collectible>();

        rb2d.SetRotation(Random.rotation);
        rb2d.AddForce(Vector2.left * Random.value);
    }*/

}
