using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;

public class Grabbable : Destructible
{
    public bool isSalvagable;
    public GameObject objectSprite;

    public void Start()
    {
        SetupDestructibleSprite();
        CommitFracture = Fracture;
        d2D.OnSplitStart += CommitFracture;
    }
    
    public void Fracture()
    {
        isFractured = true;
        SetupFadeDestruction(objectSprite);
    }
    public override void ApplyPhysics()
    {

    }
}
