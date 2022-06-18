using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;

public class Grabbable : Destructible
{
    public bool isSalvagable;
    public bool isGrabbed;
    public GameObject objectSprite;

    public void Start()
    {
        if (isFractured)
            return;

        SetupDestructibleSprite();
        CommitFracture = Fracture;
        d2D.OnSplitStart += CommitFracture;
    }
    public override void OnDestroy()
    {
        if(d2D)
            d2D.OnSplitStart -= CommitFracture;
    }
    public void Fracture()
    {
        isFractured = true;
        if (objectSprite)
            SetupFadeDestruction(objectSprite);
        else
            SetupFadeDestruction(gameObject);
    }
    public override void ApplyPhysics()
    {

    }
}
