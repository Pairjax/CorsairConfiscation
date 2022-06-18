using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupernovaThruster : ComponentBehavior
{
    void Start() { name = "Supernova Thruster"; }

    public override void UpdateStats()
    {
        playerStats._accelerationMultiplier += 0.05f;
        playerStats._maxSpeedMultiplier += 0.2f;
    }

    // TODO: Max Speed collision Damage Reduction.
}

public class RetrofitThruster : ComponentBehavior
{
    void Start() { name = "Retrofit Thruster"; }

    public override void UpdateStats()
    {
        playerStats._accelerationMultiplier += 0.1f;
        playerStats._maxSpeedMultiplier += 0.05f;
        playerStats._rotateSpeedMultiplier += 0.05f;
    }
}

public class ThrusterAmplifier : ComponentBehavior
{
    [SerializeField] private float burnDamage;

    void Start() { name = "Thruster Amplifier"; }

    void OnTriggerStay2D(Collision2D collision)
    {
        Spaceship s;

        if (collision.gameObject.TryGetComponent<Spaceship>(out s)
            && collision.gameObject.tag != "Player")
        {
            s.Damage(burnDamage * Time.fixedDeltaTime);
        }
    }

}

