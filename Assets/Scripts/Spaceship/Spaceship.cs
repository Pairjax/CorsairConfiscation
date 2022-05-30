using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spaceship : MonoBehaviour
{
    static BountyManager bountyManager;
    public enum SpaceshipCategory { Cop, Player, Civilian }
    public SpaceshipCategory category;

    private int bountyPoints;
    public SpaceshipStats spaceshipStats;
    internal SpriteRenderer rend;
    internal Animator animator;
    internal Rigidbody2D rb2d;

    private void Awake()
    {
        if(!bountyManager)
            bountyManager = FindObjectOfType<BountyManager>();
        SetupStats();
    }
    public abstract void SetupStats();
    public abstract void Damage();
    
    public void OnDestroy()
    {
        if (category != SpaceshipCategory.Civilian)
            DropLoot(spaceshipStats);

        bountyManager.AddBounty(spaceshipStats.bountyPoints);
    }

    private void DropLoot(SpaceshipStats stats)
    {

    }
}
