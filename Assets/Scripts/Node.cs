using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace Nodes
{
    public class Node
    {
        private string _name;

        private List<Node> _children = new List<Node>();

        public Node(string name)
        {
            _name = name;
        }

        public void FillTree(Transform _root)
        {
            AddNode(_root);
        }
        
        public void AddNode(Transform node)
        {
            Node Parent = new Node(node.name);
            _children.Add(Parent);
            
            var children = node.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child != node && child.parent == node)
                {
                    Parent.AddNode(child);
                }
            }
        }

        public void DFSOrder()
        {
            Stack<Node> tempStack = new Stack<Node>();
            string treeTrace = "";
            
            tempStack.Push(this);

            do
            {

                Node tn = tempStack.Pop();
                
                Debug.Log(tn._name);

                foreach (var child in tn._children)
                {
                    tempStack.Push(child);
                }

            } while (tempStack.Count > 0);
        }
        
        public void BFSOrder()
        {
            Queue<Node> tempQueue = new Queue<Node>();
            string treeTrace = "";
            
            tempQueue.Enqueue(this);

            do
            {

                Node tn = tempQueue.Dequeue();
                
                Debug.Log(tn._name);

                foreach (var child in tn._children)
                {
                    tempQueue.Enqueue(child);
                }

            } while (tempQueue.Count > 0);
        }

        public void Print()
        {
            Debug.Log(_name);
            foreach (Node child in _children)
            {
                child.Print();
            }
        }
    }
}