using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Update = Unity.VisualScripting.Update;

public class GameManager : MonoBehaviour
{
    public Tilemap tilemap;

    [SerializeField] private WFCGenerator _wfcGenerator;
    [SerializeField] private WFCAnalyzer _wfcAnalyzer;
    
    private State_MapGeneration _stateMapGeneration;
    private State_PlayerTurn _statePlayerTurn;
    private State_EnnemyTurn _stateEnnemyTurn;
    
    [Header("Red buildings")]
    public TileBase redHQ;
    public TileBase redBarrack;
    public TileBase redFactrory;
    public TileBase redAirPort;

    [SerializeField] private EnnemyUnit _ennemyUnit;
    
    [Header("Blue buildings")]
    public TileBase blueHQ;
    public TileBase blueBarrack;
    public TileBase blueFactrory;
    public TileBase blueAirPort;

    [SerializeField] private PlayerUnit _playerUnit;
    
    [Header("States")]
    public State_MapGeneration StateMapGeneration => _stateMapGeneration;
    public State_PlayerTurn StatePlayerTurn => _statePlayerTurn;
    public State_EnnemyTurn StateEnnemyTurn => _stateEnnemyTurn;

    private StateMachine _stateMachine;
    
    public void Start()
    {
        _stateMachine = new StateMachine();
        _stateMapGeneration = new State_MapGeneration(this);
        _statePlayerTurn = new State_PlayerTurn(this, _playerUnit);
        _stateEnnemyTurn = new State_EnnemyTurn(this, _ennemyUnit);
        
        _stateMachine.AddTransition(_stateMapGeneration, _statePlayerTurn, () => _wfcGenerator.mapGenerated);
        _stateMachine.AddTransition(_statePlayerTurn, _stateEnnemyTurn, () => _statePlayerTurn.playerEndTurn);
        _stateMachine.AddTransition(_stateEnnemyTurn, _statePlayerTurn, () => _stateEnnemyTurn.EnnemyEndTurn);
        
        _stateMachine.ChangeState(_stateMapGeneration);
    }

    public void Update()
    {
        _stateMachine.UpdateState();
    }

    public void Generate()
    {
        if (_wfcAnalyzer is null)
        {
            Debug.LogWarning("no analyzer");
            return;
        }
        if (_wfcGenerator is null)
        {
            Debug.LogWarning("no generator");
            return;
        }
        _wfcAnalyzer.Analyze();
        _wfcGenerator.Initiate();
    }

    public void Step()
    {
        if (_wfcGenerator is null)
        {
            Debug.LogWarning("no generator");
            return;
        }
        _wfcGenerator.Step(new List<WFCSlot>());
    }

    public void SkipPlayerTurn()
    {
        _statePlayerTurn.playerEndTurn = true;
    }
    
    public void SkipEnemyTurn()
    {
        _stateEnnemyTurn.EnnemyEndTurn = true;
    }
}
