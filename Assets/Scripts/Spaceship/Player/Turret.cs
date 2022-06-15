using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public float rotationCorrection;
    public float rotationSpeed;
    [SerializeField] private float fireCooldown;
    [SerializeField] private GameObject bulletPrefab;
    private GameObject target;
    private float lastFire = 0;
    private bool isFiring;

    void OnEnable()
    {
        InvokeRepeating("OnFire", 1f, fireCooldown);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsSpaceship(collision)) return;

        target = collision.gameObject;

        isFiring = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsSpaceship(collision)) return;

        isFiring = true;

        Vector3 direction = transform.position - collision.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Abs(180 - angle);
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, 
            Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsSpaceship(collision)) return;

        isFiring = false;
    }

    private bool IsSpaceship(Collider2D c)
    {
        if (c.GetComponent<CopShipController>() != null)
            return true;

        Spaceship chosenShip = c.GetComponent<Spaceship>();
        if (!chosenShip || chosenShip.category.Equals(Spaceship.SpaceshipCategory.Player))
            return false;

        return true;
    }

    private void OnFire()
    {
        if (!isFiring)
            return;

        Instantiate(bulletPrefab.gameObject, transform.position, transform.rotation);
        
    }

    private void LookAt(Vector3 target, Transform obj, float offset)
    {
        Vector3 dir = target - obj.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.localRotation = Quaternion.Euler(0f, 0f, angle + offset);
    }
}
