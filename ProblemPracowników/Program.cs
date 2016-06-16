using System;
using System.Collections;
using System.Collections.Generic;
using GAF;
using GAF.Operators;

namespace ProblemPracowników
{
    class Program
    {
        private static void Main(string[] args)
        {
            const int populationSize = 10;

            var population = new Population();

            //create the chromosomes
            for (var p = 0; p < populationSize; p++)
            {
                var employees = CreateEmployees();

                var chromosome = new Chromosome();
                foreach (var Employee in employees)
                {
                    chromosome.Genes.Add(new Gene(Employee));
                }

                //var rnd = GAF.Threading.RandomProvider.GetThreadRandom();
                //chromosome.Genes.ShuffleFast(rnd);

                population.Solutions.Add(chromosome);
            }

            //create the elite operator
            var elite = new Elite(5);

            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                //CrossoverType = CrossoverType.SinglePoint
                CrossoverType = CrossoverType.DoublePoint
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
                Console.WriteLine("Task number: {0} Emplyee number: {1} Work cost: {2}",index,((Employee) gene.ObjectValue).Id,
                    ((Employee)gene.ObjectValue).TaskDictionary[index]);
            }
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var distanceToTravel = CalculateTotalTaskCost(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, Total Work Cost: {2}", e.Generation, fittest.Fitness, distanceToTravel);

        }

        private static IEnumerable CreateEmployees()
        {
            Random rand=new Random();

            var employees = new List<Employee>();
           

            employees.Add(new Employee(0, new Dictionary<int, double>()
            {
                {0, 5},
                {1, 4},
                {2, 3},
                {3, 5}
            }
                ));
            employees.Add(new Employee(1, new Dictionary<int, double>()
            {
                {0, 7},
                {1, 8},
                {2, 7},
                {3, Double.PositiveInfinity}
            }
                ));
            employees.Add(new Employee(2, new Dictionary<int, double>()
            {
                {0, 1},
                {1, 12},
                {2,5},
                {3,7}
            }
                ));
            employees.Add(new Employee(3, new Dictionary<int, double>()
            {
                {0, 4},
                {1, Double.PositiveInfinity},
                {2, 1},
                {3, 2}
            }
                ));

            //return employees;

            List<Employee> employerss=new List<Employee>();

            while (employees.Count != 0)
            {
                int value = rand.Next(0, 4);
                if (value < employees.Count)
                {
                    employerss.Add(employees[value]);
                    employees.Remove(employees[value]);
                }
            }
              
            return employerss;
        }
/*
 private static IEnumerable CreateEmployees()
        {
            var employees = new List<Employee>();
            employees.Add(new Employee(0, new Dictionary<int, double>()
            {
                {0, 5},
                {1, Double.PositiveInfinity},
                {2, Double.PositiveInfinity},
                {3, 3}
            }
                ));
            employees.Add(new Employee(1, new Dictionary<int, double>()
            {
                {0, Double.PositiveInfinity},
                {1, 0.1},
                {2, Double.PositiveInfinity},
                {3, 1}
            }
                ));
            employees.Add(new Employee(2, new Dictionary<int, double>()
            {
                {0, 7},
                {1, 0.5},
                {2,5},
                {3,Double.PositiveInfinity}
            }
                ));
            employees.Add(new Employee(3, new Dictionary<int, double>()
            {
                {0, Double.PositiveInfinity},
                {1, Double.PositiveInfinity},
                {2, Double.PositiveInfinity},
                {3, 0.1}
            }
                ));

            return employees;
        }
*/

        public static double CalculateFitness(Chromosome chromosome)
        {
            var distanceToTravel = CalculateTotalTaskCost(chromosome);
            return 1 - distanceToTravel / 10000;
        }

        private static double CalculateTotalTaskCost(Chromosome chromosome)
        {
            var totalTaskCost = 0.0;
            //run through each Employee in the order specified in the chromosome
            for (int index = 0; index < chromosome.Genes.Count; index++)
            {
                var gene = chromosome.Genes[index];
                var currentEmployee = (Employee) gene.ObjectValue;

                if (currentEmployee != null)
                {
                    var cost = currentEmployee.TaskDictionary[index];
                    totalTaskCost += cost;
                }
            }
            return totalTaskCost;
        }

        public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 400;
        }
    }
}
