using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Units
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager _instance;
        public static MapManager Instance { get { return _instance; } }

        public GameObject overlayPrefab;
        public GameObject overlayContainer;

        public Dictionary<Vector2Int, OverlayTile> map;

        private void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else
            {
                _instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
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
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = tileLocation;
                        
                        map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                    }
                }
            }
        }
    }
}
