using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public float cooldown;
    public string _name;
    public string description;
    public Sprite sprite;
    public ItemEffect effect;
}
