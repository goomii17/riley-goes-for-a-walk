using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    // Coordinates of the cell
    public int x;
    public int y;

    // Content of the cell
    public Entity content;

    // Neighbors of the cell
    public Cell[] neighbors;

    void OnMouseDown()
    {
        var disNuts = this;
        GameManager.instance.OnCellClicked(disNuts);
        Debug.Log("Cell clicked");
        Debug.Log("Coordinates: " + x + ", " + y);
        Debug.Log("Content: " + content);
        // Debug.Log("Neighbors: " + neighbors);
        // Debug all neighbors
        foreach (Cell neighbor in neighbors)
        {
            Debug.Log("Neighbor: " + neighbor);
        }
    }

    void FocusCell()
    {
        // Highlight the cell or change sprite in content
    }

}
