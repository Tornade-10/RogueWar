using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Update = Unity.VisualScripting.Update;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private MapManager _mapManager;
    [SerializeField] private State_MapGeneration _stateMapGeneration;
    [SerializeField] private State_PlayerTurn _statePlayerTurn;
    [SerializeField] private State_EnnemyTurn _stateEnnemyTurn;

    public MapManager MapManager => _mapManager;

    public State_MapGeneration StateMapGeneration => _stateMapGeneration;
    public State_PlayerTurn StatePlayerTurn => _statePlayerTurn;
    public State_EnnemyTurn StateEnnemyTurn => _stateEnnemyTurn;

    private StateMachine _stateMachine;

    //Creat an instance of the game manager
    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    public void Start()
    {
        _stateMachine = new StateMachine();
        _stateMapGeneration = new State_MapGeneration();
        _statePlayerTurn = new State_PlayerTurn();
        _stateEnnemyTurn = new State_EnnemyTurn();
        
        //_stateMachine.AddTransition(_stateMapGeneration, _statePlayerTurn, () => );
        _stateMachine.AddTransition(_statePlayerTurn, _stateEnnemyTurn, () => _statePlayerTurn.playerEndTurn);
        _stateMachine.AddTransition(_stateEnnemyTurn, _statePlayerTurn, () => _stateEnnemyTurn.EnnemyEndTurn);
        
        _stateMachine.ChangeState(_stateMapGeneration);
    }

    public void Update()
    {
        _stateMachine.UpdateState();
    }
}
