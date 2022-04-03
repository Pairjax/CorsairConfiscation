using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperengines : ItemEffect
{
    public float speedIncrease = 1.5f;
    public float healthDecrease = .5f;
    public override void Enable()
    {
        playerStats.speedUp = speedIncrease;
        stats.maxhp *= healthDecrease;
        stats.hp *= healthDecrease;
    }
}
