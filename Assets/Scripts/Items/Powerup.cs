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
            shipStats = FindObjectOfType<SpaceshipStats>();

    }
    public override void PickUp()
    {
        StartCoroutine(ActivatePowerup());
    }

    private IEnumerator ActivatePowerup()
    {
        if(gameObject.name.Equals("Shield"))
            spawnedSprite = Instantiate(pickupSprite.gameObject, FindObjectOfType<PlayerShipController>().gameObject.transform);
        ApplyPowerup();
        yield return new WaitForSeconds(duration);
        DiscardPowerup();

    }

    public virtual void ApplyPowerup()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    public virtual void DiscardPowerup()
    {
        Destroy(spawnedSprite);
        Destroy(gameObject);
    }
}
