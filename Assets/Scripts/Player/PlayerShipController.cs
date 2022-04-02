using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerShipController : MonoBehaviour
{
    private float thrust = 1f;
    private float turnSpeed = 20f;
    public PlayerInput input;
    public Rigidbody2D rb2d;

    private void Update()
    {
        if (!input.movementInput.Equals(Vector2.zero))
            Move(input.movementInput);

        // BRANDON: Tractor beam can be called here!
        //if (input.tractorActive)
            //EnableTractor();
    }

    private void Move(Vector2 movement)
    {
        // Turning right/left
        if(movement.x != 0f)
        {
            transform.Rotate(new Vector3(0f, 0f, -movement.x) * turnSpeed * Time.fixedDeltaTime);
        }

        // Thrust up
        if (movement.y > 0f)
        {
            rb2d.AddForce(transform.right * thrust, ForceMode2D.Impulse);
        }
        if (movement.y < 0f) // Thrust down
        {
            rb2d.AddForce(-transform.right * thrust, ForceMode2D.Impulse);
        }
    }
}
