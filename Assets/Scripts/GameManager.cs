using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Menu,
    PlayerTurn,
    AnimateAndMovePlayer,
    AnimateAndAttackPlayer,
    EnemyTurn,
    AnimateAndMoveEnemies,
    GameOver
}

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    // Menu canvas
    public GameObject menuCanvas;

    public GameInfo gameInfo;
    public GameState gameState;

    public CellGrid cellGrid;

    Cell focusedCell;
    Cell targetCell;
    List<Cell> highlightedCells = new List<Cell>();

    public void OnEnable()
    {
        Instance = this;
    }

    public void Awake()
    {
        menuCanvas.SetActive(true);
        gameState = GameState.Menu;
    }

    public void Update()
    {
        switch (gameState)
        {
            case GameState.Menu:
                // Animations or whatever
                break;
            case GameState.PlayerTurn:
                // Play Idle animations
                AnimateIdleEntities();
                HandlePlayerInstruction();
                break;
            case GameState.AnimateAndMovePlayer:
                bool finished = cellGrid.player.MakeMove();
                if (finished)
                {
                    gameState = GameState.AnimateAndAttackPlayer;
                }
                break;
            case GameState.AnimateAndAttackPlayer:
                gameState = GameState.EnemyTurn;
                break;
            case GameState.EnemyTurn:
                // Shoot or hit the player and calculate where to move next
                gameState = GameState.AnimateAndMoveEnemies;
                break;
            case GameState.AnimateAndMoveEnemies:
                // Make the animation and make the move
                gameState = GameState.PlayerTurn;
                break;
            case GameState.GameOver:
                break;
        }
    }

    public void ResetGame()
    {
        gameInfo = new GameInfo();
        cellGrid.ResetGrid();
        cellGrid.FillGrid(level: 1);
    }

    public void OnPlayButtonClicked()
    {
        // Initialize the game
        ResetGame();

        // Hide menu
        menuCanvas.SetActive(false);

        // Transition state
        gameState = GameState.PlayerTurn;
    }

    public void OnCellClicked(Cell cell)
    {
        if (gameState == GameState.PlayerTurn)
        {
            targetCell = cell;
        }
    }

    /// <summary>
    /// We can click on: Void, Floor, Enemy, Player, Elevator
    /// </summary>
    public void HandlePlayerInstruction()
    {
        // No click
        if (targetCell == null)
        {
            return;
        }

        // Unfocus cell and all highlighted cells
        focusedCell?.UnfocusCell();
        foreach (Cell cell in highlightedCells)
        {
            cell.UnHighlightCell();
        }
        highlightedCells.Clear();

        // Click same cell or void
        if (targetCell == focusedCell || targetCell.tile.TileType == TileType.Void)
        {
            focusedCell = null;
            targetCell = null;
            return;
        }

        // Click on floor
        if (targetCell.content == null)
        {
            HandleClickOnFloor();
        }
        // Click on enemy, player or elevator
        else
        {
            HandleClickOnEntity();
        }
    }

    public void HandleClickOnFloor()
    {
        // Click with focur NOT on Player
        if (focusedCell == null || focusedCell.content == null || focusedCell.content.EntityType != EntityType.Player)
        {
            // Focus new cell
            focusedCell = targetCell;
            focusedCell.FocusCell();
            targetCell = null;
            return;
        }

        var moveOutcome = focusedCell.content.UpdateNextMove(targetCell);
        if (moveOutcome != MoveOutcome.Fail)
        {
            Debug.Log("Player moved to cell");
            gameState = GameState.AnimateAndMovePlayer;
        }
        else
        {
            Debug.Log("Player could not move to cell");
        }
        focusedCell = null;
        targetCell = null;
    }

    public void HandleClickOnEntity()
    {
        // Show enemy moves
        if (targetCell.content.EntityType == EntityType.Enemy)
        {
            foreach (Cell cell in targetCell.content.GetAttackableCells())
            {
                cell.HighlightCell();
                highlightedCells.Add(cell);
            }
        }
        else if (targetCell.content.EntityType == EntityType.Player)
        {
            foreach (Cell cell in targetCell.content.GetMoveableCells())
            {
                cell.HighlightCell();
                highlightedCells.Add(cell);
            }
        }

        focusedCell = targetCell;
        targetCell = null;
    }

    private void AnimateIdleEntities()
    {
        // Play idle animations

    }

}