using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LootEffects
{
    public static void ApplyEffect(PlayerStats s, Droppable d)
    {
        switch (d.name)
        {
            case "Advanced Sensors":
                s._cameraSize += 0.05f;
                break;
            case "Auxiliary Service Cable":
                s._hookMaxLength += 5;
                break;
            case "Hyperfuel":
                s._maxSpeed += 5f;
                s._acceleration += 0.5f;
                break;
            case "Impact Dampener":
                s._collisionMultiplier -= 0.05f;
                break;
            case "Improved Hull Skeleton":
                s._maxHP += 5;
                break;
            case "Modulated Harvestor":
                s._scrapBonus += 0.1f;
                break;
            case "Rest Station Rewards Card":
                s._shopPriceModifier -= 0.05f;
                break;
            case "SPF Signal Jammer":
                s._bountyMultiplier -= 0.05f;
                break;
            case "Starship Ballast":
                
                break;
            default:
                Debug.LogError("Loot of name " + d.name + " exists.");
                break;
        }
    }
}
