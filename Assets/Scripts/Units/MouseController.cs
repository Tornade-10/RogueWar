using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using static Units.ArrowTranslator;

namespace Units
{
    public class MouseController : MonoBehaviour
    {
        public GameObject cursor;
        public float speed;
        public GameObject characterPrefab;
        private CharacterInfo character;

        private PathFinder pathFinder;
        private RangeFinder rangeFinder;
        private ArrowTranslator _arrowTranslator;
        private List<OverlayTile> path;
        private List<OverlayTile> inRangeTiles = new List<OverlayTile>();

        private bool isMoving = false;
        public bool hasMoved = false;

        private void Start()
        {
            pathFinder = new PathFinder();
            rangeFinder = new RangeFinder();
            path = new List<OverlayTile>();
            _arrowTranslator = new ArrowTranslator();
        }

        void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                OverlayTile overlayTile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = overlayTile.transform.position;
                cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.transform.GetComponent<SpriteRenderer>().sortingOrder;
                if (Input.GetMouseButtonDown(0))
                {
                    overlayTile.ShowTile();

                    if (inRangeTiles.Contains(overlayTile) && !isMoving)
                    {
                        path = pathFinder.FindPath(character.standingOnTile, overlayTile, inRangeTiles);

                        foreach (var tile in inRangeTiles)
                        {
                            tile.SetArrowSprit(ArrowDirection.None);
                        }

                        for (int i = 0; i < path.Count; i++)
                        {
                            var previousTile = i > 0 ? path[i - 1] : character.standingOnTile;
                            var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                            var arrowDirection = _arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                            path[i].SetArrowSprit(arrowDirection);
                        }
                    }
                    
                    if (character == null)
                    {
                        character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                        PositionCharacterOnLine(overlayTile);
                        //character.standingOnTile = tile;
                        GetInRangeTiles();
                    } else
                    {
                        isMoving = true;
                    }
                }
            }

            if (path.Count > 0 && isMoving)
            {
                MoveAlongPath();
            }
        }

        private void GetInRangeTiles()
        {
            foreach (var tile in inRangeTiles)
            {
                tile.HideTile();
            }
            
            //the "3" can be modified to "Character movement"
            inRangeTiles = rangeFinder.GetTilesInRange(character.standingOnTile, 5);

            foreach (var tile in inRangeTiles)
            {
                tile.ShowTile();
            }
        }
        
        private void MoveAlongPath()
        {
            var step = speed * Time.deltaTime;
            
            character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
            character.transform.position = new Vector2(character.transform.position.x, character.transform.position.y);

            if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
            {
                PositionCharacterOnLine(path[0]);
                path.RemoveAt(0);
            }

            //the end of the path
            if (path.Count == 0)
            {
                GetInRangeTiles();
                isMoving = false;
                hasMoved = true;
            }
        }

        private void PositionCharacterOnLine(OverlayTile tile)
        {
            character.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y);
            character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            character.standingOnTile = tile;
        }

        private static RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }
    }
}
