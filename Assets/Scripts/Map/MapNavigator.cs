using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MapNavigator : MonoBehaviour
{
    public MapGenerator mapGenerator;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hits2d = Physics2D.GetRayIntersection(ray);
            if (hits2d.collider != null)
                CurrentClickedGameObject(hits2d.collider.gameObject);
        }
    }

    public void CurrentClickedGameObject(GameObject gameObject)
    {
        if (gameObject.tag == "MapNode")
        {
            Debug.Log("Clicked MapNode!");
            TravelToNode(gameObject);
        }
    }

    private void TravelToNode(GameObject node)
    {
        Graph systemMap = mapGenerator.objGraph;
        var clickedNode = systemMap.SearchForNode(node.transform.position);

        if (clickedNode == systemMap.currentNode)
            return;
        if (!systemMap.currentNode.isNeighbor(clickedNode))
            return;
        

        Debug.Log("Clicked nieghbor node!");
        mapGenerator.MarkNodeAsTraveled();
        systemMap.currentNode = clickedNode;
        mapGenerator.UpdateCurrentNode();
        MapManager.instance.SaveMapAsPrefab(mapGenerator.gameObject);

        if (!systemMap.traveledNodes.Contains(clickedNode))
            mapGenerator.scenes.LoadLevel(systemMap.currentNode.level.sceneName);
    }
}
