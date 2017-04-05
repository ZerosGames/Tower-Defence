using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gridX;
    public int gridY;
    public Vector3 WorldPos;
    public bool Placeable;
    public bool Walkable;
    public int pathDistance;
    public Node parent;
    public int pathingWeight;

    public Vector3 dirVector;

    public int hCost;
    public int gCost;
    public Node Parent;

    public bool isStartNode;
    public bool isEndNode;

    public Node()
    {

    }

    //public Node(Vector3 _worldPos, int _gridX, int _gridY, bool _walkable)
    //{
    //    WorldPos = _worldPos;
    //    gridX = _gridX;
    //    gridY = _gridY;
    //    Walkable = _walkable;
    //}

    public Node(int _gridX, int _gridY, Vector3 _WorldPos)
    {
        gridX = _gridX;
        gridY = _gridY;
        WorldPos = _WorldPos;
    }

    public Node(int _gridX, int _gridY, Vector3 _WorldPos, bool _placeable, bool _walkable, int _pathDistance, Node _parent, int _pathingWeight, Vector3 _dirVector)
    {
        gridX = _gridX;
        gridY = _gridY;
        WorldPos = _WorldPos;
        Placeable = _placeable;
        Walkable = _walkable;
        pathDistance = _pathDistance;
        parent = _parent;
        pathingWeight = _pathingWeight;
        dirVector = _dirVector;
    }

    public Vector3 GetDirectionVector()
    {
        //Vector3 Direction = new Vector3(parent.WorldPos.x - WorldPos.x, 0, parent.WorldPos.z - WorldPos.z).normalized;
        return dirVector;
    }

    public int fCost
    {
        get
        {
            return gCost + fCost;
        }
    }

}

