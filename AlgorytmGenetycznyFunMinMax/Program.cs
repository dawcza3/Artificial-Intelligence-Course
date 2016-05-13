using System;
using System.Collections.Generic;
using GAF;
using GAF.Extensions;
using GAF.Operators;

namespace AlgorytmGenetycznyFunMinMax
{
    class Program
    {
        static void Main(string[] args)
        {
            const int populationSize = 100;

            var points = CreatePoints();

            var population = new Population();

            //create the chromosomes
            for (var p = 0; p < populationSize; p++)
            {

                var chromosome = new Chromosome();
                foreach (var point in points)
                {
                    chromosome.Genes.Add(new Gene(point));
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
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            //create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            ga.OnGenerationComplete += Ga_OnGenerationComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //run the GA
            ga.Run(TerminateAlgorithm);

            Console.Read();
        }

        public static double CalculateFitness(Chromosome chromosome)
        {
            var distanceToMinGlobal = CalculateDistance(chromosome);
            return 1 - distanceToMinGlobal / 10000;
        }
        
        private static double CalculateDistance(Chromosome chromosome)
        {

            var distanceToMinGlobal = 0.0;
            Point previousPoint = null;

            foreach (var gene in chromosome.Genes)
            {
                var currentPoint = (Point)gene.ObjectValue;

                if (previousPoint != null)
                {
                    var distance = previousPoint.GetDistanceFromPosition(0,0);
                   // var distance = previousPoint.GetDistanceFromPosition(currentPoint.X,currentPoint.Y);
                    distanceToMinGlobal += distance;
                }

                previousPoint = currentPoint;
            }

            return distanceToMinGlobal;
        }
    
        private static void Ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var distanceToTravel = CalculateDistance(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, Distance: {2}", e.Generation, fittest.Fitness, distanceToTravel);
        }

        static List<Point> CreatePoints()
        {
            Random rand=new Random();
            List<Point> list=new List<Point>();
            for (int i = 0; i < 10; i++)
            {
                double x,y;
                x = rand.NextDouble()*200-100;
                y = rand.NextDouble()*200-100;
                list.Add(new Point(){X=x,Y=y});
            }
            return list;
        }

        public static bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 4000;
        }

    }
}
