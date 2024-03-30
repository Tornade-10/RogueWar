using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _offsetColor;
    [SerializeField] private GameObject _hilight;

    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private bool _isWalkable;

    public BaseUnit OccupiedUnit;
    
    public bool Walkable => _isWalkable && OccupiedUnit == null;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    private void OnMouseEnter()
    {
        _hilight.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        _hilight.SetActive(false);
    }

    // private void OnMouseDown()
    // {
    //     if (GameManager.Instance.State != GameState.SpawnHeroes)
    //     {
    //         return;
    //     }
    //
    //     if (OccupiedUnit != null)
    //     {
    //         if (OccupiedUnit.Faction == Faction.Hero)
    //         {
    //             UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
    //         }
    //         else
    //         {
    //             if (UnitManager.Instance.SelectedHero != null)
    //             {
    //                 var ennemy = (BaseEnemmy)OccupiedUnit;
    //                 //has to be modified for hp system
    //                 Destroy(ennemy.gameObject);
    //                 UnitManager.Instance.SetSelectedHero(null);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         if (UnitManager.Instance.SelectedHero != null)
    //         {
    //             SetUnit(UnitManager.Instance.SelectedHero);
    //             UnitManager.Instance.SetSelectedHero(null);
    //         }
    //     }
    // }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null)
        {
            unit.OccupiedTile = null;
        }
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}
