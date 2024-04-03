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
}
