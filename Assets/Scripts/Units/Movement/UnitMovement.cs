using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Workshop;
using CharacterInfo = UnityEngine.CharacterInfo;

namespace AStar
{
    public class UnitMovement : MonoBehaviour
    {
        public bool hasBeenReversed = false;
        private bool _hasArrived = false;
        
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _tilemapDebug;
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _objective;
        [SerializeField] private GameObject _unit;
        
        private List<OverlayTile> path;
        private CharacterInfo character;
        
        [SerializeField] [Range(0.1f, 5f)] private float _heuristicFactor = 3f;

        private TileBase _openTile;
        private TileBase _closedTile;
        private TileBase _pathTile;

        private List<AStarNode> _closeNodes;
        private List<AStarNode> _openNodes;
        private List<AStarNode> _pathFound;

        private AStarNode _currentNode;

        private List<Vector3> _neighbours = new List<Vector3>()
        {
            Vector3.up,
            Vector3.right,
            Vector3.down,
            Vector3.left,
        };

        void ShowBounds()
        {
            _tilemapDebug.ClearAllTiles();
        }
        
        public void FindPath()
        {
            _openNodes = new List<AStarNode>();
            _closeNodes = new List<AStarNode>();
            _pathFound = new List<AStarNode>();

            _tilemapDebug.ClearAllTiles();


            _openNodes.Add(new AStarNode(_start.position, null, 0,
                _heuristicFactor * Vector3.Distance(_start.position, _objective.position)));
            _currentNode = _openNodes[0];
            NextNode();
        }

        private void NextNode()
        {
            var tileOfTheMap = _tilemap.GetTilesBlock(_tilemap.cellBounds);

            if (_currentNode is not null && Vector3.Distance(_currentNode.Position, _objective.position) <= 1f)
            {
                Debug.Log("Path Found");
                if (!hasBeenReversed)
                {
                    _pathFound.Reverse();
                    hasBeenReversed = true;
                }
                MoveAlongPath();
                return;
            }

            if (_closeNodes.Count >= tileOfTheMap.Length)
            {
                Debug.LogWarning("Path not Found");
                return;
            }

            if (_openNodes.Count < 0)
            {
                Debug.LogWarning("Path not Found");
                return;
            }

            _currentNode = _openNodes.OrderBy(n => n.F).FirstOrDefault();

            foreach (var neighbour in _neighbours)
            {

                float possibleG = _currentNode.G + neighbour.magnitude;
                float possibleH = Vector3.Distance(_currentNode.Position + neighbour, _objective.position);
                float possibleF = possibleG + possibleH;

                if (_tilemap.GetTile(new Vector3Int(Mathf.RoundToInt(_currentNode.Position.x),
                        Mathf.RoundToInt(_currentNode.Position.y), Mathf.RoundToInt(_currentNode.Position.z))) is null)
                {
                    continue;
                }

                if (_closeNodes.Exists(n => n.Position == _currentNode.Position + neighbour) || _openNodes.Exists(n =>
                        n.Position == _currentNode.Position + neighbour && n.F >= possibleF))
                {
                    continue;
                }

                _openNodes.Add(new AStarNode(_currentNode.Position + neighbour, _currentNode,
                    _currentNode.G + neighbour.magnitude,
                    Vector3.Distance(_currentNode.Position + neighbour, _objective.position)));
            }

            _closeNodes.Add(_currentNode);
            _openNodes.Remove(_currentNode);

            GetPath(_currentNode);

            //Drawing Part

            _tilemapDebug.ClearAllTiles();

            foreach (var node in _openNodes)
            {
                _tilemapDebug.SetTile(
                    new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y),
                        Mathf.RoundToInt(node.Position.z)),
                    _openTile
                );
            }

            foreach (var node in _closeNodes)
            {
                _tilemapDebug.SetTile(
                    new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y),
                        Mathf.RoundToInt(node.Position.z)),
                    _closedTile
                );
            }

            foreach (var node in _pathFound)
            {
                _tilemapDebug.SetTile(
                    new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y),
                        Mathf.RoundToInt(node.Position.z)),
                    _pathTile
                );
            }
        }

        public void GetPath(AStarNode destination)
        {
            if (destination == null)
            {
                Debug.LogError("No destination");
                return;
            }

            _pathFound.Clear();
            AStarNode node = destination;

            while (node.Parent is not null)
            {
                _pathFound.Add(node);
                node = node.Parent;
            }
        }

        private void MoveAlongPath()
        {
            _unit.transform.position = Vector2.MoveTowards(_unit.transform.position, _pathFound[0].Position,
                5f * Time.deltaTime);
            
            //the unit move toward the position until it come to the destination 
            if(Vector2.Distance(_unit.transform.position, _pathFound[0].Position) < 0.00001f && _pathFound.Count >= 2)
            {
                _pathFound.RemoveAt(0);
                Debug.Log(_pathFound.Count);
            }
            else
            {
                _hasArrived = true;
                Debug.Log("arrived at destination");
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            FindPath();
        }

        public void MoveToTarget()
        {
            while (!_hasArrived)
            {
                //find the next node
                NextNode();
            }
        }
    }
}
