using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    public BaseHero SelectedHero;
    
    private void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnHeroes()
    {
        var heroCount = 1;

        for (int i = 0; i < heroCount; i++)
        {
            var randomPreffab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPreffab);
            var spawnTile = GridManager.Instance.GetHeroSpawnPoint();
            
            spawnTile.SetUnit(spawnedHero);
        }

        GameManager.Instance.updateGameState(GameState.SpawnEnnemies);
    }
    
    public void SpawnEnnemy()
    {
        var EnnemyCount = 1;

        for (int i = 0; i < EnnemyCount; i++)
        {
            var randomPreffab = GetRandomUnit<BaseEnemmy>(Faction.Hero);
            var spawnedEnnemy = Instantiate(randomPreffab);
            var spawnTile = GridManager.Instance.GetEnnemySpawnPoint();
            
            spawnTile.SetUnit(spawnedEnnemy);
        }
        GameManager.Instance.updateGameState(GameState.PlayerTurn);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        //return (T)_units.Where(u => u.Faction == faction).OrderBy(u => Random.value).First().UnitPrefabs;
        //get the unit depending on the faction
        var factionUnits = _units.Where(u => u.Faction == faction).ToList();

        if (factionUnits.Count == 0)
        {
            Debug.LogError("No units found for the specified faction.");
            return null;
        }

        // Sorting randomly by generating random values for each element
        factionUnits = factionUnits.OrderBy(u => Random.value).ToList();

        return factionUnits[0] as T;
    }

    public void SetSelectedHero(BaseHero hero)
    {
        SelectedHero = hero;
    }
    
}
