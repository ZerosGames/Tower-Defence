using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Node[,] grid;

    [SerializeField]
    public byte[,] mapData;
    public byte[,] playableMapData;

    [Header("MapGen")]
    public int MapHeight = 64;
    public int MapWidth = 64;

    public int playableMapWidth = 22;
    public int PlayableMapHeight = 22;

    public int GridSizeX;
    public int GridSizeY;

    public float nodeDiameter;
    public float nodeRadius;

    [Header("PathGen")]
    public GameObject StartPos;
    public GameObject EndPos;

    private Node startNode = new Node();
    private Node endNode = new Node();

    private int SXIndex, SYIndex;
    private int EXIndex, EYIndex;

    [SerializeField]
    public List<Node> Path;

    [SerializeField]
    public GameObject waypoint;

    [SerializeField]
    private World world;

    [SerializeField]
    GameObject TestText;
    [SerializeField]
    GameObject StartPortal;
    [SerializeField]
    GameObject EndPortal;

    [Header("NoiseGen")]
    public float noiseScale;

    public int octaves;
    public float persistance;
    public float lacanaity;

    public int seed;
    public Vector2 Offset;

    [Header("Resoucre Gen")]
    [SerializeField]
    private int amountOfResoucreNodes;
    [SerializeField]
    private GameObject crystalPerfab;

    /// <summary>
    /// Init all the Map gen data, not done in the start method. For loading from another script later. 
    /// </summary>
    public void InitData()
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        ResetData();

        nodeDiameter = nodeRadius * 2;
        GridSizeX = Mathf.RoundToInt(MapWidth / nodeDiameter);
        GridSizeY = Mathf.RoundToInt(MapHeight / nodeDiameter);   

        mapData = new byte[GridSizeX, GridSizeY];
        playableMapData = new byte[playableMapWidth, PlayableMapHeight];

        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                mapData[x, y] = 1;
            }
        }

        for (int x = 0; x < playableMapWidth; x++)
        {
            for (int y = 0; y < PlayableMapHeight; y++)
            { 
                playableMapData[x, y] = 1;
            }
        }

        playableMapData = MazeGenerator.CreateMaze(playableMapData, playableMapWidth, PlayableMapHeight, Vector2.one);

        CreateStartAndEndNodes(playableMapData);

        for (int x = 0; x < playableMapWidth; x++)
        {
            for (int y = 0; y < PlayableMapHeight; y++)
            {                    
                mapData[x + playableMapWidth, y + PlayableMapHeight] = playableMapData[x, y];
            }
        }


        grid = NodeGrid.NodeGridFromByteArray(transform.position, mapData, MapWidth, MapHeight, nodeDiameter, nodeRadius);

        //print("NodeGrid: " + sw.ElapsedMilliseconds + " ms");

        AssginStartEndNodes();

        //print("StartEndPoints: " + sw.ElapsedMilliseconds + " ms");

        GenerateFlowFeild();

        //print("VectorFeild: " + sw.ElapsedMilliseconds + " ms");

        Path = RetracePath();

        //print("Path: " + sw.ElapsedMilliseconds + " ms");

        ModifyMap(mapData, grid);

        //print("Change Map Data: " + sw.ElapsedMilliseconds + " ms");

        SetupStartEnd();

        //print("Excavated Map: " + sw.ElapsedMilliseconds + " ms");

        GenerateFlowFeild(grid);

        //print("Flow Feild: " + sw.ElapsedMilliseconds + " ms");

        Resourses();

        //print("Spawned resoucre" + sw.ElapsedMilliseconds + "ms");

        world.UpdateChucks();

        UpdateChunkData();
        
        sw.Stop();
        print("Entire Gen: " + sw.ElapsedMilliseconds + " ms");
    }

    /// <summary>
    /// Resets all the valves to null for redrawing mesh
    /// </summary>
    void ResetData()
    {
        grid = null;
        mapData = null;
    }

    /// <summary>
    /// Checks if the the byte array is to see if the byte at x/y is transparent
    /// </summary>
    bool isTransparent(int x, int y)
    {
        byte block = GetByte(x, y);

        switch (block)
        {
            case 0: return true;
            case 1: return false;
        }

        return false;
    }

    /// <summary>
    /// Returns a byte from the byte array at x/y;
    /// </summary>
    byte GetByte(int x, int y)
    {
        if(x < 0 || y < 0 || x >= GridSizeX || y >= GridSizeY)
            return 0;
        return mapData[x, y];
    }

    /// <summary>
    /// Randomly Places the start and end nodes.
    /// </summary>
    void CreateStartAndEndNodes(byte[,] _mapData)
    {
        bool noPath = false;

        int NESW = Random.Range(0, 3);
        int randX = Random.Range(1, playableMapWidth - 2);
        int randY = Random.Range(1, PlayableMapHeight - 2);

        if (randY != 0 || randX != 0 || randY != PlayableMapHeight - 1 || randX != playableMapWidth - 1)
        {

            switch (NESW)
            {
                //top
                case 0:
                    if (mapData[randX, PlayableMapHeight - 2] == 1 || mapData[playableMapWidth - randX, 0] == 1)
                    {
                        noPath = true;
                        break;
                    }

                    _mapData[randX, PlayableMapHeight - 1] = 0;
                    _mapData[playableMapWidth - randX, 0] = 0;

                    SXIndex = randX;
                    SYIndex = PlayableMapHeight - 1;

                    EXIndex = playableMapWidth - randX;
                    EYIndex = 0;

                    break;
                //Bottom
                case 1:

                    if (_mapData[randX, 1] == 1 || _mapData[playableMapWidth - randX, PlayableMapHeight - 2] == 1)
                    {
                        noPath = true;
                        break;
                    }

                    _mapData[randX, 0] = 0;
                    _mapData[playableMapWidth - randX, PlayableMapHeight - 2] = 0;

                    SXIndex = randX;
                    SYIndex = 0;

                    EXIndex = playableMapWidth - randX;
                    EYIndex = PlayableMapHeight - 2;
                    break;
                //Left
                case 2:
                    if (_mapData[1, randY] == 1 || _mapData[playableMapWidth - 2, PlayableMapHeight - randY] == 1)
                    {
                        noPath = true;
                        break;
                    }

                    _mapData[0, randY] = 0;
                    _mapData[playableMapWidth - 2, PlayableMapHeight - randY] = 0;

                    SXIndex = 0;
                    SYIndex = randY;

                    EXIndex = playableMapWidth - 2;
                    EYIndex = PlayableMapHeight - randY;
                    break;
                //right        
                case 3:
                    if (_mapData[playableMapWidth - 2, randY] == 1 || _mapData[1, randY] == 1)
                    {
                        noPath = true;
                        break;
                    }

                    _mapData[playableMapWidth - 1, randY] = 0;
                    _mapData[1, PlayableMapHeight - randY] = 0;

                    SXIndex = playableMapWidth - 1;
                    SYIndex = randY;

                    EXIndex = 1;
                    EYIndex = PlayableMapHeight - randY;
                    break;
            }
        }
        else
        {
            CreateStartAndEndNodes(_mapData);
            return;
        }

        if (noPath)
        {
            CreateStartAndEndNodes(_mapData);
            return;
        }
    }

    void AssginStartEndNodes()
    {
        startNode = NodeGrid.GetNodeFromByteArray(grid, SXIndex + playableMapWidth, SYIndex + PlayableMapHeight);
        endNode = NodeGrid.GetNodeFromByteArray(grid, EXIndex + playableMapWidth, EYIndex + PlayableMapHeight);
    }

    void SetupStartEnd()
    {
        if(startNode != null && endNode != null)
        {
            List<Node> OpenSet = new List<Node>();
            OpenSet.Add(startNode);
            OpenSet.Add(endNode);

            foreach (Node n in GetNeighbouringNodes(startNode, GridSizeX, GridSizeY))
            {
                OpenSet.Add(n);
            }

            foreach (Node n in GetNeighbouringNodes(endNode, GridSizeX, GridSizeY))
            {
                OpenSet.Add(n);
            }

            foreach (Node n in OpenSet)
            {
                n.Placeable = false;
                n.Walkable = true;
            }

            Vector3 startPortal = new Vector3(startNode.WorldPos.x, -0.5f, startNode.WorldPos.z);
            Vector3 endPortal = new Vector3(endNode.WorldPos.x, -0.5f, endNode.WorldPos.z);

            Quaternion q = new Quaternion();

            if(Path[1].gridX > startNode.gridX || Path[1].gridX < startNode.gridX)
            {
                q = Quaternion.AngleAxis(90, Vector3.up);
            }

            if(Path[1].gridY > startNode.gridY || Path[1].gridY < startNode.gridY)
            {
                q = Quaternion.AngleAxis(0, Vector3.up);
            }

            Instantiate(StartPortal, startPortal, q);

            if (Path[Path.Count - 2].gridX > endNode.gridX || Path[Path.Count - 2].gridX < endNode.gridX)
            {
                q = Quaternion.AngleAxis(90, Vector3.up);
            }

            if (Path[Path.Count - 2].gridY > endNode.gridY || Path[Path.Count - 2].gridY < endNode.gridY)
            {
                q = Quaternion.AngleAxis(0, Vector3.up);
            }

            Instantiate(EndPortal, endPortal, q);
        }
    }

    /// <summary>
    /// Generate The flow feild for pathing generating
    /// </summary>
    void GenerateFlowFeild()
    {
        Queue<Node> Frontier = new Queue<Node>();
        List<Node> Marked = new List<Node>();

        Frontier.Enqueue(startNode);
        Marked.Add(startNode);

        while (Frontier.Count > 0)
        {
            Node Current = Frontier.Dequeue();

            List<Node> NeighbouringNodes = GetNSEWNeighbouringNodes(Current, MapWidth, MapHeight);

            foreach (Node _node in NeighbouringNodes)
            {
                if (Marked.Contains(_node) || _node.Walkable == false)
                {
                    if (_node.Walkable == false || _node.Placeable)
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

    void GenerateFlowFeild(Node[,] _grid)
    {
        Queue<Node> Frontier = new Queue<Node>();
        List<Node> Marked = new List<Node>();

        foreach (Node n in _grid)
        {
            n.pathDistance = 0;

            if (n.Walkable)
            {
                Frontier.Enqueue(n);
            }
        }

        while (Frontier.Count > 0)
        {
            Node Current = Frontier.Dequeue();
            Marked.Add(Current);

            List<Node> NeighbouringNodes = GetNSEWNeighbouringNodes(Current, GridSizeX, GridSizeY);

            foreach (Node _node in NeighbouringNodes)
            {
                if (Marked.Contains(_node) || _node.Walkable)
                {
                    if (_node.Placeable == false)
                    {
                        _node.pathDistance = 0;
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

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap2 = PerlinNoise.GenerateNoiseMap(MapWidth, MapHeight, seed, noiseScale, octaves, persistance, lacanaity, Offset);

        float[,] noiseMap = new float[MapWidth, MapHeight];

        int maxNoiseHeight = int.MinValue;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].pathDistance > maxNoiseHeight)
                {
                    maxNoiseHeight = grid[x, y].pathDistance;
                }
            }
        }

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                noiseMap[x, y] = (float)grid[x, y].pathDistance / (float)maxNoiseHeight;
                noiseMap[x, y] *= noiseMap2[x, y];
            }
        }

        return noiseMap;
    }

    /// <summary>
    /// retrace the path from the end node to the start node then reverse the array to give the correct path.
    /// </summary>
    List<Node> RetracePath()
    {
        List<Node> _path = new List<Node>();
        Node node = endNode;

        while (node != startNode)
        {
            _path.Add(node);
            node = node.parent;
        }

        _path.Add(startNode);

        _path.Reverse();

        return _path;
    }

    /// <summary>
    /// Get the Nieghbouring Nodes of a node North, east, south, west.
    /// </summary>
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
                Neighbours.Add(grid[_node.gridX, CheckY]);
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
                Neighbours.Add(grid[CheckX, _node.gridY]);
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
                    Neighbours.Add(grid[CheckX, CheckY]);
                }
            }
        }

        return Neighbours;
    }

    void UpdateChunkData() 
    {
        foreach (Node n in grid)
        {
            if (Path.Contains(n))
            {
                if (world.GetBlock((int)n.WorldPos.x, (int)n.WorldPos.y, (int)n.WorldPos.z).GetType() == typeof(BlockGrass))
                {
                    world.SetBlock((int)n.WorldPos.x, (int)n.WorldPos.y, (int)n.WorldPos.z, new BlockPath());
                }
            }
            else if (n.CrystalNode)
            {
                for (int y = 0; y < 32; y++)
                {
                    if (world.GetBlock((int)n.WorldPos.x, (int)n.WorldPos.y + y, (int)n.WorldPos.z).GetType() == typeof(BlockAir))
                    {
                        world.SetBlock((int)n.WorldPos.x, (int)n.WorldPos.y + y - 1, (int)n.WorldPos.z, new BlockCyrstal());
                        Instantiate(crystalPerfab, new Vector3(n.WorldPos.x, (n.WorldPos.y + y - 1) + 0.5f, n.WorldPos.z), Quaternion.identity);
                        break;
                    }
                }
            } 
        }
    }

    void ModifyMap(byte[,] _mapData, Node[,] _grid)
    {
        foreach (Node n in _grid)
        {
            if (Path.Contains(n))
            {
                continue;
            }

            mapData[n.gridX, n.gridY] = 1;
            n.Placeable = true;
            n.Walkable = false;
        }
    }

    void SpawnResoucres()
    {
        int randGridX = Random.Range(0, GridSizeX - 1);
        int randGridY = Random.Range(0, GridSizeY - 1);

        if (grid[randGridX, randGridY].Walkable == true || grid[randGridX, randGridY].pathDistance < 5)
        {
            SpawnResoucres();
            return;
        }

        grid[randGridX, randGridY].CrystalNode = true;
        grid[randGridX, randGridY].Placeable = false;
    }

    void Resourses()
    {
        for (int i = 0; i < amountOfResoucreNodes; i++)
        {
            SpawnResoucres();
        }
    }

    void OnDrawGizmos()
    { 
        Gizmos.DrawWireCube(transform.position, new Vector3(MapWidth, 0, MapHeight));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.cyan;
                if (n != null)
                {
                    if (Path == null || !Path.Contains(n))
                    {
                        if (!n.Placeable && !n.Walkable && !n.CrystalNode)
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
                        else if(n.CrystalNode)
                        {
                            Gizmos.color = Color.yellow;
                        }

                        Gizmos.DrawCube(n.WorldPos, new Vector3(nodeDiameter - 0.2f, nodeDiameter - 0.2f, nodeDiameter - 0.2f));
                    }
                    else
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(n.WorldPos, new Vector3(nodeDiameter - 0.2f, nodeDiameter - 0.2f, nodeDiameter - 0.2f));
                    }
                }
            }
        }

        if (startNode != null && endNode != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(startNode.WorldPos, 1);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(endNode.WorldPos, 1);
        }
    }
}
