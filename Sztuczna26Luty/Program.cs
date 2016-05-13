using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sztuczna26Luty
{
    class Program
    {
        public static void WriteTree(BinaryTreeNode<int> current,int count)
        {
            int Count = count;
            if (current != null)
            {
                Count++;
                WriteTree(current.Left,Count);
                for(int i=0;i< Count;i++)
                {
                    Console.Write("_");
                }
                Console.Write("{0}\n",current.Value);
                WriteTree(current.Right,Count);
            }
        }
        static void Main(string[] args)
        {
            /*
            Stack<int> myStack = new Stack<int>();
            Queue<int> myQueue = new Queue<int>();
            for (int i = 0; i < 10; i++)
                myStack.Push(i);
            for (int i = 0; i < 10; i++)
                myQueue.Enqueue(i);
            Console.WriteLine("Stack Elements:");
            for (int i = 0; i < 10; i++)
                Console.WriteLine("Element {0} is: {1}",i, myStack.Pop());
            Console.WriteLine("Queue Elements:");
            for (int i = 0; i < 10; i++)
                Console.WriteLine("Element {0} is: {1}", i, myQueue.Dequeue());
            Console.ReadKey();
            */
            BinaryTreeSearch<int> search = new BinaryTreeSearch<int>();
            BinaryTree<int> btree = new BinaryTree<int>();
            btree.Root = new BinaryTreeNode<int>(10);
            btree.Root.Left = new BinaryTreeNode<int>(6);
            btree.Root.Right = new BinaryTreeNode<int>(15);
         //   int ValueToAdd = 15;
         //   while (ValueToAdd != 0)
         //   {
         //       string input = Console.ReadLine();
         //       if (!int.TryParse(input, out ValueToAdd))
         //           Console.WriteLine("Wrong input");
         //       search.InsertKey(ValueToAdd, btree.Root);
         //       WriteTree(btree.Root,0);
         //   }
            WriteTree(btree.Root,0);
            Console.WriteLine("------------------------------------");
            search.RemoveKey(6, btree.Root);
            // search.InsertKey(7, btree.Root);
           // search.InsertKey(17, btree.Root);
            WriteTree(btree.Root,0);
            Console.ReadKey();


        }
    }
}
