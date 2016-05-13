using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sztuczna26Luty
{
    public class Node<T>
    {
        // Private member-variables
        private T data;
        private NodeList<T> neighbors = null;

        public Node() { }
        public Node(T data) : this(data, null) { }
        public Node(T data, NodeList<T> neighbors)
        {
            this.data = data;
            this.neighbors = neighbors;
        }
        // elementy potomne pierwszego stopnia
        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        protected NodeList<T> Neighbors
        {
            get
            {
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }

    }


    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node<T>));
        }

        public Node<T> FindByValue(T value) // do modyfikacji 
        {
            // search the list for the value
            foreach (Node<T> node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }

    public class BinaryTreeNode<T> : Node<T>
    {
        public BinaryTreeNode() : base() { }
        public BinaryTreeNode(T data) : base(data, null) { }
        public BinaryTreeNode(T data, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            base.Value = data;
            NodeList<T> children = new NodeList<T>(2);
            children[0] = left;
            children[1] = right;

            base.Neighbors = children;
        }

        public BinaryTreeNode<T> Left
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (BinaryTreeNode<T>)base.Neighbors[0];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>(2);

                base.Neighbors[0] = value;
            }
        }

        public BinaryTreeNode<T> Right
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (BinaryTreeNode<T>)base.Neighbors[1];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>(2);

                base.Neighbors[1] = value;
            }
        }

    }

    public class BinaryTree<T>
    {
        private BinaryTreeNode<T> root;

        public BinaryTree()
        {
            root = null;
        }

        public virtual void Clear()
        {
            root = null;
        }

        public BinaryTreeNode<T> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }
    }

    public class BinaryTreeSearch<T>
    {
        public void InsertKey(T key, BinaryTreeNode<T> root)
        {
            BinaryTreeNode<T> node = new BinaryTreeNode<T>(key);
            int result;

            BinaryTreeNode<T> current = root, parent = null;
            while (current != null)
            {
                result = Comparer<T>.Default.Compare(current.Value, key);
                if (result == 0)
                    return;
                else if (result > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Right;
                }
            }

            if (parent == null)
                root = node;
            else
            {
                result = result = Comparer<T>.Default.Compare(parent.Value, key);
                if (result > 0)
                    parent.Left = node;
                else
                    parent.Right = node;
            }
        }

        public void RemoveKey(T key, BinaryTreeNode<T> root)
        {
            int result;

            BinaryTreeNode<T> current = root, parent = null;
            while (current != null)
            {
                result = Comparer<T>.Default.Compare(current.Value, key);
                if (result == 0)
                {
                    /*
                    	if (DeleteNode->Left==NULL) or (DeleteNode->Right==NULL)
		y=DeleteNode
	else
		y=BST_FIND_SUCCESSOR(DeleteNode)
	if (y->Left != NULL)
		x=y->Left
	else
		x=y->Right
	if (x!=NULL)
		x->parent = y->parent
	if (y->parent == NULL)
		Tree->root = x
	else
		if (y == y->parent->Left)
			y->parent->Left = x
		else
			y->parent->Right = x
	if (y != DeleteNode)
		DeleteNode->Key = y->Key
		// Jeśli y ma inne pola, to je także należy skopiować
	return y
                    */
                    if(parent!=null)
                    {
                        //if(parent.Left.Value==key)
                        //{
                        //
                        //}
                        //else

                    }
                    
                }
                else if (result > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Right;
                }
            }

        }
    }


}
