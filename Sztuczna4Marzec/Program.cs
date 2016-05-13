using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sztuczna4Marzec
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryTreeOperations<int> Operations = new BinaryTreeOperations<int>();
            BinaryTree<int> MyTree = new BinaryTree<int>();
            MyTree.Data = 10;
            Operations.AddValue(15, ref MyTree,ref MyTree);
            Operations.AddValue(5, ref MyTree,ref MyTree);
            Operations.AddValue(3, ref MyTree, ref MyTree);
            Operations.AddValue(7, ref MyTree, ref MyTree);
            Operations.AddValue(20, ref MyTree, ref MyTree);
            Operations.AddValue(13, ref MyTree, ref MyTree);
            Operations.WriteTree(MyTree, 0);
            Console.WriteLine("Przypadek z dwójką dzieci (wywalam 10)");
            Operations.RemoveValue(10, ref MyTree);
            Operations.WriteTree(MyTree, 0);
            Console.WriteLine("Przypadek z jednym dzieckiem (wywalam 15)");
            Operations.RemoveValue(15, ref MyTree);
            Operations.WriteTree(MyTree, 0);
            Console.WriteLine("Przypadek z brakiem dzieci (wywalam 20)");
            Operations.RemoveValue(20, ref MyTree);
            Operations.WriteTree(MyTree, 0);
            Console.ReadKey();
        }
    }
}
