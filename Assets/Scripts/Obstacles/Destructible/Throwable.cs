using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;

public class Throwable : Destructible
{

    public void Start()
    {
        grabbed = false;
        if(!isFractured)
        {
            CommitFracture = Release;
            d2D.OnSplitStart += CommitFracture;
        }
    }
    public override void OnDestroy()
    {
        d2D.OnSplitStart -= CommitFracture;
    }

    void Release()
    {
        isFractured = true;
        if (!isThrown)
        {
            isThrown = true;
            hook.UnhookFromFracture();
        }
        SetupFadeDestruction(gameObject);
    }
    
    public override void ApplyPhysics()
    {
        Vector2 dir;
        dir = -hook.transform.right;
        
        rb2d.AddForce(dir * player.rb2d.angularVelocity);

        isThrown = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isThrown)
        {
            Vector2 dir;
            dir = -hook.transform.right;

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(dir * player.rb2d.angularVelocity);
            rb.AddForce(player.rb2d.velocity);
        }
    }

    private void Update()
    {
        if (!isThrown && !isFractured)
            transform.position = hook.transform.position;
    }
}
