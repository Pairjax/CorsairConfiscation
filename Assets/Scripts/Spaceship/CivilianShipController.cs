using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CivilianShipController : MonoBehaviour
{
    public enum ShipState { Moving, Idle, Disabled };
    public ShipState shipState = ShipState.Idle;

    // Pathfinding
    public Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 4f;
    bool reachedEndOfPath = false;

    // Seeking
    private Seeker seeker;
    Rigidbody2D rb2d;

    private Civilian civ;

    private float speed;
    public Vector2 destination;
    
    public void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        civ = GetComponent<Civilian>();
        speed = civ.spaceshipStats.speed;
        rb2d.drag = civ.spaceshipStats.linDrag;
    }
    private void Update()
    {
        if (reachedEndOfPath)
            Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (path == null || !shipState.Equals(ShipState.Moving))
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb2d.AddForce(force);

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            LookAt((Vector2)path.vectorPath[currentWaypoint], transform, -90f);
        }
    }

    public void SetState(ShipState state)
    {
        switch (state)
        {
            case ShipState.Disabled:
                break;
            case ShipState.Idle:
                break;
            case ShipState.Moving:
                break;
        }
        shipState = state;
    }

    public void MoveToPosition(Vector3 movePos)
    {
        destination = movePos;
        UpdateMovePath(movePos);
    }

    private void UpdateMovePath(Vector2 movePos)
    {
        if (seeker.IsDone())
        {
            SetState(ShipState.Moving);
            seeker.StartPath(rb2d.position, destination, OnPathComplete);
        }
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void LookAt(Vector3 target, Transform obj, float offset)
    {
        Vector3 dir = target - obj.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.rotation = Quaternion.Euler(0f, 0f, angle + offset);
    }
}
