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

        parentShip.playerShip = chosenShip;
        parentShip.SetAIState(CopShipController.ShipAIState.Pursue);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Spaceship chosenShip = collision.GetComponent<Spaceship>();
        
        if (!chosenShip || chosenShip != parentShip.playerShip)
            return;

        parentShip.ResetAI();
    }
}
