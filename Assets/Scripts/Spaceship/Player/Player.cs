using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Spaceship
{
    public PlayerStats stats;
    public override void SetupStats()
    {
        stats.SetupStats(spaceshipStats);
    }

    public override void Damage()
    {
        stats._hp -= 10;

        if (stats._hp <= 0)
        {
            //display the game over screen when the player's health reaches 0
            GameOverScreen.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }
}
