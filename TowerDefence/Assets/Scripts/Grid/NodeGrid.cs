using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour {

    public Node[,] Grid;

    private int MapHeight;
    private int MapWidth;
    private int halfMapHeight;
    private int halfMapWidth;

    public Transform player;

    public LayerMask Buildable;

    public static NodeGrid nodeGrid;

    public bool GridUpdate = false;

    public GameObject Text;

    public bool debug = false;

    void Awake()
    {
        if(nodeGrid != null)
        {
            Debug.LogError("Two NodeGrids In Scene");
            return;
        }

        nodeGrid = this;
    }

	void Start ()
    {
        MapHeight = GameManager.gameManager.GetMapHeight();
        MapWidth = GameManager.gameManager.GetMapWidth();
        halfMapHeight = MapHeight / 2;
        halfMapWidth = MapWidth / 2;

        Grid = new Node[GameManager.gameManager.GetMapWidth(), GameManager.gameManager.GetMapHeight()];

        for (int x = 0; x < GameManager.gameManager.GetMapWidth(); x++)
        {
            for (int y = 0; y < GameManager.gameManager.GetMapHeight(); y++)
            {
                Vector3 WorldPos = new Vector3(x - halfMapWidth, 2f, y - halfMapHeight);
                bool Placeable = Physics.CheckBox(WorldPos, new Vector3(0.4f, 0.5f, 0.4f), Quaternion.identity, Buildable);
                bool Walkable = !Physics.CheckBox(WorldPos, new Vector3(0.4f, 0.5f, 0.4f), Quaternion.identity, Buildable);
                Grid[x, y] = new Node(x, y, WorldPos, Placeable, Walkable, 2000, null, 0, Vector3.zero);
            }
        }

        GenerateCostmap(Grid[halfMapWidth, halfMapHeight], MapWidth, MapHeight);
        GenerateWeightsMap();
        GetVectorField(Grid[halfMapWidth, halfMapHeight], MapWidth, MapHeight);

        if (debug)
        {
            DrawCostMap();
        }
    }

    public Node NodeFromWorldPos(Vector3 _worldPos)
    {
        float percentX = (_worldPos.x + MapWidth / 2) / MapWidth;
        float percentY = (_worldPos.z + MapHeight / 2) / MapHeight;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt(MapWidth * percentX);
        int y = Mathf.RoundToInt(MapHeight * percentY);

        return Grid[x,y];
    }

    public void GenerateWeightsMap()
    {
        foreach (Node g in Grid)
        {
            List<Node> Neighbor = GetNSEWNeighbouringNodes(g, MapWidth, MapHeight);

            foreach (Node n in Neighbor)
            {
                if (n.Walkable)
                    continue;

                g.pathingWeight = 5;

                g.pathDistance += g.pathingWeight;
                break;
            }
        }
    }

    public void GenerateCostmap(Node _destination, int _mapWidth, int _mapHeight)
    {
        Queue<Node> Frontier = new Queue<Node>();
        List<Node> Marked = new List<Node>();

        _destination.pathDistance = 0;
        Frontier.Enqueue(_destination);
        Marked.Add(_destination);

        while (Frontier.Count > 0)
        {
            Node Current = Frontier.Dequeue();

            List<Node> NeighbouringNodes = GetNeighbouringNodes(Current, _mapHeight, _mapWidth);

            foreach (Node _node in NeighbouringNodes)
            {
                if (Marked.Contains(_node) || _node.Walkable == false)
                {
                    if (_node.Walkable == false)
                    {
                        _node.pathDistance = 2000;
                    }

                    continue;
                }
                else
                {
                    _node.pathDistance = 0;
                    _node.parent = null;
                }

                _node.pathDistance += (Current.pathDistance + 1);
                _node.parent = Current;
                Frontier.Enqueue(_node);
                Marked.Add(_node);
            }
        }
    }

    private List<Node> GetNSEWNeighbouringNodes(Node _node, int _width, int _heigth)
    {
        List<Node> Neighbours = new List<Node>();
      
        for (int y = -1; y <= 1; y++)
        {
            if (y == 0)
            {
                continue;
            }

            int CheckY = _node.gridY + y;

            if (CheckY >= 0 && CheckY < _heigth)
            {
                Neighbours.Add(Grid[_node.gridX, CheckY]);
            }
        }

        for (int x = -1; x <= 1; x++)
        {
            if (x == 0)
            {
                continue;
            }

            int CheckX = _node.gridX + x;

            if (CheckX >= 0 && CheckX < _width)
            {
                Neighbours.Add(Grid[CheckX, _node.gridY]);
            }
        }

        return Neighbours;
    }

    private List<Node> GetNeighbouringNodes(Node _node, int _width, int _heigth)
    {
        List<Node> Neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int CheckX = _node.gridX + x;
                int CheckY = _node.gridY + y;

                if (CheckX >= 0 && CheckX < _width && CheckY >= 0 && CheckY < _heigth)
                {
                    Neighbours.Add(Grid[CheckX, CheckY]);
                }
            }
        }

        return Neighbours;
    }

    public void GetVectorField(Node _DistinationNode, int _mapWidth, int _mapHeight)
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                if (Grid[x, y].Walkable && Grid[x, y] != _DistinationNode)
                {
                    Vector3 CheapsetNode = GetCheapestNodeDirection(Grid[x, y], _mapWidth, _mapHeight);
                    Grid[x, y].dirVector.x = CheapsetNode.x;
                    Grid[x, y].dirVector.z = CheapsetNode.z;
                }
                else
                {
                    Grid[x, y].dirVector = Vector2.zero;
                }
            }
        }
    }

    private Vector3 GetCheapestNodeDirection(Node _node, int _width, int _heigth)
    {
        Node CheapestNode = new Node();
        CheapestNode.pathDistance = 2000;
        Vector3 ReturnVector = new Vector3();
        List<Node> NeighbouringNode = GetNeighbouringNodes(_node, _width, _heigth);

        foreach (Node _temp in NeighbouringNode)
        {
            if (_temp.pathDistance <= CheapestNode.pathDistance)
            {
                CheapestNode = _temp;
            }
        }

        ReturnVector = new Vector3(CheapestNode.gridX - _node.gridX, 0, CheapestNode.gridY - _node.gridY);

        return ReturnVector;
    }

    void DrawCostMap()
    {
        foreach (Node n in Grid)
        {
            GameObject GO = Instantiate(Text, n.WorldPos, Text.transform.rotation);
            GO.GetComponent<TextMesh>().text = n.pathDistance.ToString();
        }
    }

    void OnDrawGizmos()
    {
        if (Grid != null)
        {
            foreach (Node n in Grid)
            {
                Gizmos.color = Color.cyan;
                if (n != null)
                {
                    if (!n.Placeable)
                    {
                        Gizmos.color = Color.red;
                    }

                    if(n.Walkable && !n.Placeable)
                    {
                        Gizmos.color = Color.green;
                    }

                    if(n.Walkable && n.Placeable)
                    {
                        Gizmos.color = Color.magenta;
                    }
                    Gizmos.DrawCube(n.WorldPos, new Vector3(0.8f, 0.2f, 0.8f));
                }
            }
        }
    }
}
