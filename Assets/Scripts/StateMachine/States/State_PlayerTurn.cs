using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class State_PlayerTurn : IState
{
    private GameManager _gameManager;
    
    public PlayerUnit playerModel;
    
    private List<EnnemyUnit> _playerUnit = new List<EnnemyUnit>();
    private List<TileBase> _blueBuildings = new List<TileBase>();

    public bool playerEndTurn = false;

    public State_PlayerTurn(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public void OnEnter()
    {
        Debug.Log("Start player turn");
        playerEndTurn = false;
        //creat and place a unit for each building
        foreach (var building in _blueBuildings)
        {
            var playerUnit = GameObject.Instantiate(playerModel);
        }
    }
    
    public void OnUpdate()
    {
        //let the player move their unit
    }

    public void OnExit()
    {
        //Capture all the batiment where the is a player unit
    }
}
