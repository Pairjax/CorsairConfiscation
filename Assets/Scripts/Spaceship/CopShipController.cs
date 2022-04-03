using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopShipController : MonoBehaviour
{
    public enum ShipAIState { Wander, Pursue, Engage }
    public ShipAIState shipAIState = ShipAIState.Wander;

    public enum ShipState { Moving, Idle, Shooting, Dying, Disabled }
    public ShipState shipState = ShipState.Idle;

    private float shipSpeed = 1.3f;

    private float fireCooldown = .8f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform BulletHolder;
    // AI Vars
    public Vector3 destination = Vector3.zero;
    // Wander
    private float wanderRadius = 3f;
    private float wanderWaitTime = 1f;
    // Pursuit
    public Collider2D playerCheckCollider;
    public Spaceship playerShip;

    private void Start()
    {
        ResetAI();
    }

    private void Update()
    {

        if (shipAIState.Equals(ShipAIState.Wander))
        {
            if (!destination.Equals(Vector2.zero))
            {
                SetState(ShipState.Moving);
            }

            if (transform.position.Equals(destination) && shipState.Equals(ShipState.Moving))
            {
                SetState(ShipState.Idle);
            }
        }

        if (shipAIState.Equals(ShipAIState.Pursue))
        {
            MoveToPosition(playerShip.transform.position);
            LookAt();
            FireBullet();
        }


    }
    public void MoveToPosition(Vector3 movePos)
    {
        SetState(ShipState.Moving);
        destination = movePos;
    }
    public void ResetAI()
    {
        playerShip = null;
        destination = Vector2.zero;
        SetState(ShipState.Idle);
        SetAIState(ShipAIState.Wander);
        Invoke("Wander", wanderWaitTime);
    }
    private void Wander()
    {
        MoveToPosition(PickRandomWanderSpot(transform.position, wanderRadius));
        LookAt();
    }

    public void SetState(ShipState state)
    {
        switch (state)
        {
            case ShipState.Idle:
                if (shipAIState.Equals(ShipAIState.Wander))
                {
                    MoveToPosition(Vector2.zero);
                    SetAIState(ShipAIState.Wander);
                    Invoke("Wander", wanderWaitTime);
                }
                break;
            case ShipState.Moving:
                transform.position = Vector2.MoveTowards(transform.position, destination, shipSpeed * Time.deltaTime);
                break;
            
        }
        shipState = state;
    }
    public void SetAIState(ShipAIState state)
    {
        shipAIState = state;
    }
    private void LookAt()
    {
        Vector3 dir = (Vector3)destination - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    private Vector2 PickRandomWanderSpot(Vector2 origin, float dist)
    {
        Vector2 destination = Random.insideUnitSphere * dist;
        destination += origin;
        destination = new Vector3(destination.x, destination.y, 0f);
        return destination;
    }
    float lastFire = 2f;
    private void FireBullet()
    {
        if ((Time.time - lastFire > fireCooldown))
        {
            SetState(ShipState.Shooting);
            
            Instantiate(bulletPrefab.gameObject, bulletSpawn.position, transform.rotation, BulletHolder);
            lastFire = Time.time;
        }
    }
}
