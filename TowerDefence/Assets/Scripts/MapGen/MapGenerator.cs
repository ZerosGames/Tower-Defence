using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Node[,] grid;

    [SerializeField]
    public byte[,] mapData;

    [Header("MapGen")]
    public int MapHeight;
    public int MapWidth;

    public int GridSizeX;
    public int GridSizeY;

    public float nodeDiameter;
    public float nodeRadius;

    [Header("PathGen")]

    private Node startNode = new Node();
    private Node endNode = new Node();

    private int SXIndex, SYIndex;
    private int EXIndex, EYIndex;

    [SerializeField]
    public List<Node> Path;

    [Header("MeshGen")]
    [SerializeField]
    MeshFilter mf;

    [SerializeField]
    MeshCollider mc;

    [Header("Load From Texture")]
    public bool loadFromTexture = false;

    public Dictionary<int, Texture2D> Maps = new Dictionary<int, Texture2D>();

    [Header("Save To Texture")]
    public string savedPNGName = "";
 
    Mesh mesh;

    private List<Vector3> verts = new List<Vector3>();
    protected List<Vector2> uvs = new List<Vector2>();
    protected List<int> tris = new List<int>();

	void Start ()
    {
        InitData();
    }

    /// <summary>
    /// Init all the Map gen data, not done in the start method. For loading from another script later. 
    /// </summary>
    public void InitData()
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        ResetData();

        mesh = new Mesh();

        nodeDiameter = nodeRadius * 2;
        GridSizeX = Mathf.RoundToInt(MapWidth / nodeDiameter);
        GridSizeY = Mathf.RoundToInt(MapHeight / nodeDiameter);   

        if (!loadFromTexture)
        {
            mapData = new byte[GridSizeX, GridSizeY];

            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    mapData[x, y] = 1;
                }
            }

            mapData = MazeGenerator.CreateMaze(mapData, GridSizeX, GridSizeY, Vector2.one);

            CreateStartAndEndNodes();

            grid = NodeGrid.NodeGridFromByteArray(transform.position, MapWidth, MapHeight, nodeRadius, mapData);

            AssignStartEndNodes();

            GenerateFlowFeild();

            Path = RetracePath();

            ModifyMap(mapData, grid);
        }
        else
        {
            mapData = LoadFromTexture(2);

            nodeDiameter = nodeRadius * 2;

            MapWidth = mapData.GetLength(0) * (int)nodeDiameter;
            MapHeight = mapData.GetLength(1) * (int)nodeDiameter;

            grid = NodeGrid.NodeGridFromByteArray(transform.position, MapWidth, MapHeight, nodeRadius, mapData);
        }

        GenerateMapMesh();

        sw.Stop();
        print(sw.ElapsedMilliseconds + " ms");
    }

    /// <summary>
    /// Resets all the valves to null for redrawing mesh
    /// </summary>
    void ResetData()
    {
        mesh = null;
        grid = null;
        mapData = null;

        mf.mesh = null;
        mc.sharedMesh = null;

        verts.Clear();
        uvs.Clear();
        tris.Clear();
    }
	
    /// <summary>
    /// Generates a blocky mesh base on the MapGeneration byte array; 
    /// </summary>
    void GenerateMapMesh()
    {
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int z = 0; z < GridSizeY; z++)
            {
                if (mapData[x, z] == 0) continue;

                Vector3 pos = new Vector3(grid[x,z].WorldPos.x - nodeRadius, 0, grid[x, z].WorldPos.z - nodeRadius);

                //Top Face;
                    CreateTile(pos, Vector3.forward * nodeDiameter, Vector3.right * nodeDiameter, Vector3.up * nodeRadius, true, verts, tris, uvs);

                //Left Face;
                if (isTransparent(x - 1, z))
                    CreateTile(pos, Vector3.up * nodeDiameter, Vector3.forward * nodeDiameter, Vector3.down * nodeRadius, false, verts, tris, uvs);

                //right Face;
                if (isTransparent(x + 1, z))
                    CreateTile(pos, Vector3.up * nodeDiameter, Vector3.forward * nodeDiameter, Vector3.right * nodeRadius * 2 + Vector3.down * nodeRadius, true, verts, tris, uvs);

                //Back Face;
                if (isTransparent(x, z + 1))
                    CreateTile(pos, Vector3.up * nodeDiameter, Vector3.right * nodeDiameter, Vector3.down * nodeRadius + Vector3.forward * nodeDiameter, false, verts, tris, uvs);

                //Front Face;
                if (isTransparent(x, z - 1))
                    CreateTile(pos, Vector3.up * nodeDiameter, Vector3.right * nodeDiameter, Vector3.down * nodeRadius, true, verts, tris, uvs);
            }
        }

        mesh.vertices = verts.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.RecalculateNormals();
        mf.mesh = mesh;

        mesh.RecalculateBounds();
        mc.sharedMesh = mesh;
        
    }

    /// <summary>
    /// Creates a single quad for the mesh
    /// </summary>
    void CreateTile(Vector3 _corner, Vector3 _up, Vector3 _right, Vector3 offset, bool reversed, List<Vector3> _verts, List<int> _tris, List<Vector2> _uvs)
    {
        int index = _verts.Count;

        _verts.Add(_corner + offset);
        _verts.Add(_corner + _up + offset);
        _verts.Add(_corner + _up + _right + offset);
        _verts.Add(_corner + _right + offset);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));

        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 0);
        }
        else
        {
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 2);
            tris.Add(index + 0);
        }

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
    void CreateStartAndEndNodes()
    {
        bool noPath = false;

        int NESW = Random.Range(0, 3);
        int randX = Random.Range(1, GridSizeX - 2);
        int randY = Random.Range(1, GridSizeY - 2);

        if (randY != 0 || randX != 0 || randY != GridSizeY - 1 || randX != GridSizeX - 1)
        {

            switch (NESW)
            {
                //top
                case 0:
                    if (mapData[randX, GridSizeY - 2] == 1 || mapData[GridSizeX - 2, randY] == 1)
                    {
                        noPath = true;
                        break;
                    }
      
                    mapData[randX, GridSizeY - 1] = 0;
                    mapData[GridSizeX - 1, randY] = 0;

                    SXIndex = randX;
                    SYIndex = GridSizeY - 1;

                    EXIndex = GridSizeX - 1;
                    EYIndex = randY;

                    break;
                //Bottom
                case 1:

                    if (mapData[randX, 1] == 1 || mapData[1, randY] == 1)
                    {
                        noPath = true;
                        break;
                    }

                    mapData[randX, 0] = 0;
                    mapData[0, randY] = 0;

                    SXIndex = randX;
                    SYIndex = 0;

                    EXIndex = 0;
                    EYIndex = randY;
                    break;
                //Left
                case 2:
                    if (mapData[1, randY] == 1 || mapData[randX, 1] == 1)
                    {
                        noPath = true;
                        break;
                    }

                    mapData[0, randY] = 0;
                    mapData[randX, 0] = 0;

                    SXIndex = 0;
                    SYIndex = randY;

                    EXIndex = randX;
                    EYIndex = 0;
                    break;
                //right        
                case 3:
                    if (mapData[GridSizeX - 2, randY] == 1 || mapData[randX, GridSizeY - 2] == 1)
                    {
                        noPath = true;
                        break;
                    }

                    mapData[GridSizeX - 1, randY] = 0;
                    mapData[randX, GridSizeY - 1] = 0;

                    SXIndex = GridSizeX - 1;
                    SYIndex = randY;

                    EXIndex = randX;
                    EYIndex = GridSizeY - 1;
                    break;
            }
        }
        else
        {
            CreateStartAndEndNodes();
        }

        if (noPath)
        {
            CreateStartAndEndNodes();
        }
    }

    /// <summary>
    /// Sets the node for the start and end byte.
    /// </summary>
    void AssignStartEndNodes()
    {
        startNode = NodeGrid.GetNodeFromByteArray(grid, SXIndex, SYIndex);
        endNode = NodeGrid.GetNodeFromByteArray(grid, EXIndex, EYIndex);
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

            List<Node> NeighbouringNodes = GetNSEWNeighbouringNodes(Current, GridSizeX, GridSizeY);

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

    public void SaveToTexture(byte [,] _mapData, string _textureName, int _gridSizeX, int _gridSizeY)
    {
        Texture2D Tex = new Texture2D(GridSizeX,GridSizeY,TextureFormat.RGB24, false);

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                if (_mapData[x, y] == 1)
                {
                    Tex.SetPixel(x, y, Color.black);
                }
                else
                {
                    Tex.SetPixel(x, y, Color.white);
                }
            }
        }

        byte[] Texture = Tex.EncodeToPNG();
        FileStream file = File.Open(Application.dataPath + "/" + "Maps" + "/" + _textureName, FileMode.CreateNew);
        BinaryWriter binary = new BinaryWriter(file);
        binary.Write(Texture);
        file.Close();
        DestroyImmediate(Tex);
    }

    public void LoadAllTextures()
    {
        int i = 0;
        Maps.Clear();

        foreach (Texture2D tex in Resources.LoadAll<Texture2D>("Maps"))
        {
            Maps.Add(i, tex);
            i++;
        }

        i = 0;
    }

    byte[,] LoadFromTexture(int _texIndex)
    {  
        Texture2D tex = Maps[_texIndex];
        tex.mipMapBias = 0;
        byte[,] _mapData = new byte[tex.width, tex.height];
        byte[] _textureData = tex.GetRawTextureData();
        Color _color = new Color();


        GridSizeX = tex.width;
        GridSizeY = tex.height;


        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                _color = tex.GetPixel(x, y);

                if (_color == Color.black)
                {
                    _mapData[x, y] = 1;
                }
                else
                {
                    _mapData[x, y] = 0;
                }
            }
        }

        return _mapData;
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
