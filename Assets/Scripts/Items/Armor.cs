using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Powerup
{
    public override void ApplyPowerup()
    {
        base.ApplyPowerup();
        playerStats.speedUp = .5f;
        playerStats.healthUp = 2f;
        shipStats.UpdateHealth(playerStats.healthUp);
    }

    public override void DiscardPowerup()
    {
        base.DiscardPowerup();
    }
}
