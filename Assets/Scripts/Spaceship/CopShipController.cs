using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CopShipController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float speed;

    // AI Targets
    private Transform followObject;
    private Transform targetedObject;

    [Header("Ship AI")]
    [SerializeField] private CircleCollider2D followCollider;
    [SerializeField] private CircleCollider2D targetCollider;

    public enum AIState { Engaged, Wandering, Sleep, Idle };
    public AIState currentAIState;

    // Wandering
    Vector2 wanderPoint;

    // Pathfinding
    public Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 1f;

    // Seeking
    private Seeker seeker;
    Rigidbody2D rb2d;

    float fireCooldown =.8f;
    bool isFiring;

    [Header("Cannon")]
    public GameObject bulletPrefab;
    public Transform turret;
    public Transform bulletSpawn;
    public int turretOffset = 20; // A Random range from [-x, x) angle offset the turret can make when firing

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        if (currentAIState.Equals(AIState.Wandering) && !followObject && !targetedObject)
            BeginWandering();
    }
    private void Update()
    {
        if(currentAIState.Equals(AIState.Engaged))
        {
            if (followObject)
            {
                LookAt(followObject, this.transform, 0f);
            }

            if (!isFiring && targetedObject)
            {
                LookAt(targetedObject, turret, -45f - transform.rotation.eulerAngles.z);
            }
        }
    }

    bool reachedEndOfPath = false;
    private void FixedUpdate()
    {
        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        
        reachedEndOfPath = false;

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb2d.AddForce(force);

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
            currentWaypoint++;

    }
    private void SetAIState(AIState aiState)
    {
        switch (aiState)
        {
            case AIState.Sleep:
                AISleep();
                currentAIState = AIState.Sleep;
                break;
            case AIState.Wandering:
                currentAIState = AIState.Wandering;
                break;
            case AIState.Engaged:
                EndWandering();
                currentAIState = AIState.Engaged;
                break;
        }
    }

    private void AISleep()
    {
        EndFiring();
        EndFollow();
    }

    private void LookAt(Transform target, Transform obj, float offset)
    {
        Vector3 dir = target.position - obj.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.localRotation = Quaternion.Euler(0f, 0f, angle + offset);
    }
    private void LookAt(Vector3 target, Transform obj, float offset)
    {
        Vector3 dir = target - obj.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.localRotation = Quaternion.Euler(0f, 0f, angle + offset);
    }

    private float GetRandomOffset()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        float angleOffset = Random.Range(0, turretOffset) - (turretOffset / 2);
        return angleOffset;
    }

    public void BeginWandering()
    {
        SetAIState(AIState.Wandering);
        InvokeRepeating("UpdateWanderPath", 0f, 3f);
    }

    private void UpdateWanderPath()
    {
        if (seeker.IsDone())
        {
            wanderPoint = GetPointInsideCircle();
            LookAt(wanderPoint, this.transform, 0f);
            seeker.StartPath(rb2d.position, wanderPoint, OnPathComplete);
        }
    }

    private void EndWandering()
    {
        CancelInvoke("UpdateWanderPath");
    }

    private Vector2 GetPointInsideCircle()
    {
        Vector2 newPoint;
        float angle = Random.Range(0.0F, 1.0F) * (Mathf.PI * 2);
        float radius = Random.Range(0.0F, 1.0F) * targetCollider.radius;
        newPoint.x = targetCollider.transform.position.x + radius * Mathf.Cos(angle);
        newPoint.y = targetCollider.transform.position.y + radius * Mathf.Sin(angle);

        return newPoint;
    }

    public void BeginFollow(Transform transform)
    {
        followObject = transform;
        SetAIState(AIState.Engaged);
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb2d.position, followObject.position, OnPathComplete);
    }

    public void EndFollow()
    {
        followObject = null;
        CancelInvoke("UpdatePath");
    }
    public void BeginFiring(Transform transform)
    {
        targetedObject = transform;
        SetAIState(AIState.Engaged);
        InvokeRepeating("Fire", 0f, fireCooldown);
    }

    float lastFire = 2f;
    private void Fire()
    {
        if (Time.time - lastFire > fireCooldown)
        {
            isFiring = true;
            LookAt(targetedObject, turret, -45f - transform.rotation.eulerAngles.z + GetRandomOffset());
            Instantiate(bulletPrefab.gameObject, bulletSpawn.position, bulletSpawn.rotation);
            lastFire = Time.time;
            isFiring = false;
        }
    }

    public void EndFiring()
    {
        isFiring = false;
        targetedObject = null;
        CancelInvoke("Fire");
        BeginWandering();
    }

    public void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    
}
