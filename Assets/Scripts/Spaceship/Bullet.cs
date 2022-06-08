using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private CopShipController parentShip;
    public Rigidbody2D rb2d;
    public float speed = 5f;
    public void Awake()
    {
        rb2d.isKinematic = true;
        rb2d.velocity = transform.TransformDirection(Vector3.right * speed);
    }
    public void SetParent(CopShipController copShip)
    {
        parentShip = copShip;
    }
    private void OnDestroy()
    {
        if (parentShip != null)
            parentShip.spawnedBullets.Remove(gameObject);
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
