using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Droppable drop;
    
    private SpriteRenderer icon;

    void Start()
    {
        icon = GetComponent<SpriteRenderer>();
        icon.sprite = drop.sprite;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerStats stats;
        bool isPlayer = collision.collider.TryGetComponent<PlayerStats>(out stats);
        if (isPlayer)
           stats.AddScrap(drop);
    }
}
