using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerShipClass baseClass;

    [Header("Health")]
    public float _hp;
    public float _regen;

    public float _maxHP;
    public float _maxHPMultiplier;

    [Header("Movement")]
    public float _acceleration;
    public float _accelerationMultiplier;

    public float _maxSpeed;
    public float _maxSpeedMultiplier;

    public float _linearDrag;

    public float _rotateSpeed;
    public float _rotateSpeedMultiplier;

    public float _mass;
    public float _massMultiplier;

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
        _maxHPMultiplier = 1;
        _accelerationMultiplier = 1;
        _maxSpeedMultiplier = 1;
        _rotateSpeedMultiplier = 1;
        _massMultiplier = 1;

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

        UpdateEffects();
    }

    public void AddComponent(ShipComponent c)
    {
        switch (c.type)
        {
            case ComponentType.Hull:
                components.hullSlot = c;
                components.hullBehavior = c.behavior
                    .GetComponent<ComponentBehavior>();
                break;
            case ComponentType.Top_Mount:
                components.mountSlot = c;
                components.mountBehavior = c.behavior
                    .GetComponent<ComponentBehavior>();
                break;
            case ComponentType.Thruster:
                components.thrustSlot = c;
                components.thrustBehavior = c.behavior
                    .GetComponent<ComponentBehavior>();
                break;
            case ComponentType.Harpoon:
                components.harpoonSlot = c;
                components.harpoonBehavior = c.behavior
                    .GetComponent<ComponentBehavior>();
                break;
            case ComponentType.Auxiliary:
                components.auxSlot = c;
                components.auxBehavior = c.behavior
                    .GetComponent<ComponentBehavior>();
                break;
            default:
                Debug.LogError("Invalid type found. Is the ShipComponent null?");
                break;
        }

        UpdateEffects();
    }

    public void UpdateEffects()
    {
        // Base stats
        _maxHP = baseClass.hp;

        _acceleration = baseClass.acceleration;
        _maxSpeed = baseClass.maxSpeed;
        _linearDrag = baseClass.linearDrag;
        _rotateSpeed = baseClass.rotateSpeed;
        _mass = baseClass.weight;

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

        // Update loot effects
        foreach (KeyValuePair<Droppable, int> l in loot)
        {
            Droppable d = l.Key;

            LootEffects.ApplyEffect(this, d);
        }

        // Component slot effects updated
        components.hullBehavior.UpdateStats();
        components.mountBehavior.UpdateStats();
        components.thrustBehavior.UpdateStats();
        components.harpoonBehavior.UpdateStats();
        components.auxBehavior.UpdateStats();

        // Multipliers applied.
        _maxHP *= _maxHPMultiplier;
        _acceleration *= _accelerationMultiplier;
        _rotateSpeed *= _rotateSpeedMultiplier;
        _mass *= _massMultiplier;
    }

    [System.Serializable]
    public struct ComponentSlots
    {
        public List<ShipComponent> storage;

        public ShipComponent hullSlot;
        public ComponentBehavior hullBehavior;

        public ShipComponent mountSlot;
        public ComponentBehavior mountBehavior;

        public ShipComponent thrustSlot;
        public ComponentBehavior thrustBehavior;

        public ShipComponent harpoonSlot;
        public ComponentBehavior harpoonBehavior;

        public ShipComponent auxSlot;
        public ComponentBehavior auxBehavior;
    }
}
