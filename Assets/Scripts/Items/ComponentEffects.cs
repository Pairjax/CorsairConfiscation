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
                s._maxHPMultiplier += 0.25f;
                s._massMultiplier += 0.20f;
                break;
            case "Null-Mass Hull":
                s._maxHPMultiplier += 0.20f;
                s._massMultiplier += 0.25f;
                break;
            case "Arc Defense System":
                s.electrocute = true;
                break;
            case "Anti-Pursuit Net":
                s.net = true;
                break;
            case "X-3 Auto Cannon":
                s.turret = true;
                break;
            case "Energy Shield":
                s.shield = true;
                break;
            case "Supernova Thruster":
                s._accelerationMultiplier += 0.05f;
                s._maxSpeedMultiplier += 0.2f;
                s.speedShield = true;
                break;
            case "Retrofit Thruster":
                s._accelerationMultiplier += 0.1f;
                s._maxSpeedMultiplier += 0.05f;
                s._rotateSpeedMultiplier += 0.05f;
                break;
            case "Thruster Amplifier":
                s.burner = true;
                break;
            case "Piercer Harpoon":
                s.piercer = true;
                break;
            case "Multi-Harpoon Launcher":
                s.multiHarpoon = true;
                break;
            case "The Maw":
                s.maw = true;
                break;
            case "Nuclear Radar":
                s.radar = true;
                break;
            case "Asteroid Salvager":
                s.salvager = true;
                break;
            case "Automatic Repair System":
                s.repair = true;
                break;
            default:
                Debug.LogError("Loot of name " + c.name + " does not exist");
                break;
        }
    }
}
