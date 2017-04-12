using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
    MeshFilter filter;
    MeshCollider coll;
    MeshRenderer renderer;

    public Material tiles;

    public static int chunkSize = 16;
    public bool update = true;

    public World world;
    public WorldPos pos;

    public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];

    //Use this for initialization
    void Start()
    {
        filter = GetComponent<MeshFilter>();
        coll = GetComponent<MeshCollider>();
        renderer = GetComponent<MeshRenderer>();
    }
    //Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
    }

    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    public bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }

    //Updates the chunk based on its contents
    void UpdateChunk()
    {
        MeshData meshData = new MeshData();

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }
        RenderMesh(meshData);
    }

    //Sends the calculated mesh information
    //to the mesh and collision components
    void RenderMesh(MeshData meshData)
    {
        Mesh _mesh = new Mesh();
        _mesh.vertices = meshData.vertices.ToArray();
        _mesh.triangles = meshData.triangles.ToArray();
        _mesh.uv = meshData.uv.ToArray();
        _mesh.RecalculateNormals();

        filter.mesh.Clear();
        filter.mesh.vertices = _mesh.vertices;
        filter.mesh.triangles = _mesh.triangles;
        filter.mesh.uv = _mesh.uv;
        filter.mesh.RecalculateNormals();

        renderer.sharedMaterial = tiles;

        coll.sharedMesh = _mesh;
    }
}