using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node : ScriptableObject
{
    [SerializeField]
    private List<Node> neighbors;
    public List<Node> Neighbors
    {
        get
        {
            if (neighbors == null)
            {
                neighbors = new List<Node>();
            }

            return neighbors;
        }
    }

    public static T Create<T>(string name)
    where T : Node
    {
        T node = CreateInstance<T>();
        node.name = name;
        return node;
    }
}
