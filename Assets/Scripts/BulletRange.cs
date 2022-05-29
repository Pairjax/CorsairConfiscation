using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRange : MonoBehaviour
{
    public CopShipController parentShip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Spaceship chosenShip = collision.GetComponent<Spaceship>();
        if (!chosenShip || !chosenShip.category.Equals(Spaceship.SpaceshipCategory.Player))
            return;

        parentShip.BeginFiring(chosenShip.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Spaceship chosenShip = collision.GetComponent<Spaceship>();
        if (!chosenShip || !chosenShip.category.Equals(Spaceship.SpaceshipCategory.Player))
            return;

        parentShip.EndFiring();
        parentShip.BeginWandering();
    }
}
