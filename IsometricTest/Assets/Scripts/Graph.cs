using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Assets.Scripts;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using Color = UnityEngine.Color;

public class Graph
{
    public Dictionary<string, ANode> ANodeMap { get; }
    const int distNodes = 8;
    private Tilemap tileMap;

    public Graph(Tilemap tmap)
    {
        tileMap = tmap;
        ANodeMap = new Dictionary<string, ANode>();
        CreateGraph(distNodes);
    }

    public void AddANode(string id, Vector2 pos)
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
        ANodeMap[start].AdjEdges.Add(e);
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

            foreach (Edge neighbourEdge in current.AdjEdges)
            {
                ANode neighbourANode = neighbourEdge.Dest;
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
            newANode.AdjEdges.Add(new Edge(totalPath, edgeCost));
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
            foreach (Edge e in current.AdjEdges)
            {
                ANode neighbour = e.Dest;
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
            List<Edge> edges = node.Value.AdjEdges;
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].Dest.ID == input)
                {
                    node.Value.AdjEdges.Remove(edges[i]);
                }
            }
        }
        ANodeMap.Remove(input);
    }

    // Creates a complete graph in this order:
    // 1. Generate all the nodes
    // 2. Combine each node with edges for both going from start to end and end to start
    // 3. Remove all nodes that are placed on top of static objects like walls
    public void CreateGraph(int distNodes)
    {
        int amountNodesX = 100 / distNodes;
        int amountNodesY = 100 / distNodes;

        float vectorX, vectorY;
        string val, val2;

        // Make nodes
        for (int i = 0; i < amountNodesY; i++)
        {
            for (int j = 0; j < amountNodesX; j++)
            {
                vectorX = j * distNodes + 5;
                vectorY = i * distNodes + 4;
                val = vectorX + ";" + vectorY;
                Vector2 vector = new Vector2(vectorX, vectorY);

                AddANode(val, vector);
            }
        }
        //Generate edges
        for (int i = 0; i < amountNodesY; i++)
        {
            for (int j = 0; j < amountNodesX; j++)
            {
                vectorX = j * distNodes + 5;
                vectorY = i * distNodes + 4;
                val = vectorX + ";" + vectorY;
                vectorX = (j + 1) * distNodes + 5;
                vectorY = i * distNodes + 4;
                val2 = vectorX + ";" + vectorY;

                // Horizontal
                if (j < amountNodesX - 1)
                {
                    AddEdge(val, val2);
                    AddEdge(val2, val);
                }

                // Vertical
                vectorX = j * distNodes + 5;
                vectorY = (i + 1) * distNodes + 4;
                val2 = vectorX + ";" + vectorY;
                if (i < amountNodesY - 1)
                {
                    AddEdge(val, val2);
                    AddEdge(val2, val);
                }

                // Diagonal \ 
                vectorX = (j + 1) * distNodes + 5;
                vectorY = (i + 1) * distNodes + 4;
                val2 = vectorX + ";" + vectorY;
                if (i < amountNodesY - 1 && j < amountNodesX - 1)
                {
                    AddEdge(val, val2);
                    AddEdge(val2, val);
                }

                // Diagonal /
                vectorX = (j - 1) * distNodes + 5;
                vectorY = (i + 1) * distNodes + 4;
                val2 = vectorX + ";" + vectorY;
                if (i < amountNodesY - 1 && j > 0)
                {
                    AddEdge(val, val2);
                    AddEdge(val2, val);
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
    }

    public void Render()
    {
        Vector2 pos;
        Vector2 destPos;
        ANode dest = null;

        // Draw all edges
        foreach (KeyValuePair<string, ANode> node in ANodeMap)
        {
            pos = new Vector2(node.Value.Position.x, node.Value.Position.y);

            foreach (Edge edge in node.Value.AdjEdges)
            {
                dest = edge.Dest;
                destPos = new Vector2(dest.Position.x, dest.Position.y);
                Debug.DrawLine(new Vector3(pos.x, pos.y, -1), new Vector3(destPos.x, destPos.y, -1), Color.green);
                Debug.Log("pos: " + pos + ", destpos: " + destPos);

                //g.DrawLine(new Pen(Color.Orange, 1), pos, destPos);
            }
        }

        //Draw all nodes
        foreach (KeyValuePair<string, ANode> node in ANodeMap)
        {
            pos = new Vector2(node.Value.Position.x - 1.1f, node.Value.Position.y - 1.1f);
            Debug.DrawLine(new Vector3(pos.x, pos.y, -1), new Vector3(pos.x + 1, pos.y + 1, -1), Color.cyan);

            //g.FillEllipse(new SolidBrush(node.Value.Color), pos.X, pos.Y, 3, 3);
        }

    }
}
