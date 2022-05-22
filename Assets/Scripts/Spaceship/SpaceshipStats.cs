using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[CreateAssetMenu(fileName = "Character", menuName = "Characters / ShipStats")]
public class SpaceshipStats : ScriptableObject
{
    [Header("Health")]
    public float hp;
    public float maxhp;

    [Header("Score")]
    public int bountyPoints;

    [Header("Movement")]
    public float speed;
    public float linDrag;

    [Header("Appearance")]
    public Sprite[] spriteSelection = null;
}
