using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveOutcome
{
    Fail,
    Moved,
    MovedAndAttacked,
}

public class Player : Entity
{
    public Player(GameObject gameObject) : base(gameObject)
    {
        EntityType = EntityType.Player;
    }

    public override MoveOutcome UpdateNextMove(Cell cell)
    {
        List<Cell> moveableCells = GetMoveableCells();
        if (moveableCells.Contains(cell))
        {
            NextMove = cell;
            return MoveOutcome.Moved;
        }
        return MoveOutcome.Fail;
    }

    public override List<Cell> GetAttackableCells()
    {
        return new List<Cell>();
    }

}
