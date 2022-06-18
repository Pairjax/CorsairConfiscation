using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiPursuitNet : ComponentBehavior
{
    private bool onCooldown = false;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float throwForce;
    [SerializeField] private GameObject net;

    void Start() { name = "Anti-Pursuit Net"; }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onCooldown) return;

        GameObject thrownNet = Instantiate(net, Vector2.zero, Quaternion.identity);
        thrownNet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * throwForce);

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }
}

public class X3AutoCannon : ComponentBehavior
{
    [SerializeField] private float damage;

    void Start() { name = "X-3 Auto Cannon"; }

    // TODO: Turret Effects
}

public class EnergyShield : ComponentBehavior
{
    [SerializeField] private Sprite shieldSprite;
    [SerializeField] private float regenRate;
    public float hp;
    private float hpMax;

    [SerializeField] private float percentIncrease;
    [SerializeField] private float cooldownTimer;
    private bool onCooldown = false;

    void Start() { name = "Energy Shield"; }

    void FixedUpdate()
    {
        hp = Mathf.Max(hp, 0);

        if (onCooldown) return;

        hpMax = playerStats._maxHP / 10;
        hp += hpMax * regenRate;

        hp = Mathf.Min(hp, hpMax);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        onCooldown = false;
    }

}
