using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using DelaunatorSharp;

public class Graph : ScriptableObject
{
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    [SerializeField]
    private List<LevelNode> nodes;
    private List<LevelNode> Nodes
    {
        get
        {
            if (nodes == null)
            {
                nodes = new List<LevelNode>();
            }

            return nodes;
        }
    }

    public LevelNode currentNode;
    public LevelNode selectedNode;
    public static Graph Create(string name)
    {
        Graph graph = CreateInstance<Graph>();

        string path = string.Format("Assets/ScriptableObjects/Map/{0}.asset", name);
        AssetDatabase.CreateAsset(graph, path);

        return graph;
    }

    public void AddNode(LevelNode node)
    {
        Nodes.Add(node);
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
    }

    public LevelNode SearchForNode(IPoint point)
    {
        return nodes.Find((x) => x.point == point);
    }

    public LevelNode SearchForNode(Vector2 point)
    {
        return nodes.Find((x) => x.position == point);
    }

    public void SetCurrentNode(Vector2 pos)
    {
        var node = SearchForNode(pos);
        currentNode = node;
    }
}
