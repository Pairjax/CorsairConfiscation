using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using QuikGraph;
using QuikGraph.Algorithms;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject trianglePointPrefab;
    [SerializeField] GameObject voronoiPointPrefab;

    private List<IPoint> points = new List<IPoint>();
    private GameObject meshObject;
    private Delaunator delaunator;
    public ScenesData scenes;
    List<QuikGraph.TaggedEdge<DelaunatorSharp.IPoint, Level>> mst;
    List<QuikGraph.TaggedEdge<DelaunatorSharp.IPoint, Level>> addedEdges = new List<QuikGraph.TaggedEdge<DelaunatorSharp.IPoint, Level>>();
    public List<GameObject> levelNodePoints = new List<GameObject>();
    public Graph objGraph;
    public bool mapGenerated;
    
    private Transform MSTTrianglesContainer;
    private Transform MSTPointsContainer;

    [SerializeField] float triangleEdgeWidth = .01f;

    [SerializeField] Color triangleEdgeColor = Color.black;

    [SerializeField] Material lineMaterial;

    [SerializeField] float generationSize = 3;
    [SerializeField] float generationMinDistance = .2f;

    private void Start()
    {
        gameObject.name = "SystemMap";
        if (mapGenerated)
            return;

        if (MapManager.instance.FindMap() != null)
        {
            MapManager.instance.LoadMapIntoScene();
            Destroy(gameObject);
        }
        else
        {
            var sampler = DelaunatorSharp.Unity.UniformPoissonDiskSampler.SampleCircle(Vector2.zero, generationSize, generationMinDistance);
            points = sampler.Select(point => new Vector2(point.x, point.y)).ToPoints().ToList();

            Debug.Log($"Generated Points Count {points.Count}");
            Create();
            SetupNavigation();
            mapGenerated = true;
        }
    }
    private void Create()
    {
        if (points.Count < 3) return;

        Clear();
        delaunator = new Delaunator(points.ToArray());
        ConvertToMST();
        AddEdges();
        CreateGraphScriptableObj();
    }
    #region GRAPH CREATION

    UndirectedGraph<IPoint, TaggedEdge<IPoint, Level>> graph;
    private void ConvertToMST()
    {
        graph = new UndirectedGraph<IPoint, TaggedEdge<IPoint, Level>>();

        delaunator.ForEachTriangleEdge(edge =>
        {
            ConvertEdge(edge);
        });
        mst = graph.MinimumSpanningTreePrim(e => e.Tag.GenerateRandomWeight()).ToList();
    }

    int edgeCount = 0;
    private void AddEdges()
    {
        edgeCount = 0;
        Debug.Log(mst.Count);
        do
        {
            int j = UnityEngine.Random.Range(0, graph.EdgeCount);
            TaggedEdge<IPoint, Level> edge = graph.Edges.ElementAt(j);
            edge = CheckForEdge(edge);

            if (edge == null)
                continue;

            addedEdges.Add(edge);
            mst = mst.Concat(addedEdges).ToList();
            addedEdges.Clear();
            edgeCount++;

        } while (edgeCount <= graph.EdgeCount * .15f);
        Debug.Log("Final count: " + mst.Count);
    }

    private TaggedEdge<IPoint, Level> CheckForEdge(TaggedEdge<IPoint, Level> edge)
    {
        foreach(TaggedEdge<IPoint, Level> mstEdge in mst)
        {
            if ((mstEdge.Source == edge.Source || mstEdge.Source == edge.Target)
            && (mstEdge.Target == edge.Source || mstEdge.Target == edge.Target))
                return null;
        }
        return edge;
    }

    private void ConvertEdge(IEdge edge)
    {
        var newEdge = new TaggedEdge<IPoint, Level>(edge.P, edge.Q, scenes.PickRandomLevel());

        graph.AddVerticesAndEdge(newEdge);
    }

    public List<IPoint> pickedPoints = new List<IPoint>();
    private void CreateGraphScriptableObj()
    {
        int i = -1;
        int j = -1;
        objGraph = Graph.Create("System Map");
        pickedPoints.Clear();
        foreach (TaggedEdge<IPoint, Level> edge in mst)
        {
            LevelNode nodeP;
            LevelNode nodeQ;
            GameObject pointGameObject;
            GameObject pointGameObject2;

            i++;
            j++;
            if (pickedPoints.Contains(edge.Source) && !pickedPoints.Contains(edge.Target))
            {
                var PointA = edge.Source;
                var PointB = edge.Target;

                var FoundNode = objGraph.SearchForNode(PointA);

                pointGameObject = Instantiate(trianglePointPrefab, MSTPointsContainer).gameObject;
                pointGameObject.transform.SetPositionAndRotation(edge.Target.ToVector3(), Quaternion.identity);
                levelNodePoints.Add(pointGameObject);

                CreateLine(MSTTrianglesContainer, $"TriangleEdge - {j}", new Vector3[] { edge.Source.ToVector3(), edge.Target.ToVector3() }, triangleEdgeColor, triangleEdgeWidth, 0);

                nodeQ = Node.Create<LevelNode>("Node" + i);
                nodeQ.level = scenes.PickRandomLevel();
                nodeQ.position = edge.Target.ToVector2();
                nodeQ.point = PointB;
                nodeQ.Neighbors.Add(FoundNode);
                FoundNode.Neighbors.Add(nodeQ);
                objGraph.AddNode(nodeQ);

                pickedPoints.Add(edge.Target);

                continue;
            }
            else if (!pickedPoints.Contains(edge.Source) && pickedPoints.Contains(edge.Target))
            {
                var PointA = edge.Source;
                var PointB = edge.Target;

                var FoundNode = objGraph.SearchForNode(PointB);

                pointGameObject = Instantiate(trianglePointPrefab, MSTPointsContainer).gameObject;
                pointGameObject.transform.SetPositionAndRotation(edge.Source.ToVector3(), Quaternion.identity);
                levelNodePoints.Add(pointGameObject);

                CreateLine(MSTTrianglesContainer, $"TriangleEdge - {j}", new Vector3[] { edge.Source.ToVector3(), edge.Target.ToVector3() }, triangleEdgeColor, triangleEdgeWidth, 0);

                nodeP = Node.Create<LevelNode>("Node" + i);
                nodeP.level = scenes.PickRandomLevel();
                nodeP.position = edge.Source.ToVector2();
                nodeP.point = PointA;
                nodeP.Neighbors.Add(FoundNode);
                FoundNode.Neighbors.Add(nodeP);
                objGraph.AddNode(nodeP);

                pickedPoints.Add(edge.Source);

                continue;
            }
            else if(pickedPoints.Contains(edge.Source) && pickedPoints.Contains(edge.Target))
            {
                var PointA = edge.Source;
                var PointB = edge.Target;

                var FoundNode1 = objGraph.SearchForNode(PointA);
                var FoundNode2 = objGraph.SearchForNode(PointB);

                FoundNode1.Neighbors.Add(FoundNode2);
                FoundNode2.Neighbors.Add(FoundNode1);

                CreateLine(MSTTrianglesContainer, $"TriangleEdge - {j}", new Vector3[] { edge.Source.ToVector3(), edge.Target.ToVector3() }, triangleEdgeColor, triangleEdgeWidth, 0);

                continue;
            }


            pickedPoints.Add(edge.Source);
            pickedPoints.Add(edge.Target);

            nodeP = Node.Create<LevelNode>("Node" + i);
            i++;
            nodeQ = Node.Create<LevelNode>("Node" + i);
            nodeP.Neighbors.Add(nodeQ);
            nodeQ.Neighbors.Add(nodeP);

            pointGameObject = Instantiate(trianglePointPrefab, MSTPointsContainer).gameObject;
            pointGameObject.transform.SetPositionAndRotation(edge.Source.ToVector3(), Quaternion.identity);
            levelNodePoints.Add(pointGameObject);

            pointGameObject2 = Instantiate(trianglePointPrefab, MSTPointsContainer).gameObject;
            pointGameObject2.transform.SetPositionAndRotation(edge.Target.ToVector3(), Quaternion.identity);
            levelNodePoints.Add(pointGameObject2);

            CreateLine(MSTTrianglesContainer, $"TriangleEdge - {j}", new Vector3[] { edge.Source.ToVector3(), edge.Target.ToVector3() }, triangleEdgeColor, triangleEdgeWidth, 0);

            nodeP.level = scenes.PickRandomLevel();
            nodeP.position = edge.ToVertexPair().Source.ToVector2();
            nodeP.point = edge.ToVertexPair().Source;

            nodeQ.level = scenes.PickRandomLevel();
            nodeQ.position = edge.ToVertexPair().Target.ToVector2();
            nodeQ.point = edge.ToVertexPair().Target;

            objGraph.AddNode(nodeP);
            objGraph.AddNode(nodeQ);
        }
    }
    #endregion GRAPH CREATION
    #region NAVIGATION SETUP
    private void SetupNavigation()
    {
        PickStartingNode();
    }
    GameObject startingLevel;
    private void PickStartingNode()
    {
        startingLevel = levelNodePoints[UnityEngine.Random.Range(0, 4)];
        startingLevel.GetComponent<SpriteRenderer>().color = Color.green;
        objGraph.SetCurrentNode(startingLevel.transform.position);
    }

    public void UpdateCurrentNode()
    {
        GetCurrentPoint().GetComponent<SpriteRenderer>().color = Color.green;
    }

    public GameObject GetCurrentPoint()
    {
        return levelNodePoints.Find((x) => (Vector2) x.transform.position == objGraph.currentNode.position);
    }
    #endregion NAVIGATION SETUP
    public GameObject SearchForRelativePoint(LevelNode node)
    {
        return levelNodePoints.Find((x) => (Vector2)x.transform.position == node.position);
    }
    private void CreateLine(Transform container, string name, Vector3[] points, Color color, float width, int order = 1)
    {
        var lineGameObject = new GameObject(name);
        lineGameObject.transform.parent = container;
        var lineRenderer = lineGameObject.AddComponent<LineRenderer>();

        lineRenderer.SetPositions(points);

        lineRenderer.material = lineMaterial ?? new Material(Shader.Find("Standard"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.sortingOrder = order;
    }

    private void CreateNewContainers()
    {
        CreateNewMSTContainers();
    }

    private void CreateNewMSTContainers()
    {
        CreateNewMSTPointsContainer();
        CreateNewMSTTrianglesContainer();
    }
    private void CreateNewMSTPointsContainer()
    {
        if (MSTPointsContainer != null)
        {
            Destroy(MSTPointsContainer.gameObject);
        }

        MSTPointsContainer = new GameObject(nameof(MSTPointsContainer)).transform;
        MSTPointsContainer.transform.SetParent(this.transform);
    }

    private void CreateNewMSTTrianglesContainer()
    {
        if (MSTTrianglesContainer != null)
        {
            Destroy(MSTTrianglesContainer.gameObject);
        }

        MSTTrianglesContainer = new GameObject(nameof(MSTTrianglesContainer)).transform;
        MSTTrianglesContainer.transform.SetParent(this.transform);
    }

    private void Clear()
    {
        CreateNewContainers();

        if (meshObject != null)
        {
            Destroy(meshObject);
        }

        delaunator = null;
    }

    
}
