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
        if (stats.components.mountBehavior != null)
        {
            EnergyShield s;
            if (stats.components.mountBehavior.TryGetComponent<EnergyShield>(out s))
                amount = HandleEnergyShield(s, amount);
        }

        stats._hp -= amount;

        if (stats._hp <= 0)
        {
            //display the game over screen when the player's health reaches 0
            GameOverScreen.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }

    private float HandleEnergyShield(EnergyShield shield, float damage)
    {
        float difference = shield.hp - damage;

        if (difference < 0)
        {
            shield.hp = 0;
            return Mathf.Abs(difference);
        }

        shield.hp = difference;

        return 0;
    }
}
