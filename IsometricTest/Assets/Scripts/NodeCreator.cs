using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeCreator : MonoBehaviour
{
    public Tilemap tmap;
    public GameObject nodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        BoundsInt area = tmap.cellBounds;
        TileBase[] tiles = tmap.GetTilesBlock(area);
        
        for (int x = 0; x < area.size.x; x++)
        {
            for (int y = 0; y < area.size.y; y++)
            {
                TileBase tile = tiles[x + y * area.size.x];
                if (tile != null)
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    if (tile.name.Contains("green"))
                    {
                        nodePrefab.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else
                    {
                        nodePrefab.GetComponent<SpriteRenderer>().color = Color.red;
                    }

                    float xx = x + y;
                    float yy = ((float)y - (float)x) / 2f;

                    GameObject node = (GameObject)Instantiate(nodePrefab, new Vector3(xx / 2, yy / 2, -1), Quaternion.Euler(0,0,0));
                    WorldTile wt = node.GetComponent<WorldTile>();
                    wt.gridX = x;
                    wt.gridY = y;
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
    }
}