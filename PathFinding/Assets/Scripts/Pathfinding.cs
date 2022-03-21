using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script implements the flood fill, Dijkstra and A* algorithms
public class Pathfinding : MonoBehaviour
{
    private MapGenerator m_MapGenerator;

    public enum Algorithm
    {
        FloodFill = 0,
        Dijkstra = 1,
        AStar = 2
    }

    public Algorithm m_CurrentAlgorithm;

    private void Awake()
    {
        m_MapGenerator = FindObjectOfType<MapGenerator>();
    }

   
    private void Start()
    {
        m_CurrentAlgorithm = Algorithm.Dijkstra;
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
        while (frontier.Count > 0)
        {
            //Take from top most tile from the queue
            Tile currentTile = frontier.Dequeue();

            //Check for each neighbor
            foreach (Tile neighbor in m_MapGenerator.Neighbors(currentTile))
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
        while (curPathTile != goalTile)
        {
            curPathTile = came_from[curPathTile];
            path.Enqueue(curPathTile);
        }

        //Return path that was just created
        return path;
    }



    Queue<Tile> Dijkstra(Tile startTile, Tile goalTile)
    {
        //Determines for each tile where the agent needs to go to reach the goal. Key=Tile, Value=Direction to Goal tile
        Dictionary<Tile, Tile> came_from = new Dictionary<Tile, Tile>();

        //Total Movement Cost to reach the tile
        Dictionary<Tile, int> cost_so_far = new Dictionary<Tile, int>();

        //Advanced frontier that prioritises amount of cost for each tile
        PriorityQueue<Tile> frontier = new PriorityQueue<Tile>();

        //Starting goal tile costs 0 turns
        frontier.Enqueue(goalTile, 0);

        //Initilise cost to reach the next tile dictionary
        cost_so_far[goalTile] = 0;

        //Explore till frontier is empty
        while (frontier.Count > 0)
        {
            //Starting with the smallest cost, in this case is always the goal.
            Tile curTile = frontier.Dequeue();

            //Stop if you reached the starting tile
            if (curTile == startTile)
                break;

            //Check for each neighbour
            foreach (Tile neighbor in m_MapGenerator.Neighbors(curTile))
            {
                //Save the cost to move towards the neighbor
                int newCost = cost_so_far[curTile] + neighbor.g_Cost;

                //Check if the neighbor has been added alread
                //or if the new cost is less than the current cost to reach the tile
                if (cost_so_far.ContainsKey(neighbor) == false || newCost < cost_so_far[neighbor])
                {
                    //Skip if it is a wall tile
                    if (neighbor._TileType != Tile.TileType.Wall)
                    {
                        //Set the new cost to reach the neighbor
                        cost_so_far[neighbor] = newCost;
                        //Add the new cost of the neighbour to the frontier
                        frontier.Enqueue(neighbor, newCost);
                        came_from[neighbor] = curTile;

                        //Displays the cost the agent needs to reach this destination
                        neighbor.g_Text = cost_so_far[neighbor].ToString();
                    }
                }
            }
        }

        //Get the Path

        //check if tile is reachable
        if (came_from.ContainsKey(startTile) == false)
        {
            return null;
        }

        Queue<Tile> path = new Queue<Tile>();
        Tile pathTile = startTile;
        while (goalTile != pathTile)
        {
            pathTile = came_from[pathTile];
            path.Enqueue(pathTile);
        }
        return path;
    }

    Queue<Tile> AStar(Tile startTile, Tile goalTile)
    {
        //Determines for each tile where you need to go to reach the goal. Key=Tile, Value=Direction to Goal
        Dictionary<Tile, Tile> came_from = new Dictionary<Tile, Tile>();

        //Total Movement Cost to reach the tile
        Dictionary<Tile, int> cost_so_far = new Dictionary<Tile, int>();

        //Advanced frontier that prioritises amount of cost for each tile
        PriorityQueue<Tile> frontier = new PriorityQueue<Tile>();


        //Initilise cost to reach the next tile dictionary
        frontier.Enqueue(goalTile, 0);
        cost_so_far[goalTile] = 0;

        //Check till frontier is empty
        while (frontier.Count > 0)
        {
            //Starting with the smallest cost, in this case is always the goal
            Tile curTile = frontier.Dequeue();

            //To take full advantage of A*, if the 
            if (curTile == startTile)
                break;

            foreach (Tile neighbor in m_MapGenerator.Neighbors(curTile))
            {
                //Save the cost to move towards the neighbor
                int newCost = cost_so_far[curTile] + neighbor.g_Cost;

                //Check if the neighbor has been added alread or if the new cost is less than the current cost to reach the tile
                if (cost_so_far.ContainsKey(neighbor) == false || newCost < cost_so_far[neighbor])
                {
                    //Skip if it is a wall tile
                    if (neighbor._TileType != Tile.TileType.Wall)
                    {
                        //Set the new cost to reach the neighbor
                        cost_so_far[neighbor] = newCost;

                        //A*, it will not compute tiles that are beyond the cheapest path
                        int priority = newCost + Distance(neighbor, startTile); 

                        //Add the new cost of the neighbour to the frontier
                        frontier.Enqueue(neighbor, priority);
                        came_from[neighbor] = curTile;
                        neighbor.g_Text = cost_so_far[neighbor].ToString();
                    }
                }
            }
        }

        //Get the Path

        //check if tile is reachable
        if (came_from.ContainsKey(startTile) == false)
        {
            return null;
        }

        Queue<Tile> path = new Queue<Tile>();
        Tile pathTile = startTile;
        while (goalTile != pathTile)
        {
            pathTile = came_from[pathTile];
            path.Enqueue(pathTile);
        }
        return path;
    }

    public Queue<Tile> FindPath(Tile start, Tile end)
    {
        switch (m_CurrentAlgorithm)
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


    //Manhattan distance, Determines the amount of blocks between two tiles. (= How many Tiles the player must move to reach it)
    int Distance(Tile t1, Tile t2)
    {
        return Mathf.Abs(t1._X - t2._X) + Mathf.Abs(t1._Y - t2._Y);
    }

   

}
