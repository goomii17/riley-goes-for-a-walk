using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Coordinates of the cell
    public int x;
    public int y;

    // Base of the cell
    public Tile tile;

    // Content of the cell
    public Entity content;

    // Neighbors of the cell
    public Cell[] neighbors;

    void OnMouseDown()
    {
        var disNuts = this;
        GameManager.Instance.OnCellClicked(disNuts);
    }

    public void SetContent(Entity entity)
    {
        // Set the content of the cell
        content = entity;
        // Set parent of GO
        entity.GO.transform.SetParent(transform, false);
        entity.CurrentCell = this;
    }

    public void SetTile(Tile tile)
    {
        // Set the tile of the cell
        this.tile = tile;
        // Set parent of GO
        tile.GO.transform.SetParent(transform, false);
    }

    public void FocusCell()
    {
        // Highlight the cell or change sprite in content
        tile.HighlightTile();
    }

    public void UnfocusCell()
    {
        // Unhighlight the cell or change sprite in content
        tile.UnHighlightTile();
    }

    public void HighlightCell()
    {
        // Highlight the cell or change sprite in content
        tile.HighlightTile();
    }

    public void UnHighlightCell()
    {
        // Unhighlight the cell or change sprite in content
        tile.UnHighlightTile();
    }

}
