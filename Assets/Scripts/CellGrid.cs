using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellGrid : MonoBehaviour
{
    const int MATRIX_WIDTH = 9;
    const int MATRIX_HEIGHT = 11;

    const int GRID_WIDTH = 5;
    const int GRID_HEIGHT = 7;

    const int CELL_WIDTH = 31;
    const int CELL_HEIGHT = 27;
    const float HEX_WIDTH = 16;

    // Cell prefab
    public GameObject cellPrefab;
    private GameObject[,] grid;

    public void Awake()
    {
        grid = new GameObject[MATRIX_HEIGHT, MATRIX_WIDTH];
        Debug.Log("CellGrid Awake", grid[0, 0]);

        InitBoard();
    }

    public static bool IsInGrid(int i, int j)
    {
        int start = Mathf.Max(i + 1 - GRID_HEIGHT, 0);
        int end = Mathf.Min(GRID_WIDTH + i, MATRIX_WIDTH);
        return i >= 0 && i < MATRIX_HEIGHT && j >= start && j < end;
    }

    private void InitBoard()
    {
        // Instantiate and initialize cells
        for (int i = 0; i < MATRIX_HEIGHT; i++)
        {
            int start = Mathf.Max(i + 1 - GRID_HEIGHT, 0);
            int end = Mathf.Min(GRID_WIDTH + i, MATRIX_WIDTH);
            for (int j = start; j < end; j++)
            {
                // Calculate the center of the hexagon
                float x = j * (CELL_WIDTH / 2 + HEX_WIDTH / 2);
                float y = (CELL_HEIGHT - 1) * i - j * (CELL_HEIGHT - 1) / 2;

                GameObject cellObject = Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity);
                cellObject.transform.parent = transform;
                cellObject.GetComponent<Cell>().x = j;
                cellObject.GetComponent<Cell>().y = i;

                grid[i, j] = cellObject;
            }
        }

        // Set the neighbors of each cell
        for (int i = 0; i < MATRIX_HEIGHT; i++)
        {
            int start = Mathf.Max(i + 1 - GRID_HEIGHT, 0);
            int end = Mathf.Min(GRID_WIDTH + i, MATRIX_WIDTH);
            for (int j = start; j < end; j++)
            {
                Cell cell = grid[i, j].GetComponent<Cell>();
                cell.neighbors = new Cell[6];
                cell.neighbors[0] = IsInGrid(i + 1, j) ? grid[i + 1, j].GetComponent<Cell>() : null;
                cell.neighbors[1] = IsInGrid(i + 1, j + 1) ? grid[i + 1, j + 1].GetComponent<Cell>() : null;
                cell.neighbors[2] = IsInGrid(i, j + 1) ? grid[i, j + 1].GetComponent<Cell>() : null;
                cell.neighbors[3] = IsInGrid(i - 1, j) ? grid[i - 1, j].GetComponent<Cell>() : null;
                cell.neighbors[4] = IsInGrid(i - 1, j - 1) ? grid[i - 1, j - 1].GetComponent<Cell>() : null;
                cell.neighbors[5] = IsInGrid(i, j - 1) ? grid[i, j - 1].GetComponent<Cell>() : null;
            }
        }
    }

    // Generator to return all cells that contain an enemy

}
