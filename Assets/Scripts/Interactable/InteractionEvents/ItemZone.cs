using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemZone : InteractionEvent
{
    public Item item;
    public override void Activate()
    {
        interactionMessage = $"ItemZone! Press E to purchase {item} for {cost} dabloons...";
        base.Activate();

        if (CanUse())
        {
            Debug.Log($"{item._name} picked up!");
            item.effect.Enable();
            StartCountdown();
        }
        else
        {
            Debug.Log($"ItemZone cannot be used for {timeLeft} seconds!");
        }

    }

    
}
