using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WFCSlot
{
    private List<TileBase> _domain;
    private float _entropy;
    private Vector3Int _position;
    private TileBase _tile;
    private TileBase _undetermined;
    

    public Vector3Int Position => _position;
    public float Entropy => _entropy;
    public List<TileBase> Domain
    {
        get
        {
            return _domain;
        }
        set
        {
            _domain = value;
        }
    }

    public TileBase Tile
    {
        get
        {
            //Solved slot
            if (_domain.Count == 1)
            {
                return _domain[0];
            }
            else
            {
                //Unsolved slot
                return _undetermined;
            }
        }
        set
        {
            _tile = value;
        }
    }

    public WFCSlot(Vector3Int pos, List<TileBase> startDomain, TileBase undeterminedTile)
    {
        _position.x = pos.x;
        _position.y = pos.y;
        
        _domain = new List<TileBase>(startDomain);
        _undetermined = undeterminedTile;
        
        _entropy = _domain.Count;
    }

    private string TilesToString(List<TileBase> tiles)
    {
        string toString = new string("");
        foreach (var tile in tiles)
        {
            toString += tile.name + "\n";
        }
        return toString;
    }
    
    public void ForceCollapse()
    {
        if (_domain.Count <= 0)
        {
            return;
        }
        
        TileBase collapsedTile = _domain[Random.Range(0, _domain.Count)];

        //TileBase collapsedTile = _domain.OrderBy(tile => Random.Range(0f, 1f)).First();
        _domain.Clear();
        _domain.Add(collapsedTile);
        
        _entropy = 0;
    }

    public void ForceRemove(TileBase tile)
    {
        //Remove an unwanted tile (like the HQ)
        if (_domain.Count <= 1 && _domain.Contains(tile))
        {
            Debug.Log("no other tile : " + tile.name);
        }
        else
        {
            _domain.Remove(tile);
            _entropy = _domain.Count - 1;
        }
    }

    public bool SetNewDomain(List<TileBase> propagatedSlotDomain)
    {
        //List<TileBase> newDomain = _domain.Intersect(propagatedSlotDomain).ToList();

        List<TileBase> newDomain = new List<TileBase>();
        newDomain = propagatedSlotDomain.Intersect(_domain).ToList();
        
        Debug.Log("Existing Domain : " + TilesToString(_domain));
        Debug.Log("Possibilities : " + TilesToString(propagatedSlotDomain));
        Debug.Log("Result : " + TilesToString(newDomain));
        
        if (newDomain.Count <= 0)
        {
            Debug.LogError("Domains not consistent");
            _entropy = -1;
            return false;
        }
        else
        {
            bool changed = _domain.Count != newDomain.Count;
            _domain = new List<TileBase>(newDomain);
            _entropy = _domain.Count - 1;
        
            // _domain = newDomain;
            // _entropy = _domain.Count - 1;

            return changed;
        }
    }
}
