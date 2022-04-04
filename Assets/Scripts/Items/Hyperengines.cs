using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperengines : Powerup
{
    public override void ApplyPowerup()
    {
        base.ApplyPowerup();
        playerStats.speedUp = 2f;
        playerStats.healthUp = .5f;
        shipStats.UpdateHealth(playerStats.healthUp);
    }

    public override void DiscardPowerup()
    {
        base.DiscardPowerup();
    }
}
