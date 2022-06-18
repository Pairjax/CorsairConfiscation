using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Spaceship
{
    public PlayerStats stats;
    public PlayerShipController controller;

    public override void SetupStats()
    {
        stats.UpdateEffects();
    }

    public override void Damage(float amount)
    {
        if (stats.components.mountBehavior.GetComponent<EnergyShield>() != null)
        {

        }

        stats._hp -= amount;

        if (stats._hp <= 0)
        {
            //display the game over screen when the player's health reaches 0
            GameOverScreen.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }
}
