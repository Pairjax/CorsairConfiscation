using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Effect
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (disabled) return; // To prevent overriding coroutine, probably a better way...
        if (collision.gameObject.GetComponent<Asteroid>())
        {
            Destroy(collision.gameObject);
            disabled = true;
            StartCoroutine(DisableEffect(effectCooldown));
        }

    }
}
