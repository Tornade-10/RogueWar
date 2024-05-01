using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class State_EnnemyTurn : State
{
    public EnnemyUnit ennemyModel;
    
    private List<EnnemyUnit> _ennemyUnit = new List<EnnemyUnit>();
    private List<TileBase> _redBattiment = new List<TileBase>();

    public bool EnnemyEndTurn = false;
    
    public void OnEnter()
    {
        EnnemyEndTurn = false;
        foreach (var battiment in _redBattiment)
        {
            
        }
        
        // var ennemy = Instantiate(ennemyModel);
        // _ennemyUnit.Add(ennemy);
        Debug.Log("new ennemy");
    }
    
    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
