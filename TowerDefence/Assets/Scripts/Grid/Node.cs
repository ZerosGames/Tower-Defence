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

    public Node()
    {

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
}

