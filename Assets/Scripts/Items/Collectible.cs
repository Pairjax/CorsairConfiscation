using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Droppable drop;

    public GameObject player;
    public PlayerStats pStats;

    private SpriteRenderer icon;

    public bool isCollectable = false;

    void Start()
    {
        icon = GetComponent<SpriteRenderer>();
        icon.sprite = drop.sprite;
        player = GameObject.FindGameObjectWithTag("Player");
        pStats = player.GetComponent<PlayerStats>();

        StartCoroutine(TimerRoutine());
    }

    void Update()
    {
        if (!isCollectable) return;

        if (drop.type == DropType.Scrap)
        {
            Vector2 change = Vector2.Lerp(player.transform.position, transform.position, 0.9f);
            transform.position = change;
        }

        if (Vector2.Distance(player.transform.position, transform.position) < 0.8f)
        {
            if (drop.type == DropType.Scrap)
                pStats.AddScrap(drop);
            else if (drop.type == DropType.Loot)
                pStats.AddLoot(drop);
            Destroy(gameObject);
        }
    }

    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(2);
        isCollectable = true;
    }
}
