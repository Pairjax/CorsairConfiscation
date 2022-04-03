using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InteractionEvent : MonoBehaviour
{
    public Interactable interactable;
    public int cost = 0;
    public float cooldown = 120f;
    public string interactionMessage { private get; set; }
    
    public virtual void Activate()
    {
        // Peyton: Logic for displaying interaction message to UI.
    }
    public float timeLeft;
    public bool CanUse()
    {
        if (lastActivated == -1f)
            return true;
        else if (Time.time - lastActivated < cooldown)
        {
            timeLeft = cooldown - (Time.time - lastActivated);
            return false;
        }

        //if (PlayerMoney < cost)
        //    return false;

        return true;
    }

    float lastActivated = -1f;
    public void StartCountdown()
    {
        lastActivated = Time.time;
    }
}
