using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerShipController : MonoBehaviour
{
    private float moveSpeed = 5f;
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
        Debug.Log(movement);
        rb2d.MovePosition(rb2d.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
