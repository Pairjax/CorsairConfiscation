using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Component")]
public class ShipComponent : ScriptableObject
{
    public Sprite sprite;
    public ComponentType type;
    public GameObject behavior;
    public List<CostParameters> price;
    public string description;
    public string effect;
}

public enum ComponentType
{
    Hull,
    Top_Mount,
    Thruster,
    Harpoon,
    Auxiliary
}

[System.Serializable]
public struct CostParameters
{
    [SerializeField]
    public Droppable drop;

    [SerializeField]
    public int quantity;
}