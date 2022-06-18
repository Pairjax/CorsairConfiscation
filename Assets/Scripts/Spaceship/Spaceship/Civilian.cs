using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Spaceship
{
    public override void SetupStats()
    {
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        PickRandomSprite();
    }
    private void PickRandomSprite()
    {
        if (spaceshipStats.spriteSelection == null || spaceshipStats.spriteSelection.Length == 0)
            return;

        int num = Random.Range(0, spaceshipStats.spriteSelection.Length);
        rend.sprite = spaceshipStats.spriteSelection[num];
        animator.SetInteger("sprite", num + 1);
    }

    public override void Damage(float amount)
    {
    }
}
