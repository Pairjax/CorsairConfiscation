using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed = 5f;
    public void Awake()
    {
        rb2d.isKinematic = true;
        rb2d.velocity = transform.TransformDirection(Vector3.right * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Spaceship chosenShip = collision.GetComponent<Spaceship>();
        if (!chosenShip || !chosenShip.category.Equals(Spaceship.SpaceshipCategory.Player))
            return;

        chosenShip.Damage();
        Destroy(gameObject);
    }
}
