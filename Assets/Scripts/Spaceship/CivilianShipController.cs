using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianShipController : MonoBehaviour
{
    public Sprite[] spriteOptions = new Sprite[6];

    public enum ShipState { Moving, Idle };
    public ShipState shipState = ShipState.Idle;

    private float shipSpeed = 1f;
    public Vector2 destination;
    public Animator animator;
    public SpriteRenderer rend;
    public void Awake()
    {
        PickRandomSprite();
    }
    private void PickRandomSprite()
    {
        int num = Random.Range(0, spriteOptions.Length);
        rend.sprite = spriteOptions[num];
        animator.SetInteger("sprite", num + 1);
    }

    private void Update()
    {
        if (!destination.Equals(Vector2.zero))
        {
            SetState(ShipState.Moving);
        }

        if (transform.position.Equals(destination) && shipState.Equals(ShipState.Moving))
        {
            SetState(ShipState.Idle);
        }
    }
    public void SetState(ShipState state)
    {
        switch (state)
        {
            case ShipState.Idle:
                MoveToPosition(Vector2.zero);
                break;
            case ShipState.Moving:
                transform.position = Vector2.MoveTowards(transform.position, destination, shipSpeed * Time.deltaTime);
                break;
        }
        shipState = state;
    }

    public void MoveToPosition(Vector3 movePos)
    {
        SetState(ShipState.Moving);
        destination = movePos;
        LookAt();
    }

    private void LookAt()
    {
        Vector3 dir = (Vector3)destination - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
