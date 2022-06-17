using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;

[CreateAssetMenu(menuName = "Destructible/Settings")]
public class DestructibleSettings : ScriptableObject 
{
    [Header("Settings")]
    public bool canDamage;
    public bool requireDamageToFracture;
    public int pointsPerPixel;
    public float damageThreshold;
    public float damageRequiredForFracture;
    public D2dDestructible.PixelsType pixels;
    public bool fade;
    public float lifeMin;
    public float lifeMax;
    public enum DestructibleType { Asteroid, NPCShip };
    public DestructibleType dType;
}
