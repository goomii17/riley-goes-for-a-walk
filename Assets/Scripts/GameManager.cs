using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Menu,
    PlayerTurn,
    AnimateAndMovePlayer,
    EnemyTurn,
    AnimateAndMoveEnemies,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState gameState;

    public CellGrid cellGrid;

    Cell focusedCell;
    Cell targetCell;

    public void OnEnable()
    {
        Instance = this;
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
                if (targetCell != null)
                {
                    //bool moveMade = HandlePlayerInstruction() <--- This updates focus and target and saves the instruction if valids
                    // if (moveMade)
                    // {
                    //     gameState = GameState.AnimateAndMovePlayer;
                    // }
                }
                break;
            case GameState.AnimateAndMovePlayer:
                break;
            case GameState.EnemyTurn:
                // Shoot or hit the player and calculate where to move next
                break;
            case GameState.AnimateAndMoveEnemies:
                // Make the animation and make the move
                break;
            case GameState.GameOver:
                break;
        }
    }

    public void OnPlayButtonClicked()
    {
        gameState = GameState.PlayerTurn;
    }

    public void OnCellClicked(Cell cell)
    {
        if (gameState == GameState.PlayerTurn)
        {
            targetCell = cell;
        }
    }

    private void AnimateIdleEntities()
    {
        // Play idle animations

    }

}