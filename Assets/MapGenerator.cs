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
    [SerializeField] private ScenesData scenes;
    List<QuikGraph.TaggedEdge<DelaunatorSharp.IPoint, Level>> mst;
    List<QuikGraph.TaggedEdge<DelaunatorSharp.IPoint, Level>> addedEdges = new List<QuikGraph.TaggedEdge<DelaunatorSharp.IPoint, Level>>();
    private Transform PointsContainer;
    private Transform HullContainer;
    private Transform VoronoiContainer;
    private Transform TrianglesContainer;
    private Transform MSTTrianglesContainer;
    private Transform MSTPointsContainer;

    [SerializeField] float voronoiEdgeWidth = .01f;
    [SerializeField] float triangleEdgeWidth = .01f;
    [SerializeField] float hullEdgeWith = .01f;

    [SerializeField] Color triangleEdgeColor = Color.black;
    [SerializeField] Color hullColor = Color.magenta;
    [SerializeField] Color voronoiColor = Color.white;

    [SerializeField] Material meshMaterial;
    [SerializeField] Material lineMaterial;


    [SerializeField] float generationSize = 3;
    [SerializeField] float generationMinDistance = .2f;

    [SerializeField] bool drawTrianglePoints = true;
    [SerializeField] bool drawTriangleEdges = true;
    [SerializeField] bool drawVoronoiPoints = true;
    [SerializeField] bool drawVoronoiEdges = true;
    [SerializeField] bool drawHull = true;
    [SerializeField] bool createMesh = true;

    private void Start()
    {
        var sampler = DelaunatorSharp.Unity.UniformPoissonDiskSampler.SampleCircle(Vector2.zero, generationSize, generationMinDistance);
        points = sampler.Select(point => new Vector2(point.x, point.y)).ToPoints().ToList();

        Debug.Log($"Generated Points Count {points.Count}");
        Create();
    }
    private void Create()
    {
        if (points.Count < 3) return;

        Clear();
        delaunator = new Delaunator(points.ToArray());
        ConvertToMST();

        CreateMesh();
        CreateTriangle();
        CreateHull();
        CreateVoronoi();

    }
    UndirectedGraph<IPoint, TaggedEdge<IPoint, Level>> graph;
    int count = 1;
    private void ConvertToMST()
    {
        graph = new UndirectedGraph<IPoint, TaggedEdge<IPoint, Level>>();

        delaunator.ForEachTriangleEdge(edge =>
        {
            ConvertEdge(edge);
        });
        mst = graph.MinimumSpanningTreePrim(e => e.Tag.GenerateRandomWeight()).ToList();

        Debug.Log(mst.Count);
        DrawMST();
        StartCoroutine(Countdown());
        AddEdges();
        Debug.Log(mst.Count);
        DrawMST();
    }

    private IEnumerator Countdown()
    {
        float duration = 3f;
        yield return new WaitForSecondsRealtime(duration);
    }

    private void DrawMST()
    {
        for (int i = 0; i < mst.Count; i++)
        {
            var edge = mst[i];
            CreateLine(MSTTrianglesContainer, $"TriangleEdge - {i}", new Vector3[] { edge.Source.ToVector3(), edge.Target.ToVector3() }, triangleEdgeColor, triangleEdgeWidth, 0);
        }


        for (int i = 0; i < graph.Vertices.Count(); i++)
        {
            var pointGameObject = Instantiate(trianglePointPrefab, MSTPointsContainer);
            pointGameObject.transform.SetPositionAndRotation(graph.Vertices.ElementAt(i).ToVector3(), Quaternion.identity);
        }
    }
    int edgeCount = 0;

    private void AddEdges()
    {
        for (int j = 0; j < graph.EdgeCount; j++)
        {
            foreach (TaggedEdge<IPoint, Level> edge in mst)
            {
                if (graph.Edges.ElementAt(j) != edge)
                {
                    addedEdges.Add(graph.Edges.ElementAt(j));
                    edgeCount++;
                }
                if (edgeCount >= graph.EdgeCount % 3)
                {
                    Debug.Log("added final");
                    addedEdges.ToString();
                    mst = mst.Concat(addedEdges).ToList();
                    return;
                }
            }
        }
    }

    private void ConvertEdge(IEdge edge)
    {
        var newEdge = new TaggedEdge<IPoint, Level>(edge.P, edge.Q, scenes.PickRandomLevel());

        graph.AddVerticesAndEdge(newEdge);
        count++;
    }

    private void CreateTriangle()
    {
        if (delaunator == null) return;

        delaunator.ForEachTriangleEdge(edge =>
        {
            if (drawTriangleEdges)
            {
                CreateLine(TrianglesContainer, $"TriangleEdge - {edge.Index}", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, triangleEdgeColor, triangleEdgeWidth, 0);
            }

            if (drawTrianglePoints)
            {
                var pointGameObject = Instantiate(trianglePointPrefab, PointsContainer);
                pointGameObject.transform.SetPositionAndRotation(edge.P.ToVector3(), Quaternion.identity);
            }
        });
    }

    private void CreateHull()
    {
        if (!drawHull) return;
        if (delaunator == null) return;

        CreateNewHullContainer();

        foreach (var edge in delaunator.GetHullEdges())
        {
            CreateLine(HullContainer, $"Hull Edge", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, hullColor, hullEdgeWith, 3);
        }
    }

    private void CreateVoronoi()
    {
        if (delaunator == null) return;

        delaunator.ForEachVoronoiEdge(edge =>
        {
            if (drawVoronoiEdges)
            {
                CreateLine(VoronoiContainer, $"Voronoi Edge", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, voronoiColor, voronoiEdgeWidth, 2);
            }
            if (drawVoronoiPoints)
            {
                var pointGameObject = Instantiate(voronoiPointPrefab, PointsContainer);
                pointGameObject.transform.SetPositionAndRotation(edge.P.ToVector3(), Quaternion.identity);
            }
        });
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

    private void CreateMesh()
    {
        if (!createMesh) return;

        if (meshObject != null)
        {
            Destroy(meshObject);
        }

        var mesh = new Mesh
        {
            vertices = delaunator.Points.ToVectors3(),
            triangles = delaunator.Triangles
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshObject = new GameObject("DelaunatorMesh");
        var meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = meshMaterial ?? new Material(Shader.Find("Standard"));
        var meshFilter = meshObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private void CreateNewContainers()
    {
        CreateNewPointsContainer();
        CreateNewTrianglesContainer();
        CreateNewVoronoiContainer();
        CreateNewHullContainer();
        CreateNewMSTContainers();
    }

    private void CreateNewPointsContainer()
    {
        if (PointsContainer != null)
        {
            Destroy(PointsContainer.gameObject);
        }

        PointsContainer = new GameObject(nameof(PointsContainer)).transform;
    }

    private void CreateNewTrianglesContainer()
    {
        if (TrianglesContainer != null)
        {
            Destroy(TrianglesContainer.gameObject);
        }

        TrianglesContainer = new GameObject(nameof(TrianglesContainer)).transform;
    }

    private void CreateNewHullContainer()
    {
        if (HullContainer != null)
        {
            Destroy(HullContainer.gameObject);
        }

        HullContainer = new GameObject(nameof(HullContainer)).transform;
    }

    private void CreateNewVoronoiContainer()
    {
        if (VoronoiContainer != null)
        {
            Destroy(VoronoiContainer.gameObject);
        }

        VoronoiContainer = new GameObject(nameof(VoronoiContainer)).transform;
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
    }

    private void CreateNewMSTTrianglesContainer()
    {
        if (MSTTrianglesContainer != null)
        {
            Destroy(MSTTrianglesContainer.gameObject);
        }

        MSTTrianglesContainer = new GameObject(nameof(MSTTrianglesContainer)).transform;
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
