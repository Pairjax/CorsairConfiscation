using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : ItemEffect
{
    public float healthIncrease = 1.5f;
    public float speedDecrease = .5f;
    public override void Enable()
    {
        playerStats.speedUp = speedDecrease;
        stats.maxhp *= healthIncrease;
        stats.hp *= healthIncrease;
    }
}
