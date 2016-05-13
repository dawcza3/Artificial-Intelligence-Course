using System;
using System.Collections.Generic;
using System.Linq;

namespace PakowaniePlecaka
{
    public class PakowaniePlecaka
    {
        private const int B = 27;
        private static Random _random;
        private static List<ElementPlecaka> _state = new List<ElementPlecaka>();
        private static List<ElementPlecaka> _adjState = new List<ElementPlecaka>();
        private static List<ElementPlecaka> bestState = new List<ElementPlecaka>();

        private static int _iteration = 1;
        private static readonly int maxIteration = 1000;
        private static double currTemp = 100000.0;

        public static void Start()
        {
            try
            {
                _random = new Random();
                _state = AdjacentState(GenerateElements());
                Console.WriteLine("Stan początkowy plecaka");
                DisplayBackpackItems(_state);
                while (_iteration < maxIteration && currTemp > 0)
                {
                    _adjState = AdjacentState(GenerateElements());

                    // sprawdzamy czy jego koszt jest lepszy niz poprzedni 
                    if (AcceptanceProb(_adjState, bestState))
                    {
                        bestState = _adjState;
                        DisplayBackpackItems(bestState);
                    }
                    currTemp = currTemp / _iteration;
                    ++_iteration;
                }
                DisplayBackpackItems(bestState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        /*static List<ElementPlecaka> GenerujElementy()
        {
            List<ElementPlecaka> elementy = new List<ElementPlecaka>()
            {
                new ElementPlecaka()
                {
                    CzyZapakowane = true,
                    Wartość = 4,
                    Wielkość = 12
                },
                new ElementPlecaka()
                {
                    CzyZapakowane = true,
                    Wartość = 2,
                    Wielkość = 1
                },
                new ElementPlecaka()
                {
                    CzyZapakowane = true,
                    Wartość = 10,
                    Wielkość = 4
                },
                new ElementPlecaka()
                {
                    CzyZapakowane = true,
                    Wartość = 1,
                    Wielkość = 1
                },
                new ElementPlecaka()
                {
                    CzyZapakowane = true,
                    Wartość = 2,
                    Wielkość = 2
                },
            };
            return elementy;
        }
*/
        static List<ElementPlecaka> GenerateElements()
        {
            List<ElementPlecaka> elementy = new List<ElementPlecaka>();
            for (int i = 0; i < 10; i++)
            {
                elementy.Add(new ElementPlecaka()
                {
                    CzyZapakowane = true,
                    Wartość = i + 1,
                    Wielkość = i + 1
                });
            }

            return elementy;
        }

        static List<ElementPlecaka> AdjacentState(List<ElementPlecaka> list)
        {
            foreach (var elementPlecaka in list)
            {
                elementPlecaka.CzyZapakowane = _random.Next(0, 2) == 1;
            }
            return list;
        }

        static int TotalValueBackpack(List<ElementPlecaka> lista)
        {
            return lista.Where(elementPlecaka => elementPlecaka.CzyZapakowane).Sum(elementPlecaka => elementPlecaka.Wartość);
        }

        static bool AcceptanceProb(List<ElementPlecaka> lista, List<ElementPlecaka> best)
        {
            var suma = lista.Where(elementPlecaka => elementPlecaka.CzyZapakowane)
                .Sum(elementPlecaka => elementPlecaka.Wielkość);
            var suma2 = best.Where(elementPlecaka => elementPlecaka.CzyZapakowane)
                .Sum(elementPlecaka => elementPlecaka.Wielkość);

            var val = lista.Where(elementPlecaka => elementPlecaka.CzyZapakowane)
                .Sum(elementPlecaka => elementPlecaka.Wartość);
            var val2 = best.Where(elementPlecaka => elementPlecaka.CzyZapakowane)
                .Sum(elementPlecaka => elementPlecaka.Wartość);

            // nowe upakowanie ma większą wartość i nie przekracza wagi plecaka
            if (val > val2 && suma < B)
                return true;
            //waży za dużo
            if (suma > B)
                return false;
            // losowo akceptujemy gorsze rozwiazanie 
            //return _random.Next(0, 2) == 1; (1) 
            if (1/currTemp < _random.NextDouble()) //(2) 
                return true;
            else
                return false;
        }

        static void DisplayBackpackItems(List<ElementPlecaka> list)
        {
            foreach (var elementPlecaka in list)
            {
                if (elementPlecaka.CzyZapakowane)
                    Console.WriteLine("Cena : {0} Waga: {1}", elementPlecaka.Wartość, elementPlecaka.Wielkość);
            }
            Console.WriteLine("Całkowita wartość = {0}", TotalValueBackpack(list));

        }

    }
}
