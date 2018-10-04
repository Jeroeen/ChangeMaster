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
        public List<Edge> AdjEdges { get; set; }
        public Vector2 Position { get; set; }

        public ANode(string id, Vector2 pos)
        {
            this.ID = id;
            this.Position = pos;
            AdjEdges = new List<Edge>();
        }
    }
}
