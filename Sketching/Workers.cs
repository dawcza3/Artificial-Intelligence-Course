using System;

namespace Sketching
{
    public class Workers
    {
        private static Random random;
        public static void Start()
        {
            try
            {

                // Set up problem data.
                // Create random state.
                // Set up SA variables for temperature and cooling rate.

                Console.WriteLine("\nBegin Simulated Annealing demo\n");
                Console.WriteLine("Worker 0 can do Task 0 (7.5) Task 1 (3.5) Task 2 (2.5)");
                Console.WriteLine("Worker 1 can do Task 1 (1.5) Task 2 (4.5) Task 3 (3.5)");
                Console.WriteLine("Worker 2 can do Task 2 (3.5) Task 3 (5.5) Task 4 (3.5)");
                Console.WriteLine("Worker 3 can do Task 3 (6.5) Task 4 (1.5) Task 5 (4.5)");
                Console.WriteLine("Worker 4 can do Task 0 (2.5) Task 4 (2.5) Task 5 (2.5)");

                random = new Random(0);
                int numWorkers = 5;
                int numTasks = 6;
                double[][] problemData = MakeProblemData(numWorkers, numTasks);

                /*State is represented as an int array where the array index represents a task
                 *and the value in the array at the index represents the worker assigned to the task.
                 * index-task value-worker 
                 *  */
                int[] state = RandomState(problemData);

                /*Helper method Energy computes the total time required by its state parameter,
                 *taking into account the 3.5-hour penalty for retooling every time a worker does an additional task.*/
                double energy = Energy(state, problemData);
                int[] bestState = state;
                double bestEnergy = energy;
                int[] adjState;
                double adjEnergy;

                int iteration = 0;
                int maxIteration = 1000000;
                double currTemp = 10000.0;
                /*Variable alpha represents the cooling rate, or a factor
                 *  that determines how the temperature variable decreases, or cools,
                 *  each time through the processing loop.*/
                double alpha = 0.995;

                Console.WriteLine("\nInitial state:");
                Display(state);
                Console.WriteLine("Initial energy: " + energy.ToString("F2"));
                Console.WriteLine("\nEntering main Simulated Annealing loop");
                Console.WriteLine("Initial temperature = " + currTemp.ToString("F1") + "\n");

                while (iteration < maxIteration && currTemp > 0.0001)
                {
                    adjState = AdjacentState(state, problemData);
                    adjEnergy = Energy(adjState, problemData);
                    if (adjEnergy < bestEnergy)
                    {
                        bestState = adjState;
                        bestEnergy = adjEnergy;
                        Console.WriteLine("New best state found:");
                        Display(bestState);
                        Console.WriteLine("Energy = " + bestEnergy.ToString("F2") + "\n");
                    }

                    /*
                     he loop finishes up by first generating a random value p greater than or equal to 0.0
                     and strictly less than 1.0 and comparing
                     that value against the return from helper method AcceptanceProb.
                                         */
                    double p = random.NextDouble();
                    if (AcceptanceProb(energy, adjEnergy, currTemp) > p)
                    {
                        state = adjState;
                        energy = adjEnergy;
                    }
                    currTemp = currTemp * alpha;
                    ++iteration;

                }
                // Display best state found.
                Console.Write("Temperature has cooled to (almost) zero ");
                Console.WriteLine("at iteration " + iteration);
                Console.WriteLine("Simulated Annealing loop complete");
                Console.WriteLine("\nBest state found:");
                Display(bestState);
                Console.WriteLine("Best energy = " + bestEnergy.ToString("F2") + "\n");
                Interpret(bestState, problemData);
                Console.WriteLine("\nEnd Simulated Annealing demo\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        } // Main
        static double[][] MakeProblemData(int numWorkers, int numTasks)
        {
            double[][] result = new double[numWorkers][];
            for (int w = 0; w < result.Length; ++w)
                result[w] = new double[numTasks];
            result[0][0] = 7.5; result[0][1] = 3.5; result[0][2] = 2.5;
            result[1][1] = 1.5; result[1][2] = 4.5; result[1][3] = 3.5;
            result[2][2] = 3.5; result[2][3] = 5.5; result[2][4] = 3.5;
            result[3][3] = 6.5; result[3][4] = 1.5; result[3][5] = 4.5;
            result[4][0] = 2.5; result[4][4] = 2.5; result[4][5] = 2.5;
            return result;
        }
        /*
         Method AdjacentState starts with a given state, then selects a random task and then selects
         a random worker who can do that task. Note that this is a pretty crude approach;
         it doesn’t check to see if the randomly generated new worker is the same as the current worker,
         so the return state might be the same as the current state. Depending on the nature of the problem
         being targeted by an SA algorithm, you might want to insert code logic that ensures
         an adjacent state is different from the current state.
                     */
        static int[] RandomState(double[][] problemData)
        {
            int numWorkers = problemData.Length;
            int numTasks = problemData[0].Length;
            int[] state = new int[numTasks]; //index - task value - worker
            for (int t = 0; t < numTasks; ++t)
            {
                int w = random.Next(0, numWorkers);
                while (problemData[w][t] == 0.0)
                {
                    ++w; if (w > numWorkers - 1) w = 0;
                }
                state[t] = w;
            }
            return state;
        }

        static int[] AdjacentState(int[] currState, double[][] problemData)
        {
            int numWorkers = problemData.Length;
            int numTasks = problemData[0].Length;
            int[] state = new int[numTasks];
            int task = random.Next(0, numTasks);
            int worker = random.Next(0, numWorkers);
            while (problemData[worker][task] == 0.0)
            {
                ++worker; if (worker > numWorkers - 1) worker = 0;
            }
            currState.CopyTo(state, 0);
            state[task] = worker;
            return state;
        }

        static double Energy(int[] state, double[][] problemData)
        {
            double result = 0.0;
            for (int t = 0; t < state.Length; ++t)
            {
                int worker = state[t];
                double time = problemData[worker][t];
                result += time;
            }
            int numWorkers = problemData.Length;
            int[] numJobs = new int[numWorkers];
            for (int t = 0; t < state.Length; ++t)
            {
                int worker = state[t];
                ++numJobs[worker];
                if (numJobs[worker] > 1) result += 3.50;
            }
            return result;
        }

        static double AcceptanceProb(double energy, double adjEnergy,
            double currTemp)
        {
            if (adjEnergy < energy)
                return 1.0;
            else
                return Math.Exp((energy - adjEnergy) / currTemp);
        }


        static void Display(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                    Console.Write(matrix[i][j].ToString("F2") + " ");
                Console.WriteLine("");
            }
        }
        static void Display(int[] vector)
        {
            for (int i = 0; i < vector.Length; ++i)
                Console.Write(vector[i] + " ");
            Console.WriteLine("");
        }
        static void Interpret(int[] state, double[][] problemData)
        {
            for (int t = 0; t < state.Length; ++t)
            { // task
                int w = state[t]; // worker
                Console.Write("Task [" + t + "] assigned to worker ");
                Console.WriteLine(w + ", " + problemData[w][t].ToString("F2"));
            }
        }
    }
}