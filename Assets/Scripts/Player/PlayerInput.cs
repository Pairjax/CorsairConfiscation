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

    public bool tractorActive { get; private set; }
    public bool interacted { get; private set; }

    public bool paused { get; private set; }

    void Update()
    {
        if (currentState == GameState.FLYING) handleGameInput();
        else handleMenuInput();
    }

    private void handleGameInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        tractorActive = Input.GetKey("space");
        interacted = Input.GetKeyDown(KeyCode.E);
    }

    private void handleMenuInput()
    {
        // TODO: Whatever goes in here
    }
}
