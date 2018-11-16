using System;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Assets.Scripts.Pathfinding
{
    public class Path
    {
        public ANode BestPath { get; set; }
        //private World myWorld;
        private Graph graph;
        const int distANodes = 1;
        private UnityEngine.Tilemaps.Tilemap tileMap;

        public Path(Graph graph, UnityEngine.Tilemaps.Tilemap tilemap)
        {
            this.graph = graph;
            tileMap = tilemap;
        }

        public ANode FindBestPath(string start, string end)
        {
            BestPath = graph.AStar(graph.ANodeMap[start], graph.ANodeMap[end]);
            return BestPath;
        }

        public string FindNearestANode(Vector3Int pos)
        {
            BoundsInt area = tileMap.cellBounds;
            string val = pos.x + ";" + pos.y;
            if (graph.ANodeMap.ContainsKey(val))
            {
                return val;
            }
            else
            {
                return NearestNode(pos);
            }
        }

        private string NearestNode(Vector3Int pos)
        {
            Vector3Int[] answers = new Vector3Int[4];
            int[] costs = new int[4];
            costs[0] = 1;
            costs[1] = 1;
            costs[2] = 1;
            costs[3] = 1;

            answers[0] = NearestNodeLeft(pos, ref costs[0]);
            answers[1] = NearestNodeRight(pos, ref costs[1]);
            answers[2] = NearestNodeUp(pos, ref costs[2]);
            answers[3] = NearestNodeDown(pos, ref costs[3]);

            int smallest = Int32.MaxValue;
            Vector3Int bestResult = new Vector3Int();
            for (int i = 0; i < costs.Length; i++)
            {
                if (costs[i] < smallest)
                {
                    bestResult = answers[i];
                    smallest = costs[i];
                }
            }

            return bestResult.x + ";" + bestResult.y;
        }

        private Vector3Int NearestNodeRight(Vector3Int cell, ref int stack)
        {
            try
            {
                Vector3Int potential = new Vector3Int(cell.x + stack, cell.y, cell.z);

                if (graph.ANodeMap.ContainsKey(potential.x + ";" + potential.y) || stack >= graph.ANodeMap.Count)
                {
                    return potential;
                }

                stack += 1;
                return NearestNodeRight(cell, ref stack);
            }
            catch (StackOverflowException ex)
            {
                Debug.Log("HIER GAAT HET FOUT! Right");
            }
            return new Vector3Int();
        }

        private Vector3Int NearestNodeLeft(Vector3Int cell, ref int stack)
        {
            try
            {
                Vector3Int potential = new Vector3Int(cell.x - stack, cell.y, cell.z);

                if (graph.ANodeMap.ContainsKey(potential.x + ";" + potential.y) || stack >= graph.ANodeMap.Count)
                {
                    return potential;
                }

                stack += 1;
                return NearestNodeLeft(cell, ref stack);
            }
            catch (StackOverflowException ex)
            {
                Debug.Log("HIER GAAT HET FOUT! Left");
            }
            return new Vector3Int();
        }

        private Vector3Int NearestNodeUp(Vector3Int cell, ref int stack)
        {
            try
            {
                Vector3Int potential = new Vector3Int(cell.x, cell.y + stack, cell.z);

                if (graph.ANodeMap.ContainsKey(potential.x + ";" + potential.y) || stack >= graph.ANodeMap.Count)
                {
                    return potential;
                }

                stack += 1;
                return NearestNodeUp(cell, ref stack);
            }
            catch (StackOverflowException ex)
            {
                Debug.Log("HIER GAAT HET FOUT! Up");
            }
            return new Vector3Int();
        }

        private Vector3Int NearestNodeDown(Vector3Int cell, ref int stack)
        {
            try
            {
                Vector3Int potential = new Vector3Int(cell.x, cell.y - stack, cell.z);

                if (graph.ANodeMap.ContainsKey(potential.x + ";" + potential.y) || stack >= graph.ANodeMap.Count)
                {
                    return potential;
                }

                stack += 1;
                return NearestNodeDown(cell, ref stack);
            }
            catch (StackOverflowException ex)
            {
                Debug.Log("HIER GAAT HET FOUT! Down");
            }

            return new Vector3Int();
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
                foreach (Edge edge in currANode.AdjacentEdges)
                {
                    dest = edge.Destination;
                    pos = new Vector2(pos.x, pos.y);
                    destPos = new Vector2(dest.Position.x, dest.Position.y);
                    //g.DrawLine(new Pen(Color.Cyan, 1), pos, destPos);
                    Debug.DrawLine(pos, destPos, Color.yellow);
                }

                pos = new Vector2(currANode.Position.x - 1.1f, currANode.Position.y - 1.1f);
                //g.FillEllipse(Brushes.Cyan, pos.X, pos.Y, 3, 3);


                if (currANode.AdjacentEdges.Count == 0)
                {
                    break;
                }

                currANode = dest;
            }
        }
    }
}
