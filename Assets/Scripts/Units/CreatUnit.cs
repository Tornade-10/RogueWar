using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreatUnit : MonoBehaviour
{
    public void PlaceUnit(TileBase building, Unit unit)
    {
        //creat a unit depending on the building and if it's a player or an enemy

        var theMovingThingy = Instantiate(unit);
        

        // var ennemy = Instantiate(ennemyModel);
        // _ennemyUnit.Add(ennemy);
    }
}