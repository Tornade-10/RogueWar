using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class State_EnnemyTurn : IState
{
    private GameManager _gameManager;
    
    public EnnemyUnit ennemyModel;
    
    private List<EnnemyUnit> _ennemyUnit = new List<EnnemyUnit>();
    private List<TileBase> _redBuildings = new List<TileBase>();

    public bool EnnemyEndTurn = false;

    public State_EnnemyTurn(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public void OnEnter()
    {
        Debug.Log("Start enemy turn");
        EnnemyEndTurn = false;
        
        //creat and place a unit for each building
        foreach (var buildings in _redBuildings)
        {
            var ennemyUnit = GameObject.Instantiate(ennemyModel);
        }
        
        Debug.Log("new ennemy");
    }
    
    public void OnUpdate()
    {
        //Move all the unit
    }

    public void OnExit()
    {
        //Capture all the batiment where the is a ennemy unit
    }
}
