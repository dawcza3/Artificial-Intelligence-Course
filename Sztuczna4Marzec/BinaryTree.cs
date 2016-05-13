using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sztuczna4Marzec
{
    public class BinaryTree<T>
    {
        public BinaryTree<T> Left, Right, Parrent;
        public T Data;
        public BinaryTree()
        {
            this.Left = null;
            this.Right = null;
            this.Parrent = null;
        }
    }

    public class BinaryTreeOperations<T>
    {
        public void RemoveValue(T value, ref BinaryTree<T> myTree)
        {
            if (myTree == null) return;
            // Szukamy danego wezla drzewa
            int result = Comparer<T>.Default.Compare(value, myTree.Data);
            if (result > 0)
                RemoveValue(value, ref myTree.Right);
            else if (result < 0)
                RemoveValue(value, ref myTree.Left);
            else // znalazlem interesujacy mnie wezel
            {
                if (myTree.Left == null && myTree.Right == null) // nie mamy dzieci (działa) 
                {
                    myTree = null;
                    return;
                }
                else if (myTree.Left == null ^ myTree.Right == null) // mamy jedno dziecko 
                {
                    if (myTree.Left == null)
                    {
                        result = Comparer<T>.Default.Compare(myTree.Parrent.Right.Data, myTree.Data); // Dziala
                        if (result == 0)
                        {
                            myTree.Right.Parrent = myTree.Parrent;
                            myTree.Parrent.Right = myTree.Right;
                        }
                        else
                        {
                            myTree.Right.Parrent = myTree.Parrent;
                            myTree.Parrent.Left = myTree.Right;
                        }
                    }
                    else // działa
                    {
                        result = Comparer<T>.Default.Compare(myTree.Parrent.Right.Data, myTree.Data);
                        if (result == 0)
                        {
                            myTree.Left.Parrent = myTree.Parrent;
                            myTree.Parrent.Right = myTree.Left;
                        }
                        else
                        {
                            myTree.Left.Parrent = myTree.Parrent;
                            myTree.Parrent.Left = myTree.Left;
                        }
                    }
                }
                else //mamy obydwa dzieci (wg mnie działa prawidłowo) 
                {
                    FindNextValue(out value, ref myTree);
                    myTree.Data = value;
                    RemoveValue(myTree.Data, ref myTree.Right);
                }
            }
        }
        

        private void FindParrent(out T value,ref BinaryTree<T> myTree)
        {
            if (myTree.Parrent != null)
            {
                int result = Comparer<T>.Default.Compare(myTree.Parrent.Data, myTree.Data);
                if (result > 0)
                {
                    value = myTree.Parrent.Data;
                    return;
                }
                else
                    FindParrent(out value, ref myTree.Parrent);
            }
            else
                throw new NotImplementedException("Węzeł nie posiada następnika?");
        }

        private void FindNextValue(out T value, ref BinaryTree<T> myTree)
        {
            if (myTree.Right != null)
            {
                FindMinimumValue(out value, ref myTree.Right);
            }
            else
            {
                FindParrent(out value, ref myTree); // w przypadku węzła z dwoma dziećmi
                                                    // tak naprawdę ten kod jest zbyteczny
            }
        }

        private void FindMinimumValue(out T value, ref BinaryTree<T> myTree)
        {
            if (myTree.Left != null)
            {
                FindMinimumValue(out value, ref myTree.Left);
            }
            else
            {
                value = myTree.Data;
            }
        }

        public void AddValue(T value, ref BinaryTree<T> myTree, ref BinaryTree<T> parrent)
        {
            if (myTree == null)
            {
                myTree = new BinaryTree<T>();
                myTree.Right = myTree.Left = null;
                myTree.Parrent = parrent;
                myTree.Data = value;
            }
            else
            {
                int result = Comparer<T>.Default.Compare(value, myTree.Data);
                if (result == 0)
                    return; // Nie wstawiam tych samych wartości drugi raz 
                else if (result > 0)
                {
                    AddValue(value, ref myTree.Right, ref myTree);
                }
                else if (result < 0)
                {
                    AddValue(value, ref myTree.Left, ref myTree);
                }
            }

        }

        public void WriteTree(BinaryTree<T> current, int count)
        {
            int Count = count;
            if (current != null)
            {
                Count++;
                WriteTree(current.Left, Count);
                for (int i = 0; i < Count; i++)
                {
                    Console.Write("_");
                }
                if (current.Parrent != null) Console.Write("{0} ({1}) \n", current.Data, current.Parrent.Data);
                else
                    Console.Write("{0} \n", current.Data);
                WriteTree(current.Right, Count);
            }
        }
    }

}
