using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nodes
{
    public class Tree : MonoBehaviour
    {

        [SerializeField] private Transform _root;
        
        private Node _tree;
        
        // Start is called before the first frame update
        void Start()
        {
            _tree = new Node(_root.name);
            _tree.FillTree(_root);
            _tree.Print();
        }
    }
}

