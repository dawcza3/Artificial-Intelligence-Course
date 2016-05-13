using System;

namespace Sketching
{
    public class WyzarzanieRozkladJednostajny
    {

        private static Random random;
        public static void Start()
        {
            try
            {
                random = new Random();

                
                double val1 = random.Next(1000)*-1;
                double val2 = random.Next(1000);

                int iteration = 1;
                int maxIteration = 1000000;
                double currTemp = 1000.0;

                double state = AdjacentState(ref val1, ref val2,currTemp);
                double energy = Energy(state);

                double bestState = state;
                double bestEnergy = energy;

                double adjEnergy;
                double adjState;

               
                Console.WriteLine("\nInitial state: {0}", state);

                Console.WriteLine("Initial energy: " + energy.ToString("F2"));
                Console.WriteLine("\nEntering main Simulated Annealing loop");
                Console.WriteLine("Initial temperature = " + currTemp.ToString("F1") + "\n");

                while (iteration < maxIteration && currTemp > 0)
                {
                    // losujemy nowy stan oraz jego calkowity koszt 
                    adjState = AdjacentState(ref val1,ref val2,currTemp);
                    adjEnergy = Energy(adjState);

                    // sprawdzamy czy jego koszt jest lepszy niz poprzedni 
                    if (adjEnergy < bestEnergy)
                    {
                        bestState = adjState;
                        bestEnergy = adjEnergy;
                        Console.WriteLine("New best state found: {0}", bestState);
                        Console.WriteLine("Energy = " + bestEnergy.ToString("F2") + "\n");
                    }

                    state = AcceptanceProb(adjState, currTemp, state);
                    energy = Energy(state);

                    currTemp = currTemp / iteration;
                    ++iteration;

                }
                // Display best state found.
                Console.Write("Temperature has cooled to (almost) zero ");
                Console.WriteLine("at iteration " + iteration);
                Console.WriteLine("Simulated Annealing loop complete");
                Console.WriteLine("\nBest state found: {0}", bestState);
                Console.WriteLine("Best energy = " + bestEnergy.ToString("F2") + "\n");
                Console.WriteLine("\nEnd Simulated Annealing demo\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        } // Main

        static double AdjacentState(ref double val1,ref double val2,double temp)
        {
            double result = (val1 + val2)/2;
            val1 = val1/temp;
            val2 = val2/temp;

            return result;
        }

        static double Energy(double state)
        {
            return state * state;
        }

        static double AcceptanceProb(double tmpRoz, double temp,
            double roz)
        {
            if (Energy(roz) > Energy(tmpRoz))
                return tmpRoz;
            else
            {
                if ((Math.Exp(-Energy(tmpRoz) - Energy(roz)) / temp > random.NextDouble()))
                    return tmpRoz;
                else
                    return roz;
            }
        }
    }
}