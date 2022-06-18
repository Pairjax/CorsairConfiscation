using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentBehavior : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerInput playerInput;
    public PlayerShipController playerController;

    public void OnGenerate(PlayerStats inputStats, 
        PlayerShipController inputController)
    {
        playerStats = inputStats;
        playerController = inputController;
    }

    public virtual void UpdateStats() { }
}
