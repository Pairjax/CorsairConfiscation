using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriBeam : Powerup
{
    public override void ApplyPowerup()
    {
        base.ApplyPowerup();
        playerStats.beamCount = 3;
    }

    public override void DiscardPowerup()
    {
        base.DiscardPowerup();
    }
}
