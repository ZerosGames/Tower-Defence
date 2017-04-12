using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCyrstal : Block {

    public BlockCyrstal() : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 1;
                tile.y = 1;
                return tile;
        }
        tile.x = 1;
        tile.y = 0;
        return tile;
    }
}
