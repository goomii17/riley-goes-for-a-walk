using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : Tile
{

    public Void(GameObject gameObject) : base(gameObject)
    {
        TileType = TileType.Void;
    }

    public override void HighlightTile()
    {
        // Do nothing
    }

    public override void UnHighlightTile()
    {
        // Do nothing
    }
}
