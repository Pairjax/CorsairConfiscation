using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentEffects
{
    public static void ApplyEffect(PlayerStats s, ShipComponent c)
    {
        if (c == null)
            return;

        switch (c.name)
        {
            case "Dura-Thick Armor":
                s._maxHP += 0.25f;
                break;
            default:
                Debug.LogError("Loot of name " + c.name + " does not exist");
                break;
        }
    }
}
