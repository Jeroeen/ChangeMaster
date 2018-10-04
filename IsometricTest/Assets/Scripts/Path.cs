using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Assets.Scripts
{
    public class Path
    {
        public ANode BestPath { get; set; }
        //private World myWorld;
        private Graph myGraph;
        const int distANodes = 8;
        
        public Path(Graph graph)
        {
            myGraph = graph;
        }

        public ANode FindBestPath(string start, string end)
        {
            BestPath = myGraph.AStar(myGraph.ANodeMap[start], myGraph.ANodeMap[end]);
            return BestPath;
        }

        public string FindNearestANode(Vector2 pos)
        {
            int xVal = (int)pos.x;
            int yVal = (int)pos.y;

            for (int i = 0; i < distANodes * 2; i++)
            {
                string xMin = (xVal - i) + ";";
                string xPlus = (xVal + i) + ";";

                for (int j = 0; j < distANodes * 2; j++)
                {
                    string findPosMin = xMin + (yVal - j);
                    string findPosPlus = xPlus + (yVal + j);

                    if (myGraph.ANodeMap.ContainsKey(findPosMin))
                    {
                        return findPosMin;
                    }
                    if (myGraph.ANodeMap.ContainsKey(findPosPlus))
                    {
                        return findPosPlus;
                    }
                }
            }
            return "notfound";
        }

        public void Render()
        {
            Vector2 pos;
            Vector2 destPos;
            ANode dest = null;

            // Draw A* ANodes and edges
            if (BestPath == null)
            {
                return;
            }
            ANode currANode = BestPath;
            while (currANode != null)
            {
                pos = new Vector2(currANode.Position.x, currANode.Position.y);
                foreach (Edge edge in currANode.AdjEdges)
                {
                    dest = edge.Dest;
                    pos = new Vector2(pos.x, pos.y);
                    destPos = new Vector2(dest.Position.x, dest.Position.y);
                    //g.DrawLine(new Pen(Color.Cyan, 1), pos, destPos);
                    Debug.DrawLine(pos, destPos, Color.green);
                }

                pos = new Vector2(currANode.Position.x - 1.1f, currANode.Position.y - 1.1f);
                //g.FillEllipse(Brushes.Cyan, pos.X, pos.Y, 3, 3);
                

                if (currANode.AdjEdges.Count == 0)
                {
                    break;
                }

                currANode = dest;
            }
        }
    }
}
