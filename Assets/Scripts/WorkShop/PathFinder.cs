using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;


namespace Workshop
{
    public class PathFinder : MonoBehaviour
    {

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _tilemapDebug;
        [FormerlySerializedAs("_strat")] [SerializeField] private Transform _start;
        [SerializeField] private Transform _objective;

        [SerializeField] private TileBase _openTile;
        [SerializeField] private TileBase _closedTile;
        [SerializeField] private TileBase _pathTile;

        private List<AStarNode> _closeNodes;
        private List<AStarNode> _openNodes;
        private List<AStarNode> _pathFound;

        private AStarNode _currentNode;

        private List<Vector3> _neighbours = new List<Vector3>()
        {
            Vector3.up,
            new Vector3(1, 1, 0),
            Vector3.right,
            new Vector3(1, -1, 0),
            Vector3.down,
            new Vector3(-1, -1, 0),
            Vector3.left,
            new Vector3(-1, 1, 0)
        };

        public void FindPath()
        {
            _openNodes = new List<AStarNode>();
            _closeNodes = new List<AStarNode>();
            _pathFound = new List<AStarNode>();

            _tilemapDebug.ClearAllTiles();
            
            _openNodes.Add(new AStarNode(_start.position, null, 0, Vector3.Distance(_start.position, _objective.position)));
            _currentNode = _openNodes[0];
            FindPathIteration();
        }

        private void FindPathIteration()
        {
            var tileOfTheMap = _tilemap.GetTilesBlock(_tilemap.cellBounds);
            
            if (_currentNode is not null && Vector3.Distance(this._currentNode.Position, _objective.position) < 1f)
            {
                Debug.Log("Path Found");
                return;
            }

            if (_closeNodes.Count >= tileOfTheMap.Length)
            {
                Debug.LogWarning("No Path Found");
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

                if (_tilemap.GetTile(new Vector3Int(Mathf.RoundToInt(_currentNode.Position.x), Mathf.RoundToInt(_currentNode.Position.y), Mathf.RoundToInt(_currentNode.Position.z))) is null)
                {
                    continue;
                }
                
                if (_closeNodes.Exists(n => n.Position == _currentNode.Position + neighbour) || _openNodes.Exists(n => n.Position == _currentNode.Position + neighbour && n.F >= possibleF))
                {
                    continue;
                }
                
                _openNodes.Add(new AStarNode(_currentNode.Position + neighbour, _currentNode, _currentNode.G + neighbour.magnitude, Vector3.Distance(_currentNode.Position + neighbour, _objective.position)));
            }
            _closeNodes.Add(_currentNode);
            _openNodes.Remove(_currentNode);
            
            GetPath(_currentNode);
            
            _tilemapDebug.ClearAllTiles();
            
            foreach (var node in _openNodes)
            {
                _tilemapDebug.SetTile(
                    new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y), Mathf.RoundToInt(node.Position.z)), 
                    _openTile
                    );
            }
            foreach (var node in _closeNodes)
            {
                _tilemapDebug.SetTile(
                    new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y), Mathf.RoundToInt(node.Position.z)), 
                    _closedTile
                );
            }
            foreach (var node in _pathFound)
            {
                _tilemapDebug.SetTile(
                    new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y), Mathf.RoundToInt(node.Position.z)), 
                    _pathTile
                );
            }
        }

        public void GetPath(AStarNode destination)
        {
            if (destination == null)
            {
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
        
        // Start is called before the first frame update
        void Start()
        {
            FindPath();
        }

        // Update is called once per frame
        void Update()
        {
            FindPathIteration();
        }
    }
}

