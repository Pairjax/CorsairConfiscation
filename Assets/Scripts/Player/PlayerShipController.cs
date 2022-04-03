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
    
    private float _horizontalDirection;
    public Animator animator;
    public GameObject[] tractorBeams;

    public PlayerStats stats;
    public PlayerInput input;
    public Rigidbody2D rb2d;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!input.movementInput.Equals(Vector2.zero))
        {
            Move(input.movementInput);
        }

        if (input.movementInput.y == 0)
        {
            animator.SetInteger("moveValue", 0);
        }


        // BRANDON: Tractor beam can be called here!
        if (input.tractorActive)
        {
            foreach(GameObject beam in tractorBeams)
            {
                if(beam)
                    beam.SetActive(true);
            }

        }
        else
        {
            foreach (GameObject beam in tractorBeams)
            {
                if (beam)
                {
                    beam.GetComponent<TractorBeam>().Release();
                    beam.SetActive(false);
                }
            }
            
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
            rb2d.AddForce(transform.right * _movementAcceleration);
            animator.SetInteger("moveValue", 1);
        }
        // Thrust down
        if (movement.y < 0f)
        {
            rb2d.AddForce(-transform.right * _movementAcceleration);
            animator.SetInteger("moveValue", -1);
        }
      
        // Max speed clamp.
        if (Mathf.Abs(rb2d.velocity.x) > _maxMoveSpeed * stats.speedUp)
        {
            float newX = Mathf.Sign(rb2d.velocity.x) * _maxMoveSpeed;
            rb2d.velocity = new Vector2(newX, rb2d.velocity.y);
        }
           
    }

    public void AddTractors()
    {
        tractorBeams[1] = transform.GetChild(2).gameObject;
        tractorBeams[2] = transform.GetChild(3).gameObject;
    }

    private void ApplyLinearDrag()
    {
        rb2d.drag = _linearDrag;
    }
}
