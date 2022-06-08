using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Droppable")]
public class Droppable : ScriptableObject
{
    public Sprite sprite;
    public DropType type;
    public string scriptName;
}

public enum DropType
{
    Scrap,
    Loot
}