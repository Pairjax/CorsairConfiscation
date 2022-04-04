using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : Pickup
{
    public float duration;
    public GameObject pickupSprite;
    internal static PlayerStats playerStats;
    internal static SpaceshipStats shipStats;

    public GameObject spawnedSprite;
    public void Awake()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();
        if (shipStats == null)
            shipStats = playerStats.GetComponent<SpaceshipStats>();
    }
    public override void PickUp()
    {
        StartCoroutine(ActivatePowerup());
    }

    private IEnumerator ActivatePowerup()
    {
        
        ApplyPowerup();
        yield return new WaitForSeconds(duration);
        DiscardPowerup();

    }

    public virtual void ApplyPowerup()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        EnableEffect();
    }
    public virtual void DiscardPowerup()
    {
        Destroy(spawnedSprite);
        Destroy(gameObject);
    }
    public void EnableEffect()
    {
        if (pickupSprite == null) // No effect given, return.
            return;

        spawnedSprite = Instantiate(pickupSprite.gameObject, FindObjectOfType<PlayerShipController>().gameObject.transform);
        Effect spriteEffect = spawnedSprite.GetComponent<Effect>();
    }
}
