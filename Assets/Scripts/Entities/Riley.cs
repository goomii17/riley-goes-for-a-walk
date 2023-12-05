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

    public Cell killMeleeLeft;
    public Cell killMeleeRight;
    public Cell killFrontal;

    public Player(GameObject gameObject) : base(gameObject)
    {
        EntityType = EntityType.Player;
    }

    public override MoveOutcome UpdateNextMove(Cell cell)
    {
        List<Cell> moveableCells = GetMoveableCells();
        if (moveableCells.Contains(cell))
        {
            NextMoveCell = cell;
            UpdateKills(cell);
            return MoveOutcome.Moved;
        }
        return MoveOutcome.Fail;
    }

    public void UpdateKills(Cell nextCell)
    {
        killMeleeLeft = null;
        killMeleeRight = null;
        killFrontal = null;

        var dx = nextCell.x - CurrentCell.x;
        var dy = nextCell.y - CurrentCell.y;

        if (dy > 0)
        {
            switch (dx)
            {
                case 0:
                    // Left 5, Right is 1, frontal is 0.0
                    killMeleeLeft = ValidMeleeCell(CurrentCell.neighbors[5]) ? CurrentCell.neighbors[5] : null;
                    killMeleeRight = ValidMeleeCell(CurrentCell.neighbors[1]) ? CurrentCell.neighbors[1] : null;
                    killFrontal = ValidFrontalCell(nextCell, 0) ? nextCell.neighbors[0] : null;
                    break;
                case 1:
                    // Left 0, Right is 2, frontal is 1.1
                    killMeleeLeft = ValidMeleeCell(CurrentCell.neighbors[0]) ? CurrentCell.neighbors[0] : null;
                    killMeleeRight = ValidMeleeCell(CurrentCell.neighbors[2]) ? CurrentCell.neighbors[2] : null;
                    killFrontal = ValidFrontalCell(nextCell, 1) ? nextCell.neighbors[1] : null;
                    break;
            }
        }
        else if (dy == 0)
        {
            switch (dx)
            {
                case -1:
                    // Left 4, Right is 0, frontal is 5.5
                    killMeleeLeft = ValidMeleeCell(CurrentCell.neighbors[4]) ? CurrentCell.neighbors[4] : null;
                    killMeleeRight = ValidMeleeCell(CurrentCell.neighbors[0]) ? CurrentCell.neighbors[0] : null;
                    killFrontal = ValidFrontalCell(nextCell, 5) ? nextCell.neighbors[5] : null;
                    break;
                case 1:
                    // Left 1, Right is 3, frontal is 2.2
                    killMeleeLeft = ValidMeleeCell(CurrentCell.neighbors[1]) ? CurrentCell.neighbors[1] : null;
                    killMeleeRight = ValidMeleeCell(CurrentCell.neighbors[3]) ? CurrentCell.neighbors[3] : null;
                    killFrontal = ValidFrontalCell(nextCell, 2) ? nextCell.neighbors[2] : null;
                    break;
            }
        }
        else if (dy < 0)
        {
            switch (dx)
            {
                case 0:
                    // Left 1, Right is 4, frontal is 3.3
                    killMeleeLeft = ValidMeleeCell(CurrentCell.neighbors[1]) ? CurrentCell.neighbors[1] : null;
                    killMeleeRight = ValidMeleeCell(CurrentCell.neighbors[4]) ? CurrentCell.neighbors[4] : null;
                    killFrontal = ValidFrontalCell(nextCell, 3) ? nextCell.neighbors[3] : null;
                    break;
                case -1:
                    // Left 3, Right is 5, frontal is 4.4
                    killMeleeLeft = ValidMeleeCell(CurrentCell.neighbors[3]) ? CurrentCell.neighbors[3] : null;
                    killMeleeRight = ValidMeleeCell(CurrentCell.neighbors[5]) ? CurrentCell.neighbors[5] : null;
                    killFrontal = ValidFrontalCell(nextCell, 4) ? nextCell.neighbors[4] : null;
                    break;
            }
        }
    }

    public bool ValidMeleeCell(Cell cell)
    {
        return cell != null && cell.content != null && cell.content.EntityType == EntityType.Enemy;
    }

    public bool ValidFrontalCell(Cell cell, int neighborIndex)
    {
        bool validEmpty = cell != null && cell.content == null && cell.tile.TileType != TileType.Void;
        if (!validEmpty)
        {
            return false;
        }
        bool validEnemy = cell.neighbors[neighborIndex] != null && cell.neighbors[neighborIndex].content != null && cell.neighbors[neighborIndex].content.EntityType == EntityType.Enemy;
        return validEnemy;
    }

    public override bool MakeMove()
    {
        if (NextMoveCell == null)
        {
            return false;
        }

        // Move to next cell
        CurrentCell.UnSetContent();
        NextMoveCell.SetContent(this);
        NextMoveCell = null;

        // Kill enemies
        if (killMeleeLeft != null)
        {
            killMeleeLeft.content.AutoDestroy();
        }

        if (killMeleeRight != null)
        {
            killMeleeRight.content.AutoDestroy();
        }

        if (killFrontal != null)
        {
            killFrontal.content.AutoDestroy();
        }

        return true;
    }

    public override List<Cell> GetAttackableCells()
    {
        return new List<Cell>();
    }

}
