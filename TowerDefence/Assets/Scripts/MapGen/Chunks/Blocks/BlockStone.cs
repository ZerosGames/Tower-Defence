﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStone : Block {

    public BlockStone(): base()
    {

    }

    //public override Tile TexturePosition(Direction direction)
    //{
    //    Tile tile = new Tile();
    //    switch (direction)
    //    {
    //        case Direction.up:
    //            tile.x = 2;
    //            tile.y = 0;
    //            return tile;
    //        case Direction.down:
    //            tile.x = 1;
    //            tile.y = 0;
    //            return tile;
    //    }
    //    tile.x = 3;
    //    tile.y = 0;
    //    return tile;
    //}

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 2;
        tile.y = 0;
        return tile;
    }
}