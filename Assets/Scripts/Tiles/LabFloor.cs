
using UnityEngine;

public class LabFloor : Tile
{
    private SpriteRenderer spriteRenderer;

    public LabFloor(GameObject gameObject) : base(gameObject)
    {
        spriteRenderer = GO.GetComponent<SpriteRenderer>();
    }

    public override void HighlightTile()
    {
        spriteRenderer.color = Color.yellow;
    }

    public override void UnHighlightTile()
    {
        spriteRenderer.color = Color.white;
    }


}
