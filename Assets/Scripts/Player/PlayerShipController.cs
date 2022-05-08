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

    [Header("Spaceship Abilities")]
    [SerializeField] private Harpoon _harpoon;

    private float _horizontalDirection;
    public Animator animator;
    public ParticleSystem pSystem;

    public PlayerStats stats;
    public PlayerInput input;
    public Rigidbody2D rb2d;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (input.fire)
        {
            _harpoon.gameObject.SetActive(true);
        }

        if(input.detach)
        {
            _harpoon.hook.UnhookObj();
        }

        if (!input.movementInput.Equals(Vector2.zero))
        {
            Move(input.movementInput);
        }

        if (input.movementInput.y == 0)
        {
            animator.SetInteger("moveValue", 0);
            pSystem.Stop();
        }

        ApplyLinearDrag();
    }

    private void Move(Vector2 movement)
    {
        // Turning right/left
        if(movement.x != 0f)
        {
            transform.Rotate(new Vector3(0f, 0f, -movement.x) * _rotateSpeed * Time.fixedDeltaTime);
        }

        // Thrust up
        if (movement.y > 0f)
        {
            rb2d.AddForce(transform.right * _movementAcceleration * stats.speedUp);
            animator.SetInteger("moveValue", 1);
            pSystem.Play();
        }

        // Thrust down
        if (movement.y < 0f)
        {
            rb2d.AddForce(-transform.right * _movementAcceleration * stats.speedUp);
            animator.SetInteger("moveValue", -1);
            pSystem.Stop();
        }
      
        // Max speed clamp.
        if (Mathf.Abs(rb2d.velocity.x) > _maxMoveSpeed * stats.speedUp)
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
