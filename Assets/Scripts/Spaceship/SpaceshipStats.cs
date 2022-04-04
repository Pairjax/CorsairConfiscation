using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipStats : MonoBehaviour
{
    public float hp = 100;
    public float maxhp = 100;

    public void UpdateHealth(float healthMult)
    {
        hp *= healthMult;
        maxhp *= healthMult;
    }
}
