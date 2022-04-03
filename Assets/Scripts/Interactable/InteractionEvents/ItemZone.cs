using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemZone : InteractionEvent
{
    public GameObject item;
    public override void Activate()
    {
        interactionMessage = $"ItemZone! Press E to purchase {item} for {cost} dabloons...";
        base.Activate();

        if (CanUse())
        {
            Debug.Log("Item picked up!");

            StartCountdown();
        }
        else
        {
            Debug.Log($"ItemZone cannot be used for {timeLeft} seconds!");
        }

    }

    
}
