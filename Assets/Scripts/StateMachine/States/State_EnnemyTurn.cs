using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class State_EnnemyTurn : IState
{
    private GameManager _gameManager;
    
    private EnnemyUnit _ennemyModel;
    
    private List<EnnemyUnit> _ennemyUnits = new List<EnnemyUnit>();
    private List<Vector3Int> _redBuildingPositions = new List<Vector3Int>();
    
    public bool EnnemyEndTurn = false;

    public State_EnnemyTurn(GameManager gameManager, EnnemyUnit ennemyModel)
    {
        _gameManager = gameManager;
        _ennemyModel = ennemyModel;
    }
    
    public void OnEnter()
    {
        Debug.Log("Start enemy turn");
        EnnemyEndTurn = false;
        
        GetTileType();
        
        //creat and place a unit for each building
        foreach (var buildingPosition in _redBuildingPositions)
        {
            var ennemyUnit = GameObject.Instantiate<EnnemyUnit>(_ennemyModel, buildingPosition + new Vector3(0.5f, 0.5f, 0), quaternion.identity);
            _ennemyUnits.Add(ennemyUnit);
        }
    }
    
    public void OnUpdate()
    {
        //Move all the unit
        foreach (var unit in _ennemyUnits)
        {
            //Move the unit or just skip it's turn
        }
    }

    public void OnExit()
    {
        _redBuildingPositions.Clear();
        //Capture all the batiment where the is a ennemy unit
    }

    void GetTileType()
    {
        for (int x = -16; x < _gameManager.tilemap.size.x + 1; x++)
        {
            for (int y = -10; y < _gameManager.tilemap.size.y + 1; y++)
            {
                Vector3Int position = new Vector3Int(x, y);
                var tileType = _gameManager.tilemap.GetTile(position);
                if (tileType == _gameManager.redHQ)
                {
                    _redBuildingPositions.Add(position);
                    Debug.Log("red building" + _redBuildingPositions.Count);
                }
            }
        }
    }
}
