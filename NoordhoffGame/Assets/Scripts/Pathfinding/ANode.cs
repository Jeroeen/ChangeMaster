using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
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
