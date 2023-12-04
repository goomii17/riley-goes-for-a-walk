using UnityEngine;

public enum TileType
{
    Floor,
    Void,
}

public abstract class Tile
{
    public GameObject GO { get; set; }
    public TileType TileType { get; set; }

    public Tile(GameObject gameObject)
    {
        GO = gameObject;
    }

    public abstract void HighlightTile();
    public abstract void UnHighlightTile();

    public void AutoDestroy()
    {
        Object.Destroy(GO);
        GO = null;
    }

}