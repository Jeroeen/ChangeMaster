using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;
using Color = UnityEngine.Color;

public class Graph
{
    public Dictionary<string, ANode> ANodeMap { get; }
    const int distNodes = 1;
    private Tilemap tileMap;
    private GridLayout gridLayout;

    public Graph(Tilemap tmap, GridLayout glayout)
    {
        tileMap = tmap;
        gridLayout = glayout;
        ANodeMap = new Dictionary<string, ANode>();
        CreateGraph2();
    }

    public void AddANode(string id, Vector3 pos)
    {
        ANodeMap[id] = new ANode(id, pos);
    }

    public void AddEdge(string start, string end)
    {
        double cost;
        ANode startANode = ANodeMap[start];
        ANode endANode = ANodeMap[end];
        cost = CalculateEuclidean(startANode, endANode);

        Edge e = new Edge(ANodeMap[end], cost);
        ANodeMap[start].AdjacentEdges.Add(e);
    }

    public ANode AStar(ANode start, ANode end)
    {
        List<ANode> closed = new List<ANode>();
        List<ANode> open = new List<ANode>() { start };

        // Remember the previous ANode it came from
        Dictionary<ANode, ANode> prevANode = new Dictionary<ANode, ANode>();

        // The cost of start to adjacent ANode
        Dictionary<ANode, Double> gCost = new Dictionary<ANode, Double>() { };
        foreach (KeyValuePair<string, ANode> ANode in ANodeMap)
        {
            gCost[ANode.Value] = Double.MaxValue;
        }

        gCost[start] = 0;

        // The total cost of start to end ANode
        Dictionary<ANode, Double> fCost = new Dictionary<ANode, Double>() { };
        foreach (KeyValuePair<string, ANode> ANode in ANodeMap)
        {
            fCost[ANode.Value] = Double.MaxValue;
        }

        fCost[start] = CalculateEuclidean(start, end);

        while (open.Count > 0)
        {
            // Get the ANode with lowest fScore value
            ANode current = open[0];
            for (int i = 0; i < open.Count; i++)
            {
                if (fCost[current] > fCost[open[i]])
                {
                    current = open[i];
                }
            }

            if (current == end)
            {
                return ConstructPath(prevANode, current);
            }

            open.Remove(current);
            closed.Add(current);

            foreach (Edge neighbourEdge in current.AdjacentEdges)
            {
                ANode neighbourANode = neighbourEdge.Destination;
                if (closed.Contains(neighbourANode))
                {
                    // Ignore the neighbour which is evaluated
                    continue;
                }

                // Explore a new ANode
                if (!open.Contains(neighbourANode))
                {
                    open.Add(neighbourANode);
                }

                // Distance from start to a neighbour
                int tentative_gScore = (int)(gCost[current] + neighbourEdge.Cost);
                if (tentative_gScore >= gCost[neighbourANode])
                {
                    // This is not a better path
                    continue;
                }

                // This path is the best until now, so we save it.
                prevANode[neighbourANode] = current;
                gCost[neighbourANode] = tentative_gScore;
                fCost[neighbourANode] = gCost[neighbourANode] + CalculateEuclidean(neighbourANode, end);
            }
        }

        // Failure
        return null;
    }

    // Get the ANodes connected to eachother via edges
    private ANode ConstructPath(Dictionary<ANode, ANode> map, ANode current)
    {
        // Make a new ANode out of the old one, so that old edges aren't taken over
        ANode totalPath = new ANode(current.ID, current.Position);

        while (map.ContainsKey(current) && map[current] != null)
        {
            current = map[current];
            ANode newANode = new ANode(current.ID, current.Position);
            double edgeCost = CalculateEuclidean(newANode, totalPath);
            newANode.AdjacentEdges.Add(new Edge(totalPath, edgeCost));
            totalPath = newANode;
        }

        return totalPath;
    }

    // Manhatten heuristic, good for squares without diagonals
    public double CalculateManhattan(ANode start, ANode dest)
    {
        double absoluteX = Math.Abs(start.Position.x - dest.Position.x);
        double absoluteY = Math.Abs(start.Position.y - dest.Position.y);
        int D = 1;
        return D * (absoluteX + absoluteY);
    }

    // Euclidean heuristic, good for squares with diagonals
    public double CalculateEuclidean(ANode start, ANode dest)
    {
        double dx = Math.Abs(start.Position.x - dest.Position.x);
        double dy = Math.Abs(start.Position.y - dest.Position.y);
        return Math.Sqrt(dx * dx + dy * dy);
    }

    // Dijkstra algorithm for finding the nearest item that may appear in multiple places in the map
    public ANode Dijkstra(ANode source, ANode target)
    {
        // A list with all the unvisited ANodes
        List<ANode> open = new List<ANode>();
        // A map with all ANodes and their distance / cost
        Dictionary<ANode, double> cost = new Dictionary<ANode, double>();
        // A map that keeps track of each previous ANode to given ANode
        Dictionary<ANode, ANode> prevANode = new Dictionary<ANode, ANode>();

        // Initialization
        foreach (KeyValuePair<string, ANode> ANode in ANodeMap)
        {
            cost[ANode.Value] = Double.MaxValue;
            prevANode[ANode.Value] = null;
            open.Add(ANode.Value);
        }

        cost[source] = 0;

        // Executing algorithm
        while (open.Count > 0)
        {
            // Find the ANode with the lowest cost
            ANode current = open[0];
            for (int i = 0; i < open.Count; i++)
            {
                if (cost[open[i]] < cost[current])
                {
                    current = open[i];
                }
            }

            // To make sure the ANodes won't be visited again, remove it
            open.Remove(current);

            // If target has been found, terminate
            if (current == target)
            {
                ANode path = ConstructPath(prevANode, current);
                return path;
            }

            // Visit each neighbour via the connected edge of the current ANode
            foreach (Edge e in current.AdjacentEdges)
            {
                ANode neighbour = e.Destination;
                double newCost = cost[current] + e.Cost;
                if (newCost < cost[neighbour])
                {
                    cost[neighbour] = newCost;
                    prevANode[neighbour] = current;
                }
            }
        }

        return null;
    }

    public void RemoveNode(string input)
    {
        foreach (KeyValuePair<string, ANode> node in ANodeMap)
        {
            List<Edge> edges = node.Value.AdjacentEdges;
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].Destination.ID == input)
                {
                    node.Value.AdjacentEdges.Remove(edges[i]);
                }
            }
        }

        ANodeMap.Remove(input);
    }

    // Creates a complete graph in this order:
    // 1. Generate all the nodes
    // 2. Combine each node with edges for both going from start to end and end to start
    // 3. Remove all nodes that are placed on top of static objects like walls
    public void CreateGraph2()
    {

        //tileMap.CompressBounds();

        //BoundsInt area = tileMap.cellBounds;
        //TileBase[] tiles = tileMap.GetTilesBlock(area);

        //for (int y = 0; y < area.size.y; y++)
        //{
        //    for (int x = 0; x < area.size.x; x++)
        //    {
        //        for (int z = 0; z < area.size.z; z++)
        //        {
        //            TileBase tile = tiles[x + z + y * area.size.x];
        //            if (tile != null)
        //            {
        //                //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
        //                if (tile.name.Contains("brown") || tile.name.Contains("black_white"))
        //                {
        //                    Vector3 vector = gridLayout.CellToWorld(new Vector3Int(x, y, z));
        //                    string coords = x + ";" + y;
        //                    AddANode(coords, vector);
        //                }

        //            }
        //        }
        //    }
        //}

        foreach (var pos in tileMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = gridLayout.CellToWorld(localPlace);
            string coords = pos.x + ";" + pos.y;
            if (tileMap.HasTile(localPlace) && pos.z < 1)
            {
                TileBase tile = tileMap.GetTile(localPlace);
                if (tile.name.Contains("tegel bruin") || tile.name.Contains("tegel vloer"))
                {
                    place.z = 10;
                    AddANode(coords, place);
                }
            }
            else if (tileMap.HasTile(localPlace) && pos.z >= 1)
            {
                if (ANodeMap.ContainsKey(coords))
                {
                    RemoveNode(coords);
                }
            }
        }
        
        //Generate edges
        for (int y = -12; y < 4; y++)
        {
            for (int x = 0; x < 17; x++)
            {
                string ownVal = x + ";" + y;
                //Debug.Log("x:" + x + " y:" + y);
                if (ANodeMap.ContainsKey(ownVal))
                {
                    string valWest = (x - 1) + ";" + y;
                    string valEast = (x + 1) + ";" + y;
                    string valNorth = x + ";" + (y - 1);
                    string valSouth = x + ";" + (y + 1);
                    string valNorthWest = (x - 1) + ";" + (y - 1);
                    string valNorthEast = (x + 1) + ";" + (y - 1);
                    string valSouthWest = (x - 1) + ";" + (y + 1);
                    string valSouthEast = (x + 1) + ";" + (y + 1);

                    // West
                    if (x > 0 && ANodeMap.ContainsKey(valWest))
                    {
                        AddEdge(ownVal, valWest);
                        AddEdge(valWest, ownVal);
                    }

                    // East
                    if (x < 16 - 1 && ANodeMap.ContainsKey(valEast))
                    {
                        AddEdge(ownVal, valEast);
                        AddEdge(valEast, ownVal);
                    }

                    // North
                    if (y > 0 && ANodeMap.ContainsKey(valNorth))
                    {
                        AddEdge(ownVal, valNorth);
                        AddEdge(valNorth, ownVal);
                    }

                    // South
                    if (y < 16 - 1 && ANodeMap.ContainsKey(valSouth))
                    {
                        AddEdge(ownVal, valSouth);
                        AddEdge(valSouth, ownVal);
                    }

                    //// NorthWest
                    //if (x > 0 && y > 0 && ANodeMap.ContainsKey(valNorthWest))
                    //{
                    //    AddEdge(ownVal, valNorthWest);
                    //    AddEdge(valNorthWest, ownVal);
                    //}

                    //// NorthEast
                    //if (x < 16 - 1 && y > 0 && ANodeMap.ContainsKey(valNorthEast))
                    //{
                    //    AddEdge(ownVal, valNorthEast);
                    //    AddEdge(valNorthEast, ownVal);
                    //}

                    //// SouthWest
                    //if (x > 0 && y < 16 - 1 && ANodeMap.ContainsKey(valSouthWest))
                    //{
                    //    AddEdge(ownVal, valSouthWest);
                    //    AddEdge(valSouthWest, ownVal);
                    //}

                    //// SouthEast
                    //if (x < 16 - 1 && y < 16 - 1 && ANodeMap.ContainsKey(valSouthEast))
                    //{
                    //    AddEdge(ownVal, valSouthEast);
                    //    AddEdge(valSouthEast, ownVal);
                    //}
                }
            }
        }
    }

    // Remove unnecessary nodes
    //List<ANode> nodeList = ANodeMap.Values.ToList();
    //foreach (ANode t in nodeList)
    //{
    //    foreach (StaticEntity entity in myWorld.StaticEntities)
    //    {
    //        if (entity.IntersectsPoint(t.Position))
    //        {
    //            RemoveNode(t.ID);
    //        }
    //    }
    //}


    public void Render()
    {
        Vector2 pos;
        Vector2 destPos;
        ANode dest = null;

        // Draw all edges
        foreach (KeyValuePair<string, ANode> node in ANodeMap)
        {
            pos = new Vector2(node.Value.Position.x, node.Value.Position.y);

            foreach (Edge edge in node.Value.AdjacentEdges)
            {
                dest = edge.Destination;
                destPos = new Vector2(dest.Position.x, dest.Position.y);
                Debug.DrawLine(new Vector3(pos.x, pos.y, -1), new Vector3(destPos.x, destPos.y, -1), Color.magenta);
                //Debug.Log("pos: " + pos + ", destpos: " + destPos);
            }
        }

        //Draw all nodes
        foreach (KeyValuePair<string, ANode> node in ANodeMap)
        {
            pos = new Vector2(node.Value.Position.x, node.Value.Position.y);
            Debug.DrawLine(new Vector3(pos.x - 0.1f, pos.y, -1), new Vector3(pos.x + 0.1f, pos.y, -1), Color.cyan);
            Debug.DrawLine(new Vector3(pos.x, pos.y - 0.1f, -1), new Vector3(pos.x, pos.y + 0.1f, -1), Color.cyan);

            //g.FillEllipse(new SolidBrush(node.Value.Color), pos.X, pos.Y, 3, 3);
        }
    }


}

