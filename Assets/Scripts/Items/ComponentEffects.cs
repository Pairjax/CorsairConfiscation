using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentEffects : MonoBehaviour
{
    public static void ApplyEffect(PlayerStats s, ShipComponent c)
    {
        switch (c.name)
        {
            case "Anti-Pursuit Net":
                break;
            default:
                Debug.LogError("Loot of name " + c.name + " exists.");
                break;
        }
    }
}
