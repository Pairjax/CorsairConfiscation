using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuraThick : ComponentBehavior
{
    void Start() { name = "Dura-Thick Armor"; }

    public override void UpdateStats()
    {
        playerStats._maxHPMultiplier += 0.25f;
        playerStats._massMultiplier += 0.20f;
    }
}

public class NullMassHull : ComponentBehavior
{
    void Start() { name = "Null-Mass Hull"; }

    public override void UpdateStats()
    {
        playerStats._maxHPMultiplier += 0.20f;
        playerStats._massMultiplier += 0.25f;
    }
}

public class ArcDefenseSystem : ComponentBehavior
{
    public float electrocutingDamage = 5;

    void Start() { name = "Arc Defense System"; }

    public override void UpdateStats() 
    {
        playerStats._massMultiplier += 0.05f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

    }
}