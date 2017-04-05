using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlacementMesh : MonoBehaviour {

    private byte[,] meshData;

    [SerializeField]
    private PlacementGrid grid;

    [SerializeField]
    private MeshFilter mf;

    private Mesh mesh;
    protected List<Vector3> verts = new List<Vector3>();
    protected List<Vector2> uvs = new List<Vector2>();
    protected List<int> tris = new List<int>();

    private int mapHeight;
    private int mapWidth;

    public bool UpdatePlacementMesh = false;

    // Use this for initialization
    public void GenerateMesh (byte [,] _meshData)
    {
        mesh = new Mesh();
        mapHeight = GameManager.gameManager.GetMapHeight();
        mapWidth = GameManager.gameManager.GetMapWidth();

        //meshData = _meshData;

        //for (int x = 0; x < mapWidth/2; x++)
        //{
        //    for (int z = 0; z < mapHeight/2; z++)
        //    {
        //        meshData[x, z] = 1;
        //    }
        //}

        CreateMesh(_meshData);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(UpdatePlacementMesh)
        {
            
        }
	}

    void CreateMesh(byte[,] _meshData)
    {
        for (int x = 0; x < mapWidth/2; x++)
        {
            for (int z = 0; z < mapHeight/2; z++)
            { 
                if (_meshData[x, z] == 0) continue;

                CreateTile(new Vector3(x * 2 - mapWidth / 2, 1 , z * 2 - mapHeight / 2), Vector3.forward * 2, Vector3.right * 2, verts, tris, uvs);
            }
        }

        mesh.vertices = verts.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        mf.mesh = mesh;
    }

    void CreateTile(Vector3 _corner, Vector3 _up, Vector3 _right, List<Vector3> _verts, List<int> _tris, List<Vector2> _uvs)
    {
        int index = _verts.Count;

        _verts.Add(_corner);
        _verts.Add(_corner + _up);
        _verts.Add(_corner + _up + _right);
        _verts.Add(_corner + _right);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));

        tris.Add(index + 0);
        tris.Add(index + 1);
        tris.Add(index + 2);
        tris.Add(index + 2);
        tris.Add(index + 3);
        tris.Add(index + 0);
    }

    void SetupMeshData()
    {
        for (int x = 0; x < mapWidth/2; x++)
        {
            for (int y = 0; y < mapHeight/2; y++)
            {
                if (grid.Grid[x, y].Placeable == true)
                    meshData[x, y] = 1;
                else
                    meshData[x, y] = 0;
            }
        }
    }
}
