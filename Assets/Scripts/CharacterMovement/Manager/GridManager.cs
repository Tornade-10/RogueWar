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
    
    [SerializeField] private Vector2Int _size;

    [SerializeField] private Tiles _tilesPrefab;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tiles> _tiles;
    
    //creat an instance of the GridManager
    private void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                var spawnTile = Instantiate(_tilesPrefab, new Vector3(x, y), quaternion.identity);
                spawnTile.name = "Tile" + x + y;

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                
                _tilesPrefab.Init(isOffset);
            }
            
        }

        _cam.transform.position = new Vector3((float)_size.x / 2 - 0.5f, (float)_size.y / 2 - 0.5f, -10);
        
        //once it done generating the grid pass the Game state to the next state
        GameManager.Instance.updateGameState(GameState.SpawnHeroes);
    }

    public Tiles GetHeroSpawnPoint()
    {
        return _tiles.Where(t => t.Key.x < _size.x / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }
    
    public Tiles GetEnnemySpawnPoint()
    {
        return _tiles.Where(t => t.Key.x > _size.x / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }
}
