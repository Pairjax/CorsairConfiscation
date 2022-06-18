using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentTracker : MonoBehaviour
{
    public Image hull;
    public Image mount;
    public Image thrust;
    public Image harpoon;
    public Image aux;

    public PlayerStats pStats;

    public Sprite defaultSprite;

    public void RefreshComponentVisuals()
    {
        hull.sprite = ChooseSprite(pStats.components.hullSlot);
        mount.sprite = ChooseSprite(pStats.components.mountSlot);
        thrust.sprite = ChooseSprite(pStats.components.thrustSlot);
        harpoon.sprite = ChooseSprite(pStats.components.harpoonSlot);
        aux.sprite = ChooseSprite(pStats.components.auxSlot);
    }

    public Sprite ChooseSprite(ShipComponent c)
    {
        return c == null ? defaultSprite : c.sprite;
    }
}
