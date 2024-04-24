using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class EnnemieManager : MonoBehaviour
{
    public static EnnemieManager Instance;

    //public List<> EnnemyUnit;
    public List<OverlayTile> RedBattiment = new List<OverlayTile>();
    
    
    
    public void EnnemieTurn()
    {
        StartTurn();
        MidTurn();
        EndTurn();
    }
    
    private void StartTurn()
    {
        foreach (var battiment in RedBattiment)
        {
            
        }
    }

    private void MidTurn()
    {
        //for each enemy unit, move them to a location
        
    }

    private void EndTurn()
    {
        GameManager.Instance.updateGameState(GameState.PlayerTurn);
    }
}
