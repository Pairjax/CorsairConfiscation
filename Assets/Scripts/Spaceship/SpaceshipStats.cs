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

    [Header("Drops")]
    public List<DropParameters> quantity;

    [System.Serializable]
    public struct DropParameters
    {
        [SerializeField]
        Droppable drop;

        [SerializeField]
        int minQuantity;
        [SerializeField]
        int maxQuantity;
    }
}