using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/PlayerShipClass")]
public class PlayerShipClass : ScriptableObject
{
    public float baseAcceleration;
    public float baseMaxSpeed;
    public float baseLinearDrag;
    public float baseRotateSpeed;

    public float hookMinLength = 0.2f;
    public float hookMaxLength = 15;
}
