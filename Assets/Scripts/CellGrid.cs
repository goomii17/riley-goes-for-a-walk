using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    // Cell prefab
    public GameObject cellPrefab;
    public GameObject voidPrefab;
    public GameObject floorPrefab;
    public GameObject elevatorPrefab;
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;

    // Grid of cells
    private GameObject[,] grid;

    // Player and enemies
    public Player player;
    public List<Entity> enemies = new List<Entity>();

    public void Awake()
    {
        DestroyChildren();
        InitGrid();
        SetNeighbors();
        FillGrid();
    }

    public static bool IsInGrid(int i, int j)
    {
        int start = Mathf.Max(i + 1 - GameParams.GRID_HEIGHT, 0);
        int end = Mathf.Min(GameParams.GRID_WIDTH + i, GameParams.MATRIX_WIDTH);
        return i >= 0 && i < GameParams.MATRIX_HEIGHT && j >= start && j < end;
    }

    public void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Initialize grid, instantiate all cells and add them to the grid.
    /// </summary>
    private void InitGrid()
    {
        grid = new GameObject[GameParams.MATRIX_HEIGHT, GameParams.MATRIX_WIDTH];
        // Instantiate and initialize cells
        for (int i = 0; i < GameParams.MATRIX_HEIGHT; i++)
        {
            int start = Mathf.Max(i + 1 - GameParams.GRID_HEIGHT, 0);
            int end = Mathf.Min(GameParams.GRID_WIDTH + i, GameParams.MATRIX_WIDTH);
            for (int j = start; j < end; j++)
            {
                // Calculate the center of the hexagon
                float x = j * (GameParams.CELL_WIDTH / 2 + GameParams.HEX_SIZE / 2);
                float y = (GameParams.CELL_HEIGHT - 1) * i - j * (GameParams.CELL_HEIGHT - 1) / 2;

                GameObject cellObject = Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity);
                cellObject.transform.parent = transform;
                cellObject.GetComponent<Cell>().x = j;
                cellObject.GetComponent<Cell>().y = i;

                grid[i, j] = cellObject;
            }
        }

    }

    /// <summary>
    /// Sets the neighbors of each cell.
    /// </summary>
    public void SetNeighbors()
    {
        // Set the neighbors of each cell
        for (int i = 0; i < GameParams.MATRIX_HEIGHT; i++)
        {
            int start = Mathf.Max(i + 1 - GameParams.GRID_HEIGHT, 0);
            int end = Mathf.Min(GameParams.GRID_WIDTH + i, GameParams.MATRIX_WIDTH);
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
        // foreach (Cell neighbor in grid[1, 4].GetComponent<Cell>().neighbors)
        // {
        //     Debug.Log("Coordinate: " + neighbor.x + ", " + neighbor.y);
        // }
    }

    /// <summary>
    /// Fills the grid with player, enemies, elevator, floor and void.
    /// </summary>
    /// <param name="level"></param>
    public void FillGrid(int level = 1)
    {
        // Put player on the grid
        GameObject playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Cell playerCell = grid[1, 4].GetComponent<Cell>();
        playerCell.SetContent(new Player(playerObject));
        player = playerCell.content as Player;

        // List of banned cells for placing enemies
        List<Cell> bannedCells = new List<Cell>(){
            playerCell,
            grid[0, 3].GetComponent<Cell>(),
            grid[0, 4].GetComponent<Cell>(),
            grid[1, 5].GetComponent<Cell>(),
            grid[2, 5].GetComponent<Cell>(),
            grid[3, 4].GetComponent<Cell>(),
            grid[2, 3].GetComponent<Cell>(),
        };


        for (int i = 0; i < GameParams.MATRIX_HEIGHT; i++)
        {
            int start = Mathf.Max(i + 1 - GameParams.GRID_HEIGHT, 0);
            int end = Mathf.Min(GameParams.GRID_WIDTH + i, GameParams.MATRIX_WIDTH);
            for (int j = start; j < end; j++)
            {
                Cell cell = grid[i, j].GetComponent<Cell>();
                // 90% chance of floor, 10% chance of void
                if (Random.Range(0, 10) < 9)
                {
                    GameObject floorObject = Instantiate(floorPrefab, Vector3.zero, Quaternion.identity);
                    cell.SetTile(new LabFloor(floorObject));

                    // 20% chance of enemy
                    if (Random.Range(0, 100) < 5 && !bannedCells.Contains(cell))
                    {
                        var enemyIndex = Random.Range(0, enemyPrefabs.Length);
                        GameObject enemyObject = Instantiate(enemyPrefabs[enemyIndex], Vector3.zero, Quaternion.identity);
                        cell.SetContent(EnemyFactory.CreateEnemy(enemyIndex, enemyObject));
                        enemies.Add(cell.content);
                    }
                }
                else
                {
                    GameObject voidObject = Instantiate(voidPrefab, Vector3.zero, Quaternion.identity);
                    cell.SetTile(new Void(voidObject));
                }
            }
        }

    }

    /// <summary>
    /// Destroys all content and tiles of the grid if they exist.
    /// </summary>
    public void ResetGrid()
    {
        foreach (Transform child in transform)
        {
            Cell cell = child.GetComponent<Cell>();

            // Reset content and tile, destroy them if they exist
            cell.content?.AutoDestroy();
            cell.tile?.AutoDestroy();
            cell.content = null;
            cell.tile = null;
        }
        player = null;
        enemies.Clear();
    }

    /// <summary>
    /// Funcion que dada dos cells, devuelve lista de cells que forman el camino MÁS CORTO entre ellas o lista vacia si no hay camino
    /// </summary>
    /// <param name="startCell"></param>
    /// <param name="endCell"></param>
    public List<Cell> FindPath(Cell startCell, Cell endCell)
    {
        //// Content of the cell public Entity content; // Neighbors of the cell public Cell[] neighbors;
        // List of cells to visit
        List<Cell> openCells = new List<Cell>();
        foreach (Cell neighbor in startCell.neighbors)
        {
            if (neighbor != null && neighbor.content == null)
            {
                openCells.Add(neighbor);
            }
        }
        // List of visited cells
        List<Cell> closedCells = new List<Cell>();
        closedCells.Add(startCell);
        // Dictionary of parents
        Dictionary<Cell, Cell> parents = new Dictionary<Cell, Cell>();
        foreach (Cell cell in openCells)
        {
            parents.Add(cell, startCell);
        }

        while (openCells.Count > 0)
        {
            Cell currentCell = openCells[0];
            openCells.RemoveAt(0);
            closedCells.Add(currentCell);

            if (currentCell == endCell)
            {
                // Path found
                List<Cell> path = new List<Cell>();
                Cell current = endCell;
                while (current != startCell)
                {
                    path.Add(current);
                    current = parents[current];
                }
                path.Reverse();
                return path;
            }

            foreach (Cell neighbor in currentCell.neighbors)
            {
                if (neighbor != null && neighbor.content == null && !closedCells.Contains(neighbor) && !openCells.Contains(neighbor))
                {
                    if (neighbor.tile.TileType == TileType.Floor)
                    {
                        openCells.Add(neighbor);
                        parents.Add(neighbor, currentCell);

                    }
                }
            }
        }
        // No path found
        return new List<Cell>();
    }
}


