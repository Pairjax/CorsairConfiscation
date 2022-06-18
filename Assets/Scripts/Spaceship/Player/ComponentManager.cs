using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    [Header("Manager References")]
    public ComponentTracker componentTracker;
    public PlayerStats pStats;
    public PlayerInput pInput;

    [Header("Player References")]
    public PlayerShipController pController;

    [Header("Components")]
    public GameObject currentHullComponent;
    public GameObject currentMountComponent;
    public GameObject currentThrustComponent;
    public GameObject currentHarpoonComponent;
    public GameObject currentAuxComponent;

    public void ConstructComponent(ShipComponent c)
    {
        componentTracker.RefreshComponentVisuals();

        switch (c.type)
        {
            case ComponentType.Hull:
                currentHullComponent = Instantiate(c.behavior);
                ComponentSetup(currentHullComponent);
                break;
            case ComponentType.Top_Mount:
                currentMountComponent = Instantiate(c.behavior);
                ComponentSetup(currentMountComponent);
                break;
            case ComponentType.Thruster:
                currentThrustComponent = Instantiate(c.behavior);
                ComponentSetup(currentThrustComponent);
                break;
            case ComponentType.Harpoon:
                currentHarpoonComponent = Instantiate(c.behavior);
                ComponentSetup(currentHarpoonComponent);
                break;
            case ComponentType.Auxiliary:
                currentAuxComponent = Instantiate(c.behavior);
                ComponentSetup(currentAuxComponent);
                break;
            default:
                Debug.LogError("Invalid type found. Is the ShipComponent null?");
                break;
        }
    }

    private void ComponentSetup(GameObject componentObj)
    {
        componentObj.transform.parent = transform;

        ComponentBehavior behavior = componentObj.GetComponent<ComponentBehavior>();

        behavior.playerStats = pStats;
        behavior.playerController = pController;
        behavior.playerInput = pInput;
    }
}
