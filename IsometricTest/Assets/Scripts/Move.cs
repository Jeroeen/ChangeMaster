using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Move : MonoBehaviour
{
    public Tilemap Tilemap;
    public GridLayout GridLayout;
    private Graph graph;
    private Path path;

    private string sourceNearestNode;
    private Vector3Int sourceTile;

    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph(Tilemap, GridLayout);
        path = new Path(graph, Tilemap);
        sourceNearestNode = "";
    }

    // Update is called once per frame
    void Update()
    {
        graph.Render();
        //path.Render();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int targetTile = GridLayout.WorldToCell(target);
            string targetNearestNode = path.FindNearestANode(targetTile);
            
            Debug.Log(transform.position);
            sourceTile = GridLayout.WorldToCell(transform.position);
            sourceNearestNode = path.FindNearestANode(sourceTile);
            //Debug.Log(sourceTile + ", "+ sourceNearestNode);

            path.FindBestPath(sourceNearestNode, targetNearestNode);
        }

        if (path.BestPath != null)
        {
            Debug.Log("Path: " + path.BestPath.Position);
            transform.position = Vector3.MoveTowards(transform.position, path.BestPath.Position, 4 * Time.deltaTime);

            if (Vector3.Magnitude(transform.position - (Vector3)path.BestPath.Position) < 0.1f && path.BestPath.AdjEdges.Count > 0 && path.BestPath.AdjEdges[0] != null && path.BestPath.AdjEdges[0].Dest != null)
            {
                path.BestPath = path.BestPath.AdjEdges[0].Dest;
            }
        }
    }
}
