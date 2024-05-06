using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool hasMoved = false;
    
    public int healthPoint = 10;
    public int defence = 0;
    public int attack = 1;
    public int attackRange = 1;
    public int movement = 5;

    public Vector3 position = new Vector3();
    
    public Team team;
    public MovementType movementType;

    public enum Team
    {
        BlueTeam,
        RedTeam
    }
    
    public enum MovementType
    {
        Infantry,
        Ground,
        Flying
    }

    public void Move()
    {
        //Move the unit to the targeted place
    }
    
    public void Attack()
    {
        //Attack the targeted unit
        // if (rangeAttack >= Target.Distance)
        // {
        //     Target.Health -= (Attack - Defence);
        // }
    }
    
    public void Capture()
    {
        //Capture a building if the unit is on it
    }
}
