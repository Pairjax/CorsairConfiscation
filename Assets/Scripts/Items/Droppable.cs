using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Droppable")]
public class Droppable : ScriptableObject
{
    public Sprite sprite;
    public DropType type;
    // Room for other features?
}

public enum DropType
{
    Scrap,
    Loot
}