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

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats;
        bool isPlayer = collision.gameObject.TryGetComponent<PlayerStats>(out stats);
        if (isPlayer)
        {
            if (drop.type == DropType.Scrap)
                stats.AddScrap(drop);
            else if (drop.type == DropType.Loot)
                stats.AddLoot(drop);
            Destroy(gameObject);
        }
    }
}
