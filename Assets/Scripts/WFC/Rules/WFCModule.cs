using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(menuName = "WFC Module", fileName = "module")]
[Serializable]
public class WFCModule : ScriptableObject
{
    
    //enum of the direction of the neighbors of the tiles
    public enum Neighbourhood
    {
        Up,
        Right,
        Down,
        Left,
        None
    }

    //get wich neighbors can be placed where
    public static Neighbourhood VectorToEnumDirection(Vector3Int direction)
    {
        if (direction == Vector3Int.up)
        {
            return Neighbourhood.Up;
        }
        else if (direction == Vector3Int.right)
        {
            return Neighbourhood.Right;
        }
        else if (direction == Vector3Int.down)
        {
            return Neighbourhood.Down;
        }
        else if (direction == Vector3Int.left)
        {
            return Neighbourhood.Left;
        }
        else
        {
            return Neighbourhood.None;
        }
    }
    
    public static Vector3Int EnumToVectorDirection(Neighbourhood direction)
    {
        if (direction == Neighbourhood.Up)
        {
            return Vector3Int.up;
        }
        else if (direction == Neighbourhood.Right)
        {
            return Vector3Int.right;
        }
        else if (direction == Neighbourhood.Down)
        {
            return Vector3Int.down;
        }
        else if (direction == Neighbourhood.Left)
        {
            return Vector3Int.left;
        }
        else
        {
            return Vector3Int.zero;
        }
    }
    
    [Serializable]
    public class ModuleRule
    {
        public Neighbourhood neighbourhoodDirection;
        public List<TileBase> neighbours;
    }
    
    [SerializeField] private TileBase _tile;
    [SerializeField] private List<ModuleRule> _rules = new List<ModuleRule>();

    public TileBase Tile
    {
        get => _tile;
        set => _tile = value;
    }

    public List<ModuleRule> Rules
    {
        get => _rules;
        set => _rules = value;
    }

    public void AddNeighbour(TileBase tile, Vector3Int direction)
    {
        ModuleRule rule = Rules.FirstOrDefault(r => r.neighbourhoodDirection == VectorToEnumDirection(direction));
        if (rule == null)
        {
            rule = new ModuleRule();
            rule.neighbours = new List<TileBase>();
            rule.neighbourhoodDirection = VectorToEnumDirection(direction);
            Rules.Add(rule);
        }
        
        if (!rule.neighbours.Contains(tile))
        {
            rule.neighbours.Add(tile);
        }
    }
}
