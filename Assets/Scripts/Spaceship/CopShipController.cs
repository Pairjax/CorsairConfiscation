using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CopShipController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float speed;

    [Header("AI Targets")]
    public Transform followObject;
    public Transform targetedObject;

    public bool begunChase;

    [Header("Ship AI")]
    [SerializeField] private CircleCollider2D followCollider;
    [SerializeField] private CircleCollider2D targetCollider;

    public enum AIState { Engaged, Wandering, Sleep, Idle };
    public AIState currentAIState;

    public enum EnemyType { Cruiser, Brute, Seeker };
    public EnemyType enemyType;

    // Wandering
    Vector2 wanderPoint;
    Vector2 targetPoint;
    // Pathfinding
    public Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 1f;

    // Seeking
    private Seeker seeker;
    Rigidbody2D rb2d;

    float fireCooldown = .8f;
    bool isFiring;

    [Header("Cannon")]
    public GameObject bulletPrefab;
    public Transform turret;
    public Transform bulletSpawn;
    public int turretOffset = 20; // A Random range from [-x, x) angle offset the turret can make when firing
    public List<GameObject> spawnedBullets = new List<GameObject>();
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        if (currentAIState.Equals(AIState.Wandering) && !followObject && !targetedObject)
            BeginWandering();
    }
    private void Update()
    {
        if (enemyType.Equals(EnemyType.Cruiser))
        {
            if (currentAIState.Equals(AIState.Engaged))
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
            else if (!currentAIState.Equals(AIState.Wandering) && !followObject && !targetedObject)
            {
                BeginWandering();
            }
        }
        else if (enemyType.Equals(EnemyType.Brute))
        {
            if (currentAIState.Equals(AIState.Engaged))
            {
                if (ReachedPoint(targetPoint))
                    EndChase();
            }
        }
        else if (enemyType.Equals(EnemyType.Seeker))
        {
            if (currentAIState.Equals(AIState.Engaged))
            {
                if (followObject)
                {
                    LookAt(followObject, this.transform, 0f);
                }
            }
            else if (!currentAIState.Equals(AIState.Wandering) && !followObject && !targetedObject)
            {
                BeginWandering();
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

    private bool ReachedPoint(Vector2 targetPos)
    {
        float distance = Vector2.Distance(rb2d.position, targetPos);
        return (distance < .8f);
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

    public void FlyTowards(Transform transform)
    {
        if (begunChase)
            return;

        Debug.Log("Flying towards!");
        followObject = transform;
        targetedObject = transform;
        targetPoint = followObject.position;

        LookAt(targetPoint, this.transform, 0f);
        
        followObject = null;

        if(!currentAIState.Equals(AIState.Engaged))
            SetAIState(AIState.Engaged);

        UpdatePathByPoint(targetPoint);

        begunChase = true;
    }

    public void EndFollow()
    {
        followObject = null;
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

    public void BeginFollow(Transform transform, float time)
    {
        followObject = transform;
        SetAIState(AIState.Engaged);
        if (time != 0f)
            InvokeRepeating("UpdatePath", 0f, time);
        else
            UpdatePath();
    }

    private void UpdatePath()
    {
        if (seeker.IsDone() && followObject != null)
            seeker.StartPath(rb2d.position, followObject.position, OnPathComplete);
    }

    private void UpdatePathByPoint(Vector3 position)
    {
        seeker.StartPath(rb2d.position, position, OnPathComplete);
    }

    public void EndChase()
    {
        followObject = null;
        targetedObject = null;
        begunChase = false;
    }

    public void Abort()
    {
        followObject = null;
        targetedObject = null;
        begunChase = false;
        BeginWandering();
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
        if (Time.time - lastFire > fireCooldown && spawnedBullets.Count < 4)
        {
            isFiring = true;
            LookAt(targetedObject, turret, -45f - transform.rotation.eulerAngles.z + GetRandomOffset());

            GameObject spawnedBullet = Instantiate(bulletPrefab.gameObject, bulletSpawn.position, bulletSpawn.rotation);
            HomingMissile missile = spawnedBullet.GetComponent<HomingMissile>();
            if (missile)
                missile.SetParent(this);

            if (enemyType.Equals(CopShipController.EnemyType.Seeker))
                spawnedBullets.Add(spawnedBullet);
            
            RestartCooldown();
        }

        void RestartCooldown()
        {
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
