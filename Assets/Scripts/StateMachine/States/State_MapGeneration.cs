using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_MapGeneration : IState
{

    private GameManager _gameManager;
    
    public State_MapGeneration(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public void OnEnter()
    {
        Debug.Log("Start map generation");
        if (_gameManager is not null)
        {
            //do an analize
            _gameManager.Generate();
        }
    }
    
    public void OnUpdate()
    {
        if (_gameManager is not null)
        {
            //generate the map
            _gameManager.Step();
        }
        else
        {
            Debug.LogWarning("No game manager");
        }
    }

    public void OnExit()
    {
        //go to player turn
        Debug.Log("map generated");
    }
}
