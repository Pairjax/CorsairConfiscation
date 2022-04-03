using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthZone : InteractionEvent
{
    public int healAmount;
    public override void Activate()
    {
        interactionMessage = $"Health Zone! Press E to purchase for {cost} dabloons...";

        base.Activate();

        if (CanUse())
        {
            Debug.Log("Healing!");
            if (interactable.spaceshipStats.hp + healAmount > interactable.spaceshipStats.maxhp)
                interactable.spaceshipStats.hp = interactable.spaceshipStats.maxhp;
            else
                interactable.spaceshipStats.hp += healAmount;
            StartCountdown();
        }
        else
        {
            Debug.Log($"HealthZone cannot be used for {timeLeft} seconds!");
        }
    }

}
