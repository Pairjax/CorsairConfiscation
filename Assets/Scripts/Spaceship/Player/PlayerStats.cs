using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int dubloons = 0;
    public int gotcha = 0;
    public int maxGotcha = 100;
    public int beamCount = 1;
    public float hookMinLength = 2;
    public float hookMaxLength = 15;

    // Booties
    public float speedUp = 0f;
    public int damageUp = 0;
    public float healthUp = 0f;
    public int beamLengthUp = 0;
    public int beamPlus = 0;

    // Stats
    public float hp;
    public float maxhp;

    public void SetupStats(SpaceshipStats stats)
    {
        hp = stats.hp;
        maxhp = stats.maxhp;
    }
}
