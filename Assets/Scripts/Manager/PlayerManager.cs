using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    public void PlayerTurn()
    {
        StartTurn();
        MidTurn();
        EndTurn();
    }

    private void StartTurn()
    {
        
    }

    private void MidTurn()
    {
        //for each player unit
        
    }

    private void EndTurn()
    {
        GameManager.Instance.updateGameState(GameState.EnnemyTurn);
    }
}
