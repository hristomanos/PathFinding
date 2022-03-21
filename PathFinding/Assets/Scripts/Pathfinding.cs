using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private MapGenerator m_MapGenerator;

    private void Awake()
    {
        m_MapGenerator = FindObjectOfType<MapGenerator>();
    }





    //Conduct Breadth first search
    Queue<Tile> FloodFill(Tile startTile, Tile goalTile)
    {
        //Stores the tile that points towards the destination
        Dictionary<Tile, Tile> came_from = new Dictionary<Tile, Tile>();

        //A Queue where neighbour tiles are stored
        Queue<Tile> frontier = new Queue<Tile>();

        //A list of visited tiles
        List<Tile> reached = new List<Tile>();

        //Add to the queue starting from the goal node
        frontier.Enqueue(goalTile);

        //Repeat until frontier is empty
        while(frontier.Count > 0)
        {
            //Take from top most tile from the queue
            Tile currentTile = frontier.Dequeue();

            //Check for each neighbor
            foreach(Tile neighbor in m_MapGenerator.Neighbors(currentTile))
            {
                //We have not visited before and is not frontier
                if (reached.Contains(neighbor) == false && frontier.Contains(neighbor) == false)
                {
                    //Skip if wall
                    if (neighbor._TileType != Tile.TileType.Wall)
                    {
                        //Add neighbor to the queue
                        frontier.Enqueue(neighbor);

                        //Set current tile 
                        came_from[neighbor] = currentTile;
                    }
                }
            }
            //Add tile to list of visited 
            reached.Add(currentTile);
        }

        if (reached.Contains(startTile) == false)
            return null;

        //The path the player will follow
        Queue<Tile> path = new Queue<Tile>();

        //The tile where the player is currently standing
        Tile curPathTile = startTile;

        //Fill the path Queue with tiles the player needs to follow to get to their destination
        while(curPathTile != goalTile)
        {
            curPathTile = came_from[curPathTile];
            path.Enqueue(curPathTile);
        }

        //Return path that was just created
        return path;
    }














    Queue<Tile> Dijkstra(Tile start, Tile goal)
    {
        Dictionary<Tile, Tile> NextTileToGoal = new Dictionary<Tile, Tile>();//Determines for each tile where you need to go to reach the goal. Key=Tile, Value=Direction to Goal
        Dictionary<Tile, int> costToReachTile = new Dictionary<Tile, int>();//Total Movement Cost to reach the tile

        PriorityQueue<Tile> frontier = new PriorityQueue<Tile>();
        frontier.Enqueue(goal, 0);
        costToReachTile[goal] = 0;

        while (frontier.Count > 0)
        {
            Tile curTile = frontier.Dequeue();
            if (curTile == start)
                break;

            foreach (Tile neighbor in m_MapGenerator.Neighbors(curTile))
            {
                int newCost = costToReachTile[curTile] + neighbor._Cost;
                if (costToReachTile.ContainsKey(neighbor) == false || newCost < costToReachTile[neighbor])
                {
                    if (neighbor._TileType != Tile.TileType.Wall)
                    {
                        costToReachTile[neighbor] = newCost;
                        int priority = newCost;
                        frontier.Enqueue(neighbor, priority);
                        NextTileToGoal[neighbor] = curTile;
                        neighbor._Text = costToReachTile[neighbor].ToString();
                    }
                }
            }
        }

        //Get the Path

        //check if tile is reachable
        if (NextTileToGoal.ContainsKey(start) == false)
        {
            return null;
        }

        Queue<Tile> path = new Queue<Tile>();
        Tile pathTile = start;
        while (goal != pathTile)
        {
            pathTile = NextTileToGoal[pathTile];
            path.Enqueue(pathTile);
        }
        return path;
    }

    Queue<Tile> AStar(Tile start, Tile goal)
    {
        Dictionary<Tile, Tile> NextTileToGoal = new Dictionary<Tile, Tile>();//Determines for each tile where you need to go to reach the goal. Key=Tile, Value=Direction to Goal
        Dictionary<Tile, int> costToReachTile = new Dictionary<Tile, int>();//Total Movement Cost to reach the tile

        PriorityQueue<Tile> frontier = new PriorityQueue<Tile>();
        frontier.Enqueue(goal, 0);
        costToReachTile[goal] = 0;

        while (frontier.Count > 0)
        {
            Tile curTile = frontier.Dequeue();
            if (curTile == start)
                break;

            foreach (Tile neighbor in m_MapGenerator.Neighbors(curTile))
            {
                int newCost = costToReachTile[curTile] + neighbor._Cost;
                if (costToReachTile.ContainsKey(neighbor) == false || newCost < costToReachTile[neighbor])
                {
                    if (neighbor._TileType != Tile.TileType.Wall)
                    {
                        costToReachTile[neighbor] = newCost;
                        int priority = newCost + Distance(neighbor, start);
                        frontier.Enqueue(neighbor, priority);
                        NextTileToGoal[neighbor] = curTile;
                        neighbor._Text = costToReachTile[neighbor].ToString();
                    }
                }
            }
        }

        //Get the Path

        //check if tile is reachable
        if (NextTileToGoal.ContainsKey(start) == false)
        {
            return null;
        }

        Queue<Tile> path = new Queue<Tile>();
        Tile pathTile = start;
        while (goal != pathTile)
        {
            pathTile = NextTileToGoal[pathTile];
            path.Enqueue(pathTile);
        }
        return path;
    }

    /// <summary>
    /// Finds a path from starttile to endtile
    /// </summary>
    /// <returns>Returns a Queue which contains the Tiles, the player must move.</returns>
    public Queue<Tile> FindPath(Tile start, Tile end)
    {
        switch (_currentAlgorithm)
        {
            case Algorithm.FloodFill:
                return FloodFill(start, end);
            case Algorithm.Dijkstra:
                return Dijkstra(start, end);
            case Algorithm.AStar:
                return AStar(start, end);

        }

        return null;
    }







    /// <summary>
    /// Determines the Manhatten Distance between two tiles. (=How many Tiles the player must move to reach it)
    /// </summary>
    /// <returns>Distance in amount of Tiles the player must move</returns>
    int Distance(Tile t1, Tile t2)
    {
        return Mathf.Abs(t1._X - t2._X) + Mathf.Abs(t1._Y - t2._Y);
    }















    #region unimportant
    public enum Algorithm
    {
        FloodFill = 0,
        Dijkstra = 1,
        AStar = 2
    }

    public Algorithm _currentAlgorithm;
    private void Start()
    {
        _currentAlgorithm = Algorithm.Dijkstra;
       // TMPro.TMP_Dropdown dropDown = FindObjectOfType<TMPro.TMP_Dropdown>();
        //dropDown.onValueChanged.AddListener(OnAlgorithmChanged);
        //dropDown.value = PlayerPrefs.GetInt("currentAlgorithm");
    }


    public void OnAlgorithmChanged(int algorithmID)
    {
        _currentAlgorithm = (Algorithm)algorithmID;
        FindObjectOfType<PathTester>().RepaintMap();
        PlayerPrefs.SetInt("currentAlgorithm", (int)algorithmID);
        PlayerPrefs.Save();
    }
    #endregion
}
