using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NodeGrid
{
    public static Node[,] generateGrid(Vector3 _worldPos, int _mapWidth, int _mapHeight, int _gridSizeX, int _gridSizeY, float _nodeRadius)
    {
        float nodeDiameter = _nodeRadius * 2;

        Node[,] Grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = _worldPos - Vector3.right * _mapWidth / 2 - Vector3.forward * _mapHeight / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 WorldPos = worldBottomLeft + Vector3.right * (x * nodeDiameter + _nodeRadius) + Vector3.forward * (y * nodeDiameter + _nodeRadius);
                Grid[x, y] = new Node(x, y, WorldPos);
                Grid[x, y].Placeable = true;
            }
        }
        return Grid;
    }

    public static Node NodeFromWorldPos(Vector3 _worldPos, Node[,] _grid, int _mapWidth, int _mapHeight, int _gridSizeX, int _gridSizeY)
    {
        float percentX = (_worldPos.x + _mapWidth / 2) / _mapWidth;
        float percentY = (_worldPos.z + _mapHeight / 2) / _mapHeight;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
    }

    public static byte[,] GridToByteArray(Node[,] grid)
    {
        byte[,] returnArray = new byte[grid.GetLength(0), grid.GetLength(1)];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].Placeable)
                    returnArray[x, y] = 1;
                else
                    returnArray[x, y] = 0;
            }
        }

        return returnArray;
    }

    public static bool CheckMovement(int LastX, int LastY, int nextX, int nextY, int _gridSizeX, int _gridSizeY)
    {
        if (nextX < 0 || nextX > _gridSizeX || nextY < 0 || nextY > _gridSizeY)
            return false;

        if (LastX == nextX && LastY == nextY)
            return false;

        return true;
    }

    public static Node[,] NodeGridFromByteArray(Vector3 _worldPos, int _mapWidth, int _mapHeight, float _nodeRadius, byte[,] _array)
    {
        Node[,] nodeGrid = new Node[_array.GetLength(0), _array.GetLength(1)];

        float nodeDiameter = _nodeRadius * 2;

        Vector3 worldBottomLeft = _worldPos - Vector3.right * _mapWidth / 2 - Vector3.forward * _mapHeight / 2;

        for (int x = 0; x < _array.GetLength(0); x++)
        {
            for (int y = 0; y < _array.GetLength(1); y++)
            {
                Vector3 WorldPos = worldBottomLeft + Vector3.right * (x * nodeDiameter + _nodeRadius) + Vector3.forward * (y * nodeDiameter + _nodeRadius);
                nodeGrid[x, y] = new Node(x, y, WorldPos);
                if (_array[x, y] == 1)
                {
                    nodeGrid[x, y].Placeable = true;
                    nodeGrid[x, y].Walkable = false;
                }
                else
                {
                    nodeGrid[x, y].Placeable = false;
                    nodeGrid[x, y].Walkable = true;
                }
            }
        }

        return nodeGrid;
    }

    public static Node GetNodeFromByteArray(Node[,] nodeGrid, int _xIndex, int _yIndex)
    {      
        return nodeGrid[_xIndex, _yIndex];
    }
}
