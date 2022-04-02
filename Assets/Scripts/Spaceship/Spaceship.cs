using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceshipStats))]
public class Spaceship : MonoBehaviour
{
    public enum SpaceshipCategory { Cop, Player, Civilian }
    public SpaceshipCategory category;

    public SpaceshipStats spaceshipStats;

    public void Damage()
    {
        spaceshipStats.lives--;

        if (spaceshipStats.lives <= 0)
        {
            Debug.Log("Killed!");
            Destroy(gameObject);
        }
    }

    
}
