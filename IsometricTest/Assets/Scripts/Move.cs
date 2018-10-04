using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Move : MonoBehaviour
{
    public Tilemap Tilemap;
    private Graph graph;
    private Path path;

    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph(Tilemap);
        path = new Path(graph);
        
    }

    // Update is called once per frame
    void Update()
    {
        //path.Render();
        graph.Render();
    }
}
