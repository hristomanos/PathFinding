using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject g_TilePrefab;
    public int sizeX = 20;
    public int sizeY = 20;

    public Tile[,] g_Grid;


    private Dictionary<Tile, Tile[]> neighborDictionary;
    public Tile[] Neighbors(Tile tile)
    {
        return neighborDictionary[tile];
    }

    void Awake()
    {
        g_Grid = new Tile[sizeX, sizeY];
        neighborDictionary = new Dictionary<Tile, Tile[]>();
        GenerateMap(sizeX, sizeY);
    }


    void GenerateMap(int sizeX, int sizeY)
    {
        //Generate Map
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                g_Grid[x, y] = Instantiate(g_TilePrefab, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Tile>();
                g_Grid[x, y].Init(x, y);
            }
        }

        //Build Graph from map
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                List<Tile> neighbors = new List<Tile>();
                if (y < sizeY-1)
                    neighbors.Add(g_Grid[x, y + 1]);
                if (x < sizeX-1)
                    neighbors.Add(g_Grid[x + 1, y]);
                if (y > 0)
                    neighbors.Add(g_Grid[x, y - 1]);
                if (x > 0)
                    neighbors.Add(g_Grid[x - 1, y]);

                neighborDictionary.Add(g_Grid[x, y], neighbors.ToArray());
            }
        }
    }

    public void ResetTiles()
    {
        foreach(Tile t in g_Grid)
        {
            t.g_Color = Color.white;
            t.g_Text = "";

            switch (t._TileType)
            {
                case Tile.TileType.Plains:
                    t.g_Color = Color.white;
                    break;
                case Tile.TileType.Wall:
                    t.g_Color = Color.gray;
                    break;
                case Tile.TileType.Wood:
                    t.g_Color = Color.green;
                    break;
            }

        }
    }
}
