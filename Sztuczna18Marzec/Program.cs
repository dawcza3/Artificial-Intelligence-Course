using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sztuczna18Marzec
{
    class Program
    {
        class Vertex
        {
            public Vertex(int number, double dist, int prevVertex)
            {
                Number = number;
                Dist = dist;
                PrevVertex = prevVertex;
            }
            public int Number { get; }
            public double Dist { get; set; }
            public int PrevVertex { get; set; }
        }

        static int FindMinDistance(List<Vertex> list, List<int> unvisitedVertext)
        {
            var number = 0;
            double distance = Double.PositiveInfinity;
            foreach (var vertex in list)
            {
                if (vertex.Dist < distance && unvisitedVertext.Contains(vertex.Number))
                {
                    distance = vertex.Dist;
                    number = vertex.Number;
                }
            }
            unvisitedVertext.Remove(number);
            return number;
        }

        static void Dijkstry(EdgeWeightedDigraph graph, int startNumber)
        {
            // Inicjalizacja 
            List<int> unvisitedVertex = new List<int>(graph.V());
            for (int i = 0; i < graph.V(); i++)
                unvisitedVertex.Add(i);

            List<Vertex> list = new List<Vertex>(graph.V());
            for (int i = 0; i < graph.V(); i++)
            {
                list.Add(new Vertex(i, Double.PositiveInfinity, -1));
            }
            list[startNumber].Dist = 0;
            
            // Główny program
            while (unvisitedVertex.Count != 0)
            {
                var numberVertex = FindMinDistance(list, unvisitedVertex);
                IEnumerable<DirectedEdge> neighbors = graph.Adj(numberVertex);
                foreach (var directedEdge in neighbors)
                {
                    if (unvisitedVertex.Contains(directedEdge.To()) &&
                        (list[directedEdge.To()].Dist > list[numberVertex].Dist + directedEdge.Weight()))
                    {
                        list[directedEdge.To()].Dist = list[numberVertex].Dist + directedEdge.Weight();
                        list[directedEdge.To()].PrevVertex = numberVertex;
                    }
                }
            }

            // Wypisywanie

            Stack<int> myStack = new Stack<int>();
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write("{0} : ",i);
                for(int j=i;j>-1;j=list[j].PrevVertex) myStack.Push(j);
                while(myStack.Count!=0) Console.Write("{0} ",myStack.Pop());
                Console.Write("koszt : {0}\n",list[i].Dist);
            }
        }

        static void Main(string[] args)
        {
            EdgeWeightedDigraph graph = new EdgeWeightedDigraph(6);
            List<DirectedEdge> list = new List<DirectedEdge>();
            list.Add(new DirectedEdge(0, 1, 3));
            list.Add(new DirectedEdge(0, 4, 3));

            list.Add(new DirectedEdge(1, 2, 1));

            list.Add(new DirectedEdge(2, 3, 3));
            list.Add(new DirectedEdge(2, 5, 1));

            list.Add(new DirectedEdge(3, 1, 3));

            list.Add(new DirectedEdge(4, 5, 2));

            list.Add(new DirectedEdge(5, 3, 1));
            list.Add(new DirectedEdge(5, 0, 6));



            foreach (var directedEdge in list)
            {
                graph.AddEdge(directedEdge);

            }

            IEnumerable<DirectedEdge> edges = graph.Adj(0);
            foreach (var directedEdge in edges)
            {
                Console.WriteLine(directedEdge);
            }

            graph.ShowAll();
            Console.WriteLine("Krawędzi : {0} , Węzłów:  {1}", graph.E(), graph.V());
            Dijkstry(graph, 0);
            Console.ReadKey();
        }
    }
}
