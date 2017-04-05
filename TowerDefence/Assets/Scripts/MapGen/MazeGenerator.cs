using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MazeGenerator
{  
    public static byte[,] CreateMaze(byte[,] _mapData, int _width, int _height, Vector2 _startingNode)
    {
        System.Random rnd = new System.Random();
        List<Vector2> offsets = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
        Stack<Vector2> _tiletoTry = new Stack<Vector2>();

        _startingNode = Vector2.one;
        _tiletoTry.Push(_startingNode);

        //local variable to store neighbors to the current square as we work our way through the maze
        List<Vector2> neighbors;
        //as long as there are still tiles to try
        while (_tiletoTry.Count > 0)
        {
            //excavate the square we are on
            _mapData[(int)_startingNode.x, (int)_startingNode.y] = 0;
            //get all valid neighbors for the new tile
            neighbors = GetValidNeighbors(_mapData, _width, _height, _startingNode, offsets);
            //if there are any interesting looking neighbors
            if (neighbors.Count > 0)
            {
                //remember this tile, by putting it on the stack
                _tiletoTry.Push(_startingNode);
                //move on to a random of the neighboring tiles
                _startingNode = neighbors[rnd.Next(neighbors.Count)];
            }
            else
            {
                //if there were no neighbors to try, we are at a dead-end toss this tile out
                //(thereby returning to a previous tile in the list to check).
                _startingNode = _tiletoTry.Pop();
            }
        }
        return _mapData;
    }

    // ================================================
    // Get all the prospective neighboring tiles "centerTile" The tile to test
    // All and any valid neighbors</returns>
    private static List<Vector2> GetValidNeighbors(byte[,] _mapData, int gridX, int gridY,  Vector2 centerTile, List<Vector2> _offsets)
    {
        List<Vector2> validNeighbors = new List<Vector2>();
        //Check all four directions around the tile
        foreach (var offset in _offsets)
        {
            //find the neighbor's position
            Vector2 toCheck = new Vector2(centerTile.x + offset.x, centerTile.y + offset.y);
            //make sure the tile is not on both an even X-axis and an even Y-axis
            //to ensure we can get walls around all tunnels
            if (toCheck.x % 2 == 1 || toCheck.y % 2 == 1)
            {

                //if the potential neighbor is unexcavated (==1)
                //and still has three walls intact (new territory)
                if (_mapData[(int)toCheck.x, (int)toCheck.y] == 1 && HasThreeWallsIntact(_mapData, gridX, gridY, toCheck, _offsets))
                {

                    //add the neighbor
                    validNeighbors.Add(toCheck);
                }
            }
        }
        return validNeighbors;
    }
    // ================================================
    // Counts the number of intact walls around a tile
    //"Vector2ToCheck">The coordinates of the tile to check
    //Whether there are three intact walls (the tile has not been dug into earlier.
    private static bool HasThreeWallsIntact(byte[,] mapData, int gridX, int gridY, Vector2 Vector2ToCheck , List<Vector2> _offsets)
    {

        int intactWallCounter = 0;
        //Check all four directions around the tile
        foreach (var offset in _offsets)
        {

            //find the neighbor's position
            Vector2 neighborToCheck = new Vector2(Vector2ToCheck.x + offset.x, Vector2ToCheck.y + offset.y);
            //make sure it is inside the maze, and it hasn't been dug out yet
            if (IsInside(neighborToCheck, gridX, gridY) && mapData[(int)neighborToCheck.x, (int)neighborToCheck.y] == 1)
            {
                intactWallCounter++;
            }
        }
        //tell whether three walls are intact
        return intactWallCounter == 3;
    }

    // ================================================
    private static bool IsInside(Vector2 p, int gridX, int gridY)
    {
        //return p.x >= 0  p.y >= 0  p.x < width  p.y < height;
        return p.x >= 0 && p.y >= 0 && p.x < gridX && p.y < gridY;
    }
}