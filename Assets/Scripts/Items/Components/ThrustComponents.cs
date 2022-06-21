using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupernovaThruster : ComponentBehavior
{
    [SerializeField] private float speedLeniency = 0.9f;
    [SerializeField] private float topSpeed;
    private bool hasReducedImpactDamage;
    private Rigidbody2D pRb2d;

    void Start() 
    {
        name = "Supernova Thruster";
        pRb2d = playerController.rb2d;
    }

    public override void UpdateStats()
    {
        playerStats._accelerationMultiplier += 0.05f;
        playerStats._maxSpeedMultiplier += 0.2f;
        CalculateTopSpeed();
    }

    // TODO: Max Speed collision Damage Reduction.
    void FixedUpdate()
    {
        if (pRb2d.velocity.magnitude >= topSpeed
            && !hasReducedImpactDamage)
        {
            playerStats._collisionMultiplier -= 0.5f;
            hasReducedImpactDamage = true;
            return;
        }

        if (pRb2d.velocity.magnitude <= topSpeed
            && hasReducedImpactDamage)
        {
            playerStats._collisionMultiplier += 0.5f;
            hasReducedImpactDamage = false;
            return;
        }
    }

    private void CalculateTopSpeed()
    {
        topSpeed = playerStats._maxSpeed * playerStats._maxSpeedMultiplier * speedLeniency;
    }
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

