using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPath : Block {

    public BlockPath(): base()
    {

    }

    public override MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;

        meshData = FaceDataUp(chunk, x, y, z, meshData);

        return meshData;
    }

    public override bool IsSolid(Direction direction)
    {
        return false;
    }

    protected override MeshData FaceDataUp(Chunk chunk, float x, float y, float z, MeshData meshData)
    {
        return base.FaceDataUp(chunk, x, y - 1, z, meshData);
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 1;
        return tile;
    }
}
