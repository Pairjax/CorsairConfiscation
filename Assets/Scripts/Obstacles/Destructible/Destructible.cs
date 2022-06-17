using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;

public abstract class Destructible : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D spriteRB;
    internal D2dDestructibleSprite d2D;
    internal D2dImpactDamage damage;
    internal D2dFracturer fract;
    internal D2dPolygonCollider col;
    internal Rigidbody2D rb2d;
    internal static Player player = null;
    

    [Header("Settings")]
    public DestructibleSettings settings;

    public bool isFractured;
    internal System.Action CommitFracture;

    public void Awake()
    {
        if (player == null)
            player = FindObjectOfType<Player>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        d2D = gameObject.GetComponent<D2dDestructibleSprite>();
    }
    private void OnDestroy()
    {
        d2D.OnSplitStart -= CommitFracture;
    }
    internal void SetupDestructibleSprite()
    {
       
        if(settings.dType.Equals(DestructibleSettings.DestructibleType.Asteroid))
        {
            GameObject spriteObj = gameObject.transform.GetChild(0).gameObject;
            spriteRB = spriteObj.AddComponent<Rigidbody2D>();
            d2D = spriteObj.AddComponent<D2dDestructibleSprite>();
            fract = spriteObj.AddComponent<D2dFracturer>();
            col = spriteObj.AddComponent<D2dPolygonCollider>();
            damage = spriteObj.AddComponent<D2dImpactDamage>();
        }

        if(settings.dType.Equals(DestructibleSettings.DestructibleType.NPCShip))
        {
            GameObject spriteObj = gameObject.transform.GetChild(0).gameObject;
            d2D = spriteObj.AddComponent<D2dDestructibleSprite>();
            fract = spriteObj.AddComponent<D2dFracturer>();
            col = spriteObj.AddComponent<D2dPolygonCollider>();
            damage = spriteObj.AddComponent<D2dImpactDamage>();
        }
        
        damage.Threshold = settings.damageThreshold;
        d2D.Pixels = settings.pixels;
        fract.Damage = settings.canDamage;
        fract.DamageRequired = settings.damageRequiredForFracture;
        fract.PointsPerSolidPixel = settings.pointsPerPixel;
        d2D.Rebuild();
    }

    internal void SetupFadeDestruction(GameObject obj)
    {
        D2dDestroyer destroyer = obj.AddComponent<D2dDestroyer>();
        destroyer.Life = Random.Range(settings.lifeMin, settings.lifeMax);
        Debug.Log(destroyer.Life);
        destroyer.FadeDuration = settings.lifeMin;
        destroyer.Fade = true;
    }

    public abstract void ApplyPhysics();
}
