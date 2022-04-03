using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public Item item;
    public static SpaceshipStats stats;
    public static PlayerStats playerStats;
    public static PlayerShipController player;

    public void Awake()
    {
        if(stats == null)
            stats = FindObjectOfType<SpaceshipStats>();
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
            player = FindObjectOfType<PlayerShipController>();
    }

    public virtual void Enable()
    {
        
    }
}
