using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy,
    Elevator,
}

public abstract class Entity
{
    public GameObject GO { get; set; }
    public EntityType EntityType { get; set; }

    public Cell CurrentCell { get; set; }

    public Cell NextMoveCell { get; set; }

    public Entity(GameObject gameObject)
    {
        GO = gameObject;
    }

    public abstract MoveOutcome UpdateNextMove(Cell cell);
    public abstract bool MakeMove();

    public abstract List<Cell> GetAttackableCells();

    public List<Cell> GetMoveableCells()
    {
        // Return the neighboring cells that are not occupied by another entity
        List<Cell> moveableCells = new List<Cell>();

        foreach (Cell cell in CurrentCell.neighbors)
        {
            if (cell == null || cell.tile.TileType == TileType.Void)
            {
                continue;
            }
            if (cell.content == null)
            {
                moveableCells.Add(cell);
            }
        }

        return moveableCells;
    }

    public void AutoDestroy()
    {
        Debug.Log("AutoDestroy entity");
        Object.Destroy(GO);
        GO = null;
    }

}