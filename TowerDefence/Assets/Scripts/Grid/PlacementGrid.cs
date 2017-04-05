using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGrid : MonoBehaviour {

    public Node[,] Grid;

    private int MapHeight;
    private int MapWidth;

    [SerializeField]
    private float nodeRadius;

    [SerializeField]
    private float nodeDiameter;

    [SerializeField]    private int gridSizeX, gridSizeY;

    [SerializeField]
    private byte[,] meshData;
    public Transform player;

    public LayerMask Buildable;

    public PlacementMesh placementMesh;

    void Start () {

        MapHeight = GameManager.gameManager.GetMapHeight();
        MapWidth = GameManager.gameManager.GetMapWidth();

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(MapWidth / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(MapHeight / nodeDiameter);

        meshData = new byte[gridSizeX, gridSizeY]; 

        CreateGrid();
        placementMesh.GenerateMesh(meshData);
    }

    void CreateGrid()
    {
        Grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * MapWidth / 2 - Vector3.forward * MapHeight / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 WorldPos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                WorldPos.y = 0.5f;
                bool Placeable = Physics.CheckSphere(WorldPos, nodeRadius - 0.1f, Buildable);
                bool Walkable = !Physics.CheckSphere(WorldPos, nodeRadius - 0.1f, Buildable);
                Grid[x, y] = new Node(x, y, WorldPos, Placeable, Walkable, 2000, null, 0, Vector3.zero);
                if (Placeable)
                    meshData[x, y] = 1;
                else
                    meshData[x, y] = 0;
            }
        }
    }

    public Node NodeFromWorldPos(Vector3 _worldPos)
    {
        float percentX = (_worldPos.x + MapWidth / 2) / MapWidth;
        float percentY = (_worldPos.z + MapHeight / 2) / MapHeight;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

        return Grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(MapWidth, 1, MapHeight));

        if (Grid != null)
        {
            foreach (Node n in Grid)
            {
                Gizmos.color = Color.cyan;
                if (n != null)
                {
                    if (!n.Placeable && !n.Walkable)
                    {
                        Gizmos.color = Color.red;
                    }

                    else if (n.Walkable && !n.Placeable)
                    {
                        Gizmos.color = Color.green;
                    }

                    else if (n.Walkable && n.Placeable)
                    {
                        Gizmos.color = Color.magenta;
                    }
                    Gizmos.DrawCube(n.WorldPos, new Vector3(0.8f, 0.8f, 0.8f));
                }
            }
        }
    }
}
