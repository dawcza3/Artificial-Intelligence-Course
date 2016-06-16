using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using GAF.Operators;

namespace AlgorytmGenetycznyBinarnie
{
    class Program
    {
        private static List<Chromosome> GenerateChromosoms()
        {
            /*0000, 0001, 0010, 0011, 0100, 0101, 0110, 0111,
            1000, 1001, 1010, 1011, 1100, 1101, 1110, 1111.*/
            List<Chromosome> _chromosomes = new List<Chromosome>();
            _chromosomes.Add(new Chromosome("0000"));
            _chromosomes.Add(new Chromosome("0001"));
            _chromosomes.Add(new Chromosome("0010"));
            _chromosomes.Add(new Chromosome("0011"));
            _chromosomes.Add(new Chromosome("0100"));
            _chromosomes.Add(new Chromosome("0101"));
            _chromosomes.Add(new Chromosome("0111"));
            _chromosomes.Add(new Chromosome("1000"));
            _chromosomes.Add(new Chromosome("1001"));
            _chromosomes.Add(new Chromosome("1010"));
            _chromosomes.Add(new Chromosome("1011"));
            _chromosomes.Add(new Chromosome("1100"));
            _chromosomes.Add(new Chromosome("1101"));
            _chromosomes.Add(new Chromosome("1110"));
            _chromosomes.Add(new Chromosome("1111"));
            return _chromosomes;
        }

        private static void Main(string[] args)
        {
            const double crossoverProbability = 0.85;
            const double mutationProbability = 0.08;
            const int elitismPercentage = 5;


            //100 chromosomów o długości binarnej 44 i jakieś parametry
            var population = new Population(100, 4, false, false);

            /*var population=new Population();
            foreach (var generateChromosom in GenerateChromosoms())
            {
                population.Solutions.Add(generateChromosom);
            }
            */
            //create the genetic operators 
            var elite = new Elite(elitismPercentage);

            var crossover = new Crossover(crossoverProbability, true)
            {
                CrossoverType = CrossoverType.DoublePoint
            };

            var mutation = new BinaryMutate(mutationProbability, true);

            //create the GA itself 
            var ga = new GeneticAlgorithm(population, EvaluateFitness);

            //subscribe to the GAs Generation Complete event 
            ga.OnGenerationComplete += ga_OnGenerationComplete;

            //add the operators to the ga process pipeline 
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutation);

            //run the GA 
            ga.Run(TerminateAlgorithm);
            Console.ReadKey();
        }

        public static double EvaluateFitness(Chromosome chromosome)
        {
            double fitnessValue;
            if (chromosome != null)
            {
                var value = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count), 2);
                var functionValue = 2 * (value * value) + 1;

                //fitnessValue = 1 - ((double)functionValue / 1000); // najmniejsza wartość dla jakiego x
                fitnessValue = (double) functionValue/1000;
                if (value > 15 || fitnessValue < 0)
                {
                    fitnessValue = 200;
                }
                /*                //this is a range constant that is used to keep the x/y range between -100 and +100
                                var rangeConst = 200 / (System.Math.Pow(2, chromosome.Count / 2) - 1);

                                //get x and y from the solution
                                var x1 = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count / 2), 2);
                                var y1 = Convert.ToInt32(chromosome.ToBinaryString(chromosome.Count / 2, chromosome.Count / 2), 2);

                                //Adjust range to -100 to +100
                                var x = (x1 * rangeConst) - 100;
                                var y = (y1 * rangeConst) - 100;

                                //using binary F6 for fitness.
                                var temp1 = System.Math.Sin(System.Math.Sqrt(x * x + y * y));
                                var temp2 = 1 + 0.001 * (x * x + y * y);
                                var result = 0.5 + (temp1 * temp1 - 0.5) / (temp2 * temp2);

                                fitnessValue = 1 - result;*/
            }
            else
            {
                //chromosome is null
                throw new ArgumentNullException("chromosome", "The specified Chromosome is null.");
            }

            return fitnessValue;
        }

        public static bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 1000;
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {

            //get the best solution 
            var chromosome = e.Population.GetTop(1)[0];

            var value = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count),2);
            Console.WriteLine("Dla argumentu " + value + " funkcja osiągna najmniejsza wartość");
            /*            //decode chromosome

                        //get x and y from the solution 
                        var x1 = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count / 2), 2);
                        var y1 = Convert.ToInt32(chromosome.ToBinaryString(chromosome.Count / 2, chromosome.Count / 2), 2);

                        //Adjust range to -100 to +100 
                        var rangeConst = 200 / (System.Math.Pow(2, chromosome.Count / 2) - 1);
                        var x = (x1 * rangeConst) - 100;
                        var y = (y1 * rangeConst) - 100;

                        //display the X, Y and fitness of the best chromosome in this generation 
                        Console.WriteLine("x:{0} y:{1} Fitness{2}", x, y, e.Population.MaximumFitness);*/
        }
    }
}
