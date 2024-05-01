using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class State_PlayerTurn : State
{
    public PlayerUnit playerModel;
    
    private List<EnnemyUnit> _playerUnit = new List<EnnemyUnit>();
    private List<TileBase> _blueBattiment = new List<TileBase>();

    public bool playerEndTurn = false;

    public void OnEnter()
    {
        playerEndTurn = false;
        foreach (var battiment in _blueBattiment)
        {
            
        }
    }
    
    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
