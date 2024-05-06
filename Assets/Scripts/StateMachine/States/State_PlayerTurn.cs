using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class State_PlayerTurn : IState
{
    private GameManager _gameManager;
    
    private PlayerUnit _playerModel;
    
    private List<PlayerUnit> _playerUnits = new List<PlayerUnit>();
    private List<Vector3Int> _blueBuildingPositions = new List<Vector3Int>();

    public bool playerEndTurn = false;

    public State_PlayerTurn(GameManager gameManager, PlayerUnit playerUnit)
    {
        _gameManager = gameManager;
        _playerModel = playerUnit;
    }
    
    public void OnEnter()
    {
        Debug.Log("Start player turn");
        playerEndTurn = false;
        
        GetTileType();
        
        //creat and place a unit for each building
        foreach (var buildingPosition in _blueBuildingPositions)
        {
            var playerUnit = GameObject.Instantiate<PlayerUnit>(_playerModel, buildingPosition + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            _playerUnits.Add(playerUnit);
        }
    }
    
    public void OnUpdate()
    {
        //let the player move their unit
        foreach (var unit in _playerUnits)
        {
            
        }
    }

    public void OnExit()
    {
        _blueBuildingPositions.Clear();
        //Capture all the batiment where the is a player unit
    }
    
    void GetTileType()
    {
        for (int x = -16; x < _gameManager.tilemap.size.x + 1; x++)
        {
            for (int y = -10; y < _gameManager.tilemap.size.y + 1; y++)
            {
                Vector3Int position = new Vector3Int(x, y);
                var tileType = _gameManager.tilemap.GetTile(position);
                if (tileType == _gameManager.blueHQ)
                {
                    _blueBuildingPositions.Add(position);
                    Debug.Log("blue buildings" + _blueBuildingPositions.Count);
                }
            }
        }
    }
}
