using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    [SerializeField] private Vector2 _size;

    [SerializeField] private Tiles _tilesPrefab;

    //[SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tiles> _tiles;
    
    //creat an instance of the GridManager
    private void Awake()
    {
        Instance = this;
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (float x = -17.5f; x < _size.x; x++)
        {
            for (float y = -9.5f; y < _size.y; y++)
            {
                var spawnTile = Instantiate(_tilesPrefab, new Vector3(x, y), quaternion.identity);
                spawnTile.name = "Tile" + x + y;

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                
                
                _tilesPrefab.Init(isOffset);
            }
        }
    }
}
