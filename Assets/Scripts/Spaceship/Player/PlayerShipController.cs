using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _linearDrag;
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private Harpoon harpoon;

    private float _horizontalDirection;
    public ParticleSystem pSystem;

    public Player player;
    public PlayerInput input;
    public Rigidbody2D rb2d;

    void Start()
    {
        player.animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (input.fire) harpoon.OnLaunchHook();
        if (input.retract) harpoon.OnRetract();
        if (input.extend) harpoon.OnExtend();
        if (input.loadMap) MapManager.instance.LoadMapScene();
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
    }

    private void Move(Vector2 movement)
    {
        // Turning right/left
        if(movement.x != 0f)
        {
            rb2d.AddTorque(-movement.x * _rotateSpeed * Time.fixedDeltaTime);
        }

        // Thrust up
        if (movement.y > 0f)
        {
            rb2d.AddForce(transform.right * _movementAcceleration * player.stats.speedUp);
        }

        // Thrust down
        if (movement.y < 0f)
        {
            rb2d.AddForce(-transform.right * _movementAcceleration * player.stats.speedUp);
        }

        // Max speed clamp.
        if (Mathf.Abs(rb2d.velocity.x) > _maxMoveSpeed * player.stats.speedUp)
        {
            float newX = Mathf.Sign(rb2d.velocity.x) * _maxMoveSpeed;
            rb2d.velocity = new Vector2(newX, rb2d.velocity.y);
        }

    }

    private void ApplyLinearDrag()
    {
        rb2d.drag = _linearDrag;
    }
}
