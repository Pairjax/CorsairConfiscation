using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/PlayerShipClass")]
public class PlayerShipClass : ScriptableObject
{
    [Header("Health")]
    public int hp;

    [Header("Movement")]
    public float acceleration;
    public float maxSpeed;
    public float linearDrag;
    public float rotateSpeed;
    public float weight;

    [Header("Harpoon")]
    public int numHooks;
    public float hookMaxLength;
    public float hookSwingMax;
    public float hookCooldown;

    [Header("Econ")]
    public float scrapBonus;
    public float lootBonus;
    public float shopPriceModifier;

    [Header("Misc")]
    public float cameraSize;
    public float bountyMultiplier;
    public float collisionMultiplier;
}
