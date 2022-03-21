using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTester : MonoBehaviour
{

    private Tile m_StartTile;
    private Tile m_EndTile;

    public Pathfinding m_Pathfinding;
    public MapGenerator m_MapGenerator;

    private void Update()
    {
        HandleInput();
    }

    private void CalculatePath(Tile start, Tile end)
    {
        Queue<Tile> path = m_Pathfinding.FindPath(m_StartTile, m_EndTile);
        if (path == null)
            Debug.LogWarning("Goal not reachable");
        else
        {
            foreach (Tile t in path)
            {
                t.g_Color = new Color(1, 0.6f, 0);
            }

            m_EndTile.g_Color = Color.red;
            m_EndTile.g_Text = "End";
            m_StartTile.g_Color = Color.cyan;
            m_StartTile.g_Text = "Start";
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
        
            Tile tileUnderMouse = GetTileUnderMouse();
            if (tileUnderMouse != null && tileUnderMouse._TileType == Tile.TileType.Wall)
            {
                Debug.LogWarning("Can't start or end on Walls!");
                return;
            }

            if (tileUnderMouse != null)
            {
                m_StartTile = tileUnderMouse;
            }

            RepaintMap();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Tile tileUnderMouse = GetTileUnderMouse();
            if (tileUnderMouse != null && tileUnderMouse._TileType == Tile.TileType.Wall)
            {
                Debug.LogWarning("Can't start or end on Walls!");
                return;
            }


            if (tileUnderMouse != null)
            {
                m_EndTile = tileUnderMouse;
            }
            RepaintMap();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Tile tileUnderMouse = GetTileUnderMouse();
            if (tileUnderMouse != null)
            {
                tileUnderMouse._TileType = Tile.TileType.Plains;
                RepaintMap();
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            Tile tileUnderMouse = GetTileUnderMouse();
            if (tileUnderMouse != null)
            {
                tileUnderMouse._TileType = Tile.TileType.Wood;
                RepaintMap();
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            Tile tileUnderMouse = GetTileUnderMouse();
            if (tileUnderMouse != null)
            {
                tileUnderMouse._TileType = Tile.TileType.Wall;
                RepaintMap();
            }
        }

    }

    private Tile GetTileUnderMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            return hit.transform.GetComponent<Tile>();
        }
        else
        {
            return null;
        }
    }

    public void RepaintMap()
    {
        m_MapGenerator.ResetTiles();
        if (m_EndTile != null)
        {
            m_EndTile.g_Color = Color.red;
            m_EndTile.g_Text = "End";
        }

        if (m_StartTile != null)
        {
            m_StartTile.g_Color = Color.green;
            m_StartTile.g_Text = "Start";
        }



        if (m_StartTile != null && m_EndTile != null)
        {
            CalculatePath(m_StartTile, m_EndTile);
        }
    }
}
