using System;
using System.Collections;
using System.Collections.Generic;
using GAF;
using GAF.Extensions;
using GAF.Operators;

namespace PakowaniePlecakaGenetycznyAlgorytm
{
    class Program
    {
        private static Random rand = new Random();
        private const int dopuszczalnaWaga = 15;
        private static List<ElementPlecaka> GotowaLista;

        private static void Main(string[] args)
        {
            const int populationSize = 10;
            GotowaLista = CreateElementPlecakas();

            var population = new Population();

            // Mam jedną główną listę
            // tutaj tworzę tylko liste z wartościami true/false 
            for (var p = 0; p < populationSize; p++)
            {
                var elementPlecakas = CreateElementPlecakasGen();

                var chromosome = new Chromosome();
                foreach (var elementPlecaka in elementPlecakas)
                {
                    chromosome.Genes.Add(new Gene(elementPlecaka));
                }
                var rnd = GAF.Threading.RandomProvider.GetThreadRandom();
                chromosome.Genes.ShuffleFast(rnd);
                population.Solutions.Add(chromosome);
            }


            //create the elite operator
            var elite = new Elite(5);

            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                //CrossoverType = CrossoverType.SinglePoint
                CrossoverType = CrossoverType.SinglePoint
            };

            //create the mutation operator
            var mutate = new SwapMutate(0.02);
            

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;
            ga.OnInitialEvaluationComplete += Ga_OnInitialEvaluationComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //run the GA
            ga.Run(Terminate);

            Console.Read();
        }

        private static void Ga_OnInitialEvaluationComplete(object sender, GaEventArgs e)
        {
            throw new NotImplementedException();
        }

        static void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            // wyświetlam najlepsze rozwiązanie z elity 
            var fittest = e.Population.GetTop(1)[0];
            for (int index = 0; index < fittest.Genes.Count; index++)
            {
                var gene = fittest.Genes[index];
                if (((ElementPlecakaGen)gene.ObjectValue).CzyZapakowane)
                {
                    Console.WriteLine("Wartość: "+ GotowaLista[index].Wartość +" Waga: "+
                        GotowaLista[index].Wielkość);
                }
            }
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var całkowitaWartość = CalculateTotalTaskCost(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, Całkowita wartość: {2}", e.Generation, fittest.Fitness, całkowitaWartość);
        }

        private static List<ElementPlecaka> CreateElementPlecakas()
        {
            List<ElementPlecaka> elementy = new List<ElementPlecaka>();
            elementy.Add(new ElementPlecaka
            {
                CzyZapakowane = true,
                Wartość = 4,
                Wielkość = 12
            });
            elementy.Add(new ElementPlecaka
            {
                CzyZapakowane = true,
                Wartość = 2,
                Wielkość = 1
            });
            elementy.Add(new ElementPlecaka
            {
                CzyZapakowane = true,
                Wartość = 10,
                Wielkość = 4
            });
            elementy.Add(new ElementPlecaka
            {
                CzyZapakowane = true,
                Wartość = 2,
                Wielkość = 2
            });
            elementy.Add(new ElementPlecaka
            {
                CzyZapakowane = true,
                Wartość = 1,
                Wielkość = 1
            });
            return elementy;
        }

        private static IEnumerable CreateElementPlecakasGen()
        {
            List<ElementPlecakaGen> elementy = new List<ElementPlecakaGen>();
            for (int i = 0; i < 5; i++)
            {
                elementy.Add(new ElementPlecakaGen()
                {
                    CzyZapakowane = rand.Next(0, 2) == 1,
                });
            }
            return elementy;
        }


        public static double CalculateFitness(Chromosome chromosome)
        {
            var distanceToTravel = CalculateTotalTaskCost(chromosome);
            double waga = 0;
            for (int index = 0; index < chromosome.Genes.Count; index++)
            {
                var gene = chromosome.Genes[index];
                var currentElementPlecaka = (ElementPlecakaGen)gene.ObjectValue;

                if (currentElementPlecaka != null)
                {
                    if (currentElementPlecaka.CzyZapakowane)
                        waga += (double)GotowaLista[index].Wielkość;
                }
            }
            if (waga > dopuszczalnaWaga) return 0;
            //return waga / 100;
            return distanceToTravel / 100;
        }

        private static double CalculateTotalTaskCost(Chromosome chromosome)
        {
            int totalTaskCost = 0;
            //run through each ElementPlecaka in the order specified in the chromosome
            for (int index = 0; index < chromosome.Genes.Count; index++)
            {
                var gene = chromosome.Genes[index];
                var currentElementPlecaka = (ElementPlecakaGen)gene.ObjectValue;

                if (currentElementPlecaka != null)
                {
                    if (currentElementPlecaka.CzyZapakowane)
                        totalTaskCost += GotowaLista[index].Wartość;
                }
            }
            return totalTaskCost;
        }

        public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 400;
        }
    }

    /*    class Program
        {
            private static Random rand = new Random();
            private const int dopuszczalnaWaga = 27;
            private static List<ElementPlecaka> GotowaLista;

            private static void Main(string[] args)
            {
                const int populationSize = 10;
                GotowaLista = CreateElementPlecakas();

                var population = new Population();

                //create the chromosomes
                for (var p = 0; p < populationSize; p++)
                {
                    var ElementPlecakas = CreateElementPlecakasGen();

                    var chromosome = new Chromosome();
                    foreach (var ElementPlecaka in ElementPlecakas)
                    {
                        chromosome.Genes.Add(new Gene(ElementPlecaka));
                    }

                    var rnd = GAF.Threading.RandomProvider.GetThreadRandom();
                    chromosome.Genes.ShuffleFast(rnd);

                    population.Solutions.Add(chromosome);
                }

                //create the elite operator
                var elite = new Elite(5);

                //create the crossover operator
                var crossover = new Crossover(0.8)
                {
                    //CrossoverType = CrossoverType.SinglePoint
                    CrossoverType = CrossoverType.SinglePoint
                };

                //create the mutation operator
                var mutate = new SwapMutate(0.02);

                //create the GA
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                //hook up to some useful events
                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                //add the operators
                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutate);

                //run the GA
                ga.Run(Terminate);

                Console.Read();
            }

            static void ga_OnRunComplete(object sender, GaEventArgs e)
            {
                var fittest = e.Population.GetTop(1)[0];
                for (int index = 0; index < fittest.Genes.Count; index++)
                {
                    var gene = fittest.Genes[index];
                    if (((ElementPlecakaGen) gene.ObjectValue).CzyZapakowane)
                    {
                        Console.WriteLine("Task number: {0} Wartość: {1} Waga: {2}", index,
                            GotowaLista[index].Wartość,
                            GotowaLista[index].Wielkość);
                    }
                }
            }

            private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
            {
                var fittest = e.Population.GetTop(1)[0];
                var distanceToTravel = CalculateTotalTaskCost(fittest);
                Console.WriteLine("Generation: {0}, Fitness: {1}, Total Work Cost: {2}", e.Generation, fittest.Fitness, distanceToTravel);

            }

            private static List<ElementPlecaka> CreateElementPlecakas()
            {
                List<ElementPlecaka> elementy = new List<ElementPlecaka>();
                for (int i = 0; i < 10; i++)
                {
                    elementy.Add(new ElementPlecaka()
                    {
                        //CzyZapakowane = rand.Next(0, 2) == 1,
                        CzyZapakowane = true,
                        Wartość = i + 1,
                        Wielkość = i + 1
                    });
                }
                return elementy;
            }

            private static IEnumerable CreateElementPlecakasGen()
            {
                List<ElementPlecakaGen> elementy = new List<ElementPlecakaGen>();
                for (int i = 0; i < 10; i++)
                {
                    elementy.Add(new ElementPlecakaGen()
                    {
                        CzyZapakowane = rand.Next(0, 2) == 1,
                    });
                }
                return elementy;
            }


            public static double CalculateFitness(Chromosome chromosome)
            {
                var distanceToTravel = CalculateTotalTaskCost(chromosome);
                double waga = 0;
                for (int index = 0; index < chromosome.Genes.Count; index++)
                {
                    var gene = chromosome.Genes[index];
                    var currentElementPlecaka = (ElementPlecakaGen)gene.ObjectValue;

                    if (currentElementPlecaka != null)
                    {
                        if (currentElementPlecaka.CzyZapakowane)
                            waga += (double) GotowaLista[index].Wielkość;
                    }
                }
                if (waga > dopuszczalnaWaga) return 0;
                //return waga / 100;
                return distanceToTravel/100;
            }

            private static double CalculateTotalTaskCost(Chromosome chromosome)
            {
                int totalTaskCost = 0;
                //run through each ElementPlecaka in the order specified in the chromosome
                for (int index = 0; index < chromosome.Genes.Count; index++)
                {
                    var gene = chromosome.Genes[index];
                    var currentElementPlecaka = (ElementPlecakaGen)gene.ObjectValue;

                    if (currentElementPlecaka != null)
                    {
                        if (currentElementPlecaka.CzyZapakowane)
                            totalTaskCost += GotowaLista[index].Wartość;
                    }
                }
                return totalTaskCost;
            }

            public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
            {
                return currentGeneration > 400;
            }
        }*/
}
