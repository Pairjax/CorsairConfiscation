using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    // Components
    [SerializeField] private List<Harpoon> harpoons;
    [SerializeField] private Burner burner;

    public ParticleSystem pSystem;

    [SerializeField] private Player player;
    [SerializeField] public PlayerStats pStats;
    [SerializeField] private PlayerInput input;
    public Rigidbody2D rb2d;
    [SerializeField] private Collider2D shipCollider;
    [SerializeField] private float impactBase;

    void Start()
    {
        player.animator = transform.GetChild(0).GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        shipCollider = GetComponent<Collider2D>();
        pStats = player.stats;
    }

    private void Update()
    {
        foreach (Harpoon h in harpoons)
        {
            if (!h.isActiveAndEnabled) continue;

            if (input.fire) h.OnLaunchHook();
            if (input.retract) h.Retract();
            if (input.extend) h.Extend();
        } 

        if (input.loadMap) MapManager.instance.LoadMapScene();

        if (pStats.refreshComponents)
            ActivateComponents();

        HandleRegen();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Hook>() != null)
            Physics2D.IgnoreCollision(collision.collider, shipCollider);

        // Damage calculation
        float damage = Mathf.Abs(collision.relativeVelocity.magnitude 
            * pStats._collisionMultiplier);
        damage = Mathf.Log(damage, impactBase);
        damage = damage > 1 ? damage : 0;

        // Maw Component effect
        if (pStats.maw && pStats._hp - damage <= 0)
        {
            Salvagable s;
            if (collision.gameObject.TryGetComponent(out s))
                s.OnSalvage();

            Destroy(collision.gameObject);
            return;
        }

        pStats._hp -= damage;
    }

    private void FixedUpdate()
    {

        if (!input.movementInput.Equals(Vector2.zero))
        {
            Move(input.movementInput);
        }
        #region //animation & particles (By Jack)

        switch (input.movementInput.y)
        {
            case 0:
                // Not Forward/Backward
                if (input.movementInput.x > 0)
                {   // left
                    player.animator.SetInteger("moveValue", 1);
                }
                else if (input.movementInput.x < 0)
                {   // right
                    player.animator.SetInteger("moveValue", 2);
                }
                else
                {   //default
                    player.animator.SetInteger("moveValue", 0);
                    pSystem.Stop();
                }
            break;

            case 1:
                {
                    pSystem.Play();
                    if (input.movementInput.x > 0)
                    {
                        //forwardleft
                        player.animator.SetInteger("moveValue", 4);
                    }
                    else if (input.movementInput.x < 0)
                    {   //forwardright
                        player.animator.SetInteger("moveValue", 3);
                    }
                    else
                    {  //forward
                        player.animator.SetInteger("moveValue", 5);
                    }

                }
           break;

           case -1:
                pSystem.Stop();
                if (input.movementInput.x > 0)
                {
                    //backleft
                    player.animator.SetInteger("moveValue", 7);
                }
                else if(input.movementInput.x < 0)
                {   //backright
                    player.animator.SetInteger("moveValue", 8);
                }
                else
                {   //backwards
                    player.animator.SetInteger("moveValue", 6);
                }
                break;


        }
        #endregion

        if (input.movementInput.y > 0)
            ApplyLinearDrag();

        rb2d.mass = pStats._mass;
    }

    private void Move(Vector2 movement)
    {
        // Turning right/left
        if(movement.x != 0f)
        {
            rb2d.AddTorque(-movement.x * pStats._rotateSpeed);
        }

        // Thrust up
        if (movement.y > 0f)
        {
            rb2d.AddForce(transform.right * pStats._acceleration);
        }

        // Thrust down
        if (movement.y < 0f)
        {
            rb2d.AddForce(-transform.right * pStats._acceleration);
        }

        // Max speed clamp.
        if (Mathf.Abs(rb2d.velocity.x) > pStats._maxSpeed)
        {
            float newX = Mathf.Sign(rb2d.velocity.x) * pStats._maxSpeed;
            rb2d.velocity = new Vector2(newX, rb2d.velocity.y);
        }

    }

    private void ApplyLinearDrag()
    {
        rb2d.drag = pStats._linearDrag;
    }

    private void ActivateComponents()
    {
        ResetComponents();

        if (pStats.multiHarpoon)
            foreach (Harpoon h in harpoons)
                h.gameObject.SetActive(true);

        pStats.refreshComponents = false;

        if (pStats.burner)
            burner.gameObject.SetActive(true);
    }

    private void ResetComponents()
    {
        foreach (Harpoon h in harpoons)
            h.gameObject.SetActive(false);

        harpoons[0].gameObject.SetActive(true);

        burner.gameObject.SetActive(false);
    }

    private void HandleRegen()
    {
        pStats._hp += pStats._regen * Time.fixedDeltaTime;
        pStats._hp = Mathf.Min(pStats._hp, pStats._maxHP);
    }

}
