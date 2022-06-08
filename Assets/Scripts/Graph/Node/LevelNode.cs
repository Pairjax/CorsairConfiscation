using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;

public class LevelNode : Node
{
    public Level level;
    public Vector2 position;
    public IPoint point;

    public bool isNeighbor(LevelNode node)
    {
        return this.Neighbors.Contains(node);
    }

}
