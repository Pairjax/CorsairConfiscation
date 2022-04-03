using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionEvent))]
public class Interactable : MonoBehaviour
{
    public InteractionEvent iEvent;
    public PlayerInput input;
    public SpaceshipStats spaceshipStats;
    private void OnTriggerStay2D(Collider2D other)
    {
        Spaceship chosenShip = other.GetComponent<Spaceship>();

        if (!chosenShip || chosenShip.category != Spaceship.SpaceshipCategory.Player)
            return;
        spaceshipStats = chosenShip.spaceshipStats;

        if (input.interacted)
        {
            iEvent.Activate();
        }
    }

}
