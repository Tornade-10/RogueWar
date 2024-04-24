using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChange; 
    
    //Creat an instance of the game manager
    private void Awake()
    {
        Instance = this;
    }

    //set the state of the game on Generate grid
    private void Start()
    {
        updateGameState(GameState.PlayerTurn);
    }

    public void updateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.GenerateGrid:
                MapManager.Instance.GenerateMap();
                break;
            case GameState.PlayerTurn:
                PlayerManager.Instance.PlayerTurn();
                break;
            case GameState.EnnemyTurn:
                EnnemieManager.Instance.EnnemieTurn();
                break;
            case GameState.WonState:
                break;
            case GameState.LooseState:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }
}

public enum GameState
{
    GenerateGrid,
    PlayerTurn,
    EnnemyTurn,
    WonState,
    LooseState
}
