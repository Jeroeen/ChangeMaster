using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Edge
{
    public ANode Destination { get; set; }
    public double Cost { get; set; }

    public Edge(ANode node, double cost)
    {
        Destination = node;
        Cost = cost;
    }
}
