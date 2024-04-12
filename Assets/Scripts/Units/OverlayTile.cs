using System.Collections.Generic;
using UnityEngine;
using static Units.ArrowTranslator;

namespace Units
{
    public class OverlayTile : MonoBehaviour
    {
        //the starting pos
        public int G;
        //the end pos
        public int H;
        //cost of the path
        public int F { get { return G + H; } }

        public bool isBlocked = false;

        public OverlayTile Previous;
        public Vector2Int gridLocation;

        public List<Sprite> arrows;

        public void HideTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            SetArrowSprit(ArrowDirection.None);
        }
        
        public void ShowTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        public void SetArrowSprit(ArrowDirection direction)
        {
            var arrow = GetComponentsInChildren<SpriteRenderer>()[1];
            
            if (direction == ArrowDirection.None)
            {
                arrow.color = new Color(1, 1, 1, 0);
            }
            else
            {
                arrow.color = new Color(1, 1, 1, 1);
                arrow.sprite = arrows[(int)direction];
                arrow.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
            }
        }
    }
}
