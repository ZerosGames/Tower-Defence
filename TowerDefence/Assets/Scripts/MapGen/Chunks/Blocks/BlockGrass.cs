using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrass : Block {

    public BlockGrass(): base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        switch (direction)
        {
            case Direction.up:
                tile.x = 0;
                tile.y = 0;
                return tile;
        }
        tile.x = 1;
        tile.y = 0;
        return tile;
    }
}
