using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WFCAnalyzer : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private WFCModuleSet _moduleSet;

    public void Analyze()
    {
        _moduleSet.Clear();
        
        //TileBase[] rootTile = _tilemap.GetTilesBlock(_tilemap.cellBounds);
        foreach (var tilePos in _tilemap.cellBounds.allPositionsWithin)
        {
            TileBase rootTile = _tilemap.GetTile(tilePos);
            
            if (rootTile != null)
            {
                foreach (Vector3Int direction in WFCModuleSet.NeighboursTilePositions)
                {
                    TileBase moduleTile = _tilemap.GetTile(tilePos + direction);
                    if (moduleTile != null)
                    {
                        _moduleSet.AddModule(rootTile, moduleTile, direction);
                    }
                }
            }
        }
    }
}
