using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using Random = UnityEngine.Random;

public class WFCGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _map;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private WFCModuleSet _moduleSet;
    [SerializeField] private TileBase _undetermined;
    
    [SerializeField] private WFCModule _blueHQ;
    [SerializeField] private WFCModule _redHQ;
    
    private Vector3Int _randomPositionBlue;
    private Vector3Int _randomPositionRed;
    
    private List<TileBase> _blueHQTiles = new List<TileBase>();
    private List<TileBase> _redHQTiles = new List<TileBase>();
    
    private List<WFCSlot> _slots = new List<WFCSlot>();
    
    //size of the grid
    private BoundsInt GetBounds(Vector2Int size)
    {
        BoundsInt boundsInt = new BoundsInt();

        Vector3Int min = new Vector3Int(
            Mathf.FloorToInt(-1 * size.x / 2f),
            Mathf.FloorToInt(-1 * size.y / 2f),
            0
            );

        Vector3Int max = new Vector3Int(
            Mathf.FloorToInt(size.x / 2f),
            Mathf.CeilToInt(size.y / 2f),
            1
        );
        
        boundsInt.SetMinMax(min, max);
        return boundsInt;
    }
    
    public void Initiate()
    {
        //initiate the grid
        BoundsInt gridSpace = GetBounds(_size);
        _map.ClearAllTiles();
        _slots.Clear();
        _moduleSet.ResetTileset();
        
        _blueHQTiles.Add(_blueHQ.Tile);
        _redHQTiles.Add(_redHQ.Tile);

        foreach (Vector3Int position in gridSpace.allPositionsWithin)
        {
            _slots.Add(new WFCSlot(position, _moduleSet.Tileset, _undetermined));
            _map.SetTile(position, _undetermined);
            _map.SetColor(position, Color.gray);
        }
        
        //add the HQ here

       _randomPositionBlue.x = Random.Range(-8, -16);
       _randomPositionBlue.y = Random.Range(2, -8);
       
       _randomPositionRed.x = Random.Range(8, 16);
       _randomPositionRed.y = Random.Range(-2, 8);
       
        _slots.Add(new WFCSlot(_randomPositionBlue, _blueHQTiles, _blueHQ.Tile));
        _slots.Add(new WFCSlot(_randomPositionRed, _redHQTiles, _redHQ.Tile));
    }

    public void Start()
    {
        Initiate();
    }
    
    public void Update()
    {
        Step();
    }

    public void Step()
    {
        // find minimal entropy (but greater than 0) of the grid
        var collapsableSlots = _slots.Where(slot => slot.Entropy > 0).OrderBy(slot => slot.Entropy).ToList();
        
        if (collapsableSlots.Count > 0)
        {
            //Observation
            float minEntropy = collapsableSlots[0].Entropy;
            
            // collapse one remaining tile among the tiles win min entropy
            WFCSlot collapsedSlot = collapsableSlots.OrderBy(slot => Random.value).First(slot => slot.Entropy == minEntropy);

            collapsedSlot.ForceCollapse();
            //change it's collor and position
            //Debug.Log("New collapsed tile: [" + collapsedSlot.Position + "]" + collapsedSlot.Tile.name);

            _map.SetTile(collapsedSlot.Position, collapsedSlot.Tile);
            _map.SetColor(collapsedSlot.Position, Color.white);
            
            //if there may be an empty slot that can't be propagated, reset the whole map
            if (!Propagate(collapsedSlot))
            {
                Debug.LogWarning("Contradiction detected. Reset the collapse operation.");
                Initiate();
            }
        }
        else
        {
            Debug.Log("All slots collapsed");
        }
        
        _map.SetTile(_randomPositionBlue, _blueHQ.Tile);
        _map.SetTile(_randomPositionRed, _redHQ.Tile);
    }

    private bool Propagate(WFCSlot propagatorSlot)
    {
        Stack<WFCSlot> slotsStack = new Stack<WFCSlot>();
        //List<WFCSlot> visitedSlots = new List<WFCSlot>();
        
        slotsStack.Push(propagatorSlot);
        
        //in action while there is more than 0 slots
        do
        {
            WFCSlot currentSlot = slotsStack.Pop();
            //visitedSlots.Add(currentSlot);

            foreach (Vector3Int direction in WFCModuleSet.NeighboursTilePositions)
            {
                var newSlot = _slots.FirstOrDefault(slot => slot.Position == currentSlot.Position + direction && slot.Entropy > 0);
                //if (newSlot != null && !visitedSlots.Contains(newSlot))
                if (newSlot != null)
                {
                    var possibleTiles = _moduleSet.GetTiles(currentSlot, direction);
                    
                    if (newSlot.SetNewDomain(possibleTiles))
                    {
                        slotsStack.Push(newSlot);
                    }

                    if (newSlot.Entropy == -1)
                    {
                        return false;
                    }

                    if (newSlot.Entropy == 0)
                    {
                        _map.SetTile(newSlot.Position, newSlot.Tile);
                        _map.SetColor(newSlot.Position, Color.white);
                    }
                }
            }
            
        } while (slotsStack.Count > 0);
        
        return true;
    }
}
