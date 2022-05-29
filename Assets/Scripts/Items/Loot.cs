using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/Loot")]
public abstract class Loot
{
    public Sprite sprite;
    public Player target;

    public void Effect()
    {

    }
}