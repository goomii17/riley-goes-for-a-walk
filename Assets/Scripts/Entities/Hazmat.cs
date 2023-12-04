using System.Collections.Generic;
using UnityEngine;

public class Hazmat : Entity
{
    public Hazmat(GameObject gameObject) : base(gameObject)
    {
        EntityType = EntityType.Enemy;
    }

    public override MoveOutcome UpdateNextMove(Cell cell)
    {
        return MoveOutcome.Fail;
    }

    public override List<Cell> GetAttackableCells()
    {
        // Return list of neighboring cells since Hazmat is a melee unit
        List<Cell> attackableCells = new List<Cell>();

        foreach (Cell cell in CurrentCell.neighbors)
        {
            if (cell == null || cell.tile.TileType == TileType.Void)
            {
                continue;
            }
            if (cell.content == null || cell.content.EntityType == EntityType.Player)
            {
                attackableCells.Add(cell);
            }
        }

        return attackableCells;
    }

}
