using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Edge
{
    public ANode Dest { get; set; }
    public double Cost { get; set; }

    public Edge(ANode node, double cost)
    {
        Dest = node;
        Cost = cost;
    }
}
