using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    enum GameState
    {
        FLYING,
        SHOP,
        PAUSED
    }

    // TODO: Pause menu
    private GameState currentState = GameState.FLYING;

    public Vector2 movementInput { get; private set; }

    public bool interacted { get; private set; }
    public bool fire { get; private set; }
    public bool detach { get; private set; }

    public bool paused { get; private set; }

    void Update()
    {
        if (currentState == GameState.FLYING) handleGameInput();
        else handleMenuInput();
    }

    private void handleGameInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        interacted = Input.GetKeyDown(KeyCode.E);
        fire = Input.GetKeyDown(KeyCode.Mouse0);
        detach = Input.GetKeyDown(KeyCode.Mouse1);
    }

    private void handleMenuInput()
    {
        // TODO: Whatever goes in here
    }
}
