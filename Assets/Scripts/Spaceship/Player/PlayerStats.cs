using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Base Stats
    public float hp;
    public float maxhp;

    public PlayerShipClass baseClass;

    // Booties
    [SerializeField]
    public Dictionary<Droppable, int> scrap = new Dictionary<Droppable, int>();

    // Looties
    public Dictionary<Droppable, int> loot = new Dictionary<Droppable, int>();

    public void AddScrap(Droppable drop)
    {
        if (scrap.ContainsKey(drop))
            scrap[drop]++;
        else
            scrap.Add(drop, 1);
    }

    public void AddLoot(Droppable drop)
    {
        if (loot.ContainsKey(drop))
            loot[drop]++;
        else
            loot.Add(drop, 1);

        UpdateEffects();
    }

    public void UpdateEffects()
    {
        foreach (KeyValuePair<Droppable, int> l in loot)
        {
            Droppable d = l.Key;

            // Forgive me coding gods for I am about to sin
            switch (d.name)
            {
                case "Hyperfuel":
                    print("Pain");
                    break;
                default:
                    print("Hush");
                    break;
            }
        }
    }
}
