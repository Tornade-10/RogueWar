

using UnityEngine;

namespace Workshop
{
    public class AStarNode
    {
        private Vector3 _position;
        private AStarNode _parent;

        public float G; //Displacement Cost
        public float H; //Heuristic score

        public float F => G + H;

        public Vector3 Position
        {
            get => _position;
        }

        public AStarNode Parent => _parent;

        public AStarNode(Vector3 position, AStarNode parent, float g, float h)
        {
            _position = position;
            _parent = parent;
            G = g;
            H = h;
        }
    }
}