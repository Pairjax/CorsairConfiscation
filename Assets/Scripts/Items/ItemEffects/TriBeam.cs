using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriBeam : ItemEffect
{
    public override void Enable()
    {
        playerStats.beamCount = 3;
    }
}
