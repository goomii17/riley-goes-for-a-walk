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
    public static GameManager instance;

    public GameState gameState;

    public CellBoard cellBoard;

    Cell focusedCell;

    Cell targetCell;

    void Update()
    {
        switch (gameState)
        {
            case GameState.Menu:
                // Animations or whatever
                break;
            case GameState.PlayerTurn:
                // Play Idle animations
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
                break;
            case GameState.AnimateAndMoveEnemies:
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
}