using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public float burnDamage;

    void OnCollisionStay2D(Collision2D collision)
    {
        SpaceshipStats s;
        if (collision.gameObject.TryGetComponent<SpaceshipStats>(out s))
        {
            s.hp -= burnDamage * Time.fixedDeltaTime;
        }
    }
}
