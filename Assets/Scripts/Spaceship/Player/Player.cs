using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Spaceship
{
    public PlayerStats stats;
    public override void SetupStats()
    {
    }

    public override void Damage()
    {
        stats.hp -= 10;

        if (stats.hp <= 0)
        {
            //display the game over screen when the player's health reaches 0
            GameOverScreen.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }
}
