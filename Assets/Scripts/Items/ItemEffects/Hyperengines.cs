using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperengines : Powerup
{
    public override void ApplyPowerup()
    {
        base.ApplyPowerup();
        playerStats.speedUp = 1.5f;
        playerStats.healthUp = .5f;

    }

    public override void DiscardPowerup()
    {
        base.DiscardPowerup();
    }
}
