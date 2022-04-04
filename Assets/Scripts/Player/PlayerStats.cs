using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int dubloons = 0;
    public int gotcha = 0;
    public int maxGotcha = 100;
    public int beamCount = 1;

    // Booties
    public float speedUp = 0f;
    public int damageUp = 0;
    public float healthUp = 0f;
    public int beamLengthUp = 0;
    public int beamPlus = 0;

    public PlayerShipController shipController;
    private void Update()
    {
        if (beamCount == 3 && !shipController.tractorBeams[1] && !shipController.tractorBeams[2])
        {
            shipController.AddTractors();
        }
    }
}
