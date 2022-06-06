using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerShipClass baseClass;

    [Header("Health")]
    public float _hp;
    public float _maxHP;

    [Header("Movement")]
    public float _acceleration;
    public float _maxSpeed;
    public float _linearDrag;
    public float _rotateSpeed;
    public float _weight;

    [Header("Harpoon")]
    public int numHooks;
    public float _hookMaxLength;
    public float _hookSwingMax;
    public float _hookLaunchSpeed;
    public float _hookCooldown;

    [Header("Econ")]
    public float _scrapBonus;
    public float _lootBonus;
    public float _shopPriceModifier;

    [Header("Misc")]
    public float _cameraSize;
    public float _bountyMultiplier;
    public float _collisionMultiplier;

    // Booties
    public Dictionary<Droppable, int> scrap = new Dictionary<Droppable, int>();

    // Looties
    public Dictionary<Droppable, int> loot = new Dictionary<Droppable, int>();

    // Componenties
    public ComponentSlots components;

    void Start()
    {
        UpdateEffects();
        _hp = _maxHP;
    }

    public void AddScrap(Droppable drop)
    {
        if (scrap.ContainsKey(drop))
            scrap[drop]++;
        else
            scrap.Add(drop, 1);
    }

    public void AddLoot(Droppable drop)
    {
        if (loot.ContainsKey(drop))
            loot[drop]++;
        else
            loot.Add(drop, 1);

        LootEffects.ApplyEffect(this, drop);
    }

    public void AddComponent(ShipComponent c)
    {
        switch (c.type)
        {
            case ComponentType.Hull:
                components.hullSlot = c;
                break;
            case ComponentType.Top_Mount:
                components.mountSlot = c;
                break;
            case ComponentType.Thruster:
                components.thrustSlot = c;
                break;
            case ComponentType.Harpoon:
                components.harpoonSlot = c;
                break;
            case ComponentType.Auxiliary:
                components.auxSlot = c;
                break;
            default:
                Debug.LogError("Invalid type found. Is the ShipComponent null?");
                break;
        }
    }

    public void UpdateEffects()
    {
        _maxHP = baseClass.hp;

        _acceleration = baseClass.hp;
        _maxSpeed = baseClass.maxSpeed;
        _linearDrag = baseClass.linearDrag;
        _rotateSpeed = baseClass.rotateSpeed;
        _weight = baseClass.weight;

        _hookMaxLength = baseClass.hookMaxLength;
        _hookSwingMax = baseClass.hookSwingMax;
        _hookLaunchSpeed = baseClass.hookLaunchSpeed;
        _hookCooldown = baseClass.hookCooldown;

        _scrapBonus = baseClass.scrapBonus;
        _lootBonus = baseClass.lootBonus;
        _shopPriceModifier = baseClass.shopPriceModifier;

        _cameraSize = baseClass.cameraSize;
        _bountyMultiplier = baseClass.bountyMultiplier;
        _collisionMultiplier = baseClass.collisionMultiplier;

        foreach (KeyValuePair<Droppable, int> l in loot)
        {
            Droppable d = l.Key;

            LootEffects.ApplyEffect(this, d);
        }
    }

    [System.Serializable]
    public struct ComponentSlots
    {
        public List<ShipComponent> storage;
        public ShipComponent hullSlot;
        public ShipComponent mountSlot;
        public ShipComponent thrustSlot;
        public ShipComponent harpoonSlot;
        public ShipComponent auxSlot;
    }
}
