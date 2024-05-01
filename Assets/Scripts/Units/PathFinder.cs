using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units
{
    public class PathFinder
    {

        private MapManager _mapManager;
        
        public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> searchableTiles)
        {
            List<OverlayTile> openList = new List<OverlayTile>();
            List<OverlayTile> closedList = new List<OverlayTile>();

            openList.Add(start);

            while (openList.Count > 0)
            {
                //give the overlay tile with the lowest F
                OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

                openList.Remove(currentOverlayTile);
                closedList.Add(currentOverlayTile);

                if (currentOverlayTile == end)
                {
                    //finalize the path
                    return GetFinishedList(start, end);
                }

                var GetNeightbourTiles = _mapManager.GetNeightbourOverlayTiles(currentOverlayTile, searchableTiles);
                
                foreach (var neighbourTile in GetNeightbourTiles)
                {
                    //can be modified for preventing unit to go on certain tiles
                    if (neighbourTile.isBlocked || closedList.Contains(neighbourTile))
                    {
                        continue;
                    }

                    neighbourTile.G = GetManhattenDistance(start, neighbourTile);
                    neighbourTile.H = GetManhattenDistance(end, neighbourTile);

                    neighbourTile.Previous = currentOverlayTile;
                    
                    if (!openList.Contains(neighbourTile))
                    {
                        openList.Add(neighbourTile);
                    }
                }
            }

            return new List<OverlayTile>();
        }

        private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
        {
            List<OverlayTile> finishedList = new List<OverlayTile>();
            OverlayTile currentTile = end;

            while (currentTile != start)
            {
                finishedList.Add(currentTile);
                currentTile = currentTile.Previous;
            }

            finishedList.Reverse();

            return finishedList;
        }

        private int GetManhattenDistance(OverlayTile start, OverlayTile tile)
        {
            return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
        }
    }
}