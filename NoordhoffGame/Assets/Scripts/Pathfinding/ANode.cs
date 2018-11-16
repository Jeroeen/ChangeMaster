using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pathfinding
{
    public class ANode
    {
        public string ID { get; set; }
        public List<Edge> AdjacentEdges { get; set; }
        public Vector3 Position { get; set; }

        public ANode(string id, Vector3 pos)
        {
            this.ID = id;
            this.Position = pos;
            AdjacentEdges = new List<Edge>();
        }
    }
}
