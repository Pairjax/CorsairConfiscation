using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    public CopShipController parentShip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Spaceship chosenShip = collision.GetComponent<Spaceship>();
        if (!chosenShip || !chosenShip.category.Equals(Spaceship.SpaceshipCategory.Player))
            return;

        switch(parentShip.enemyType)
        {
            case CopShipController.EnemyType.Cruiser:
                parentShip.BeginFollow(chosenShip.transform, .5f);
                break;
            case CopShipController.EnemyType.Seeker:
                parentShip.BeginFollow(chosenShip.transform, .5f);
                break;
            case CopShipController.EnemyType.Brute:
                parentShip.FlyTowards(chosenShip.transform);
                break;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Spaceship chosenShip = collision.GetComponent<Spaceship>();
        if (!chosenShip || !chosenShip.category.Equals(Spaceship.SpaceshipCategory.Player))
            return;

        switch (parentShip.enemyType)
        {
            case CopShipController.EnemyType.Brute:
                parentShip.FlyTowards(chosenShip.transform);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Spaceship chosenShip = collision.GetComponent<Spaceship>();
        if (!chosenShip || !chosenShip.category.Equals(Spaceship.SpaceshipCategory.Player))
            return;

        switch (parentShip.enemyType)
        {
            case CopShipController.EnemyType.Cruiser:
                parentShip.EndFollow();
                break;
            case CopShipController.EnemyType.Seeker:
                parentShip.EndFollow();
                break;
            case CopShipController.EnemyType.Brute:
                parentShip.Abort();
                break;

        }
    }
}
