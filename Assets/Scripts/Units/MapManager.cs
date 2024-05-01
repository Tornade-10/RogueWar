using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Units
{
    public class MapManager : MonoBehaviour
    {

        public GameObject overlayPrefab;
        public GameObject overlayContainer;

        public Dictionary<Vector2Int, OverlayTile> map;
        
        public void GenerateMap()
        {
            var tileMap = gameObject.GetComponentInChildren<Tilemap>();
            map = new Dictionary<Vector2Int, OverlayTile>();

            BoundsInt bounds = tileMap.cellBounds;

            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector2Int(x, y);
                    var tileKey = new Vector2Int(x, y);
                    if (tileMap.HasTile((Vector3Int)tileLocation) && !map.ContainsKey(tileKey))
                    {
                        var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                        var cellWorldPosition = tileMap.GetCellCenterWorld((Vector3Int)tileLocation);

                        overlayTile.transform.position = new Vector2(cellWorldPosition.x, cellWorldPosition.y);

                        //rest the sorting order by default in case it's not equal to 1 by the start
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder =
                            tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = tileLocation;

                        map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                    }
                }
            }
        }
        
        public List<OverlayTile> GetNeightbourOverlayTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles)
        {
            Dictionary<Vector2Int, OverlayTile> tileToSearch = new Dictionary<Vector2Int, OverlayTile>();

            if (searchableTiles.Count > 0)
            {
                foreach (var tile in searchableTiles)
                {
                    tileToSearch.Add(tile.gridLocation, tile);
                }
            }
            else
            {
                tileToSearch = map;
            }
            
            List<OverlayTile> neighbours = new List<OverlayTile>();

            //right
            Vector2Int locationToCheck = new Vector2Int(
                currentOverlayTile.gridLocation.x + 1,
                currentOverlayTile.gridLocation.y
            );

            if (tileToSearch.ContainsKey(locationToCheck))
            {
                neighbours.Add(tileToSearch[locationToCheck]);
            }

            //left
            locationToCheck = new Vector2Int(
                currentOverlayTile.gridLocation.x - 1,
                currentOverlayTile.gridLocation.y
            );

            if (tileToSearch.ContainsKey(locationToCheck))
            {
                neighbours.Add(tileToSearch[locationToCheck]);
            }

            //top
            locationToCheck = new Vector2Int(
                currentOverlayTile.gridLocation.x,
                currentOverlayTile.gridLocation.y + 1
            );

            if (tileToSearch.ContainsKey(locationToCheck))
            {
                neighbours.Add(tileToSearch[locationToCheck]);
            }

            //bottom
            locationToCheck = new Vector2Int(
                currentOverlayTile.gridLocation.x,
                currentOverlayTile.gridLocation.y - 1
            );

            if (tileToSearch.ContainsKey(locationToCheck))
            {
                neighbours.Add(tileToSearch[locationToCheck]);
            }

            return neighbours;
        }
        
    }
}
