using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class State_PlayerTurn : IState
{
    private GameManager _gameManager;
    
    public PlayerUnit playerModel;
    
    private List<PlayerUnit> _playerUnits = new List<PlayerUnit>();
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
        
        GetTileType();
        
        //creat and place a unit for each building
        foreach (var building in _blueBuildings)
        {
            playerModel = new PlayerUnit();
            GameObject.Instantiate(playerModel, building.GameObject().transform.position, building.GameObject().transform.rotation);
            _playerUnits.Add(playerModel);
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
        _blueBuildings.Clear();
        //Capture all the batiment where the is a player unit
    }
    
    void GetTileType()
    {
        for (int x = -16; x < _gameManager.tilemap.size.x + 1; x++)
        {
            for (int y = -10; y < _gameManager.tilemap.size.y + 1; y++)
            {
                var tileType = _gameManager.tilemap.GetTile(new Vector3Int(x, y));
                if (tileType == _gameManager.blueHQ)
                {
                    _blueBuildings.Add(tileType);
                    Debug.Log("blue buildings" + _blueBuildings.Count);
                }
            }
        }
    }
}
