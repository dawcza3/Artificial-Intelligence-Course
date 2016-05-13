using System;

namespace Sketching
{
    public class WyzarzanieRozkladNormalny
    {

        private static Random random;
        public static void Start()
        {
            try
            {
                random = new Random();

                double state = random.Next(100) - 100; // biorę jakąś liczbę z przedziału -100 100 
                double energy = Energy(state);

                double bestState = state;
                double bestEnergy = energy;

                double adjEnergy;
                double adjState;

                int iteration = 1;
                int maxIteration = 1000000;
                double currTemp = 1000.0;

                Console.WriteLine("\nInitial state: {0}", state);

                Console.WriteLine("Initial energy: " + energy.ToString("F2"));
                Console.WriteLine("\nEntering main Simulated Annealing loop");
                Console.WriteLine("Initial temperature = " + currTemp.ToString("F1") + "\n");

                while (iteration < maxIteration && currTemp > 0)
                {
                    // losujemy nowy stan oraz jego calkowity koszt 
                    adjState = AdjacentState(state, currTemp);
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

        static double NextGauss()
        {

            float v1, v2, s;
            do
            {
                v1 = 2.0f*(float)random.NextDouble() - 1.0f;
                v2 = 2.0f * (float)random.NextDouble() - 1.0f;
                s = v1 * v1 + v2 * v2;
            } while (s >= 1.0f || s == 0f);

            s = (float) Math.Sqrt((-2.0f * Math.Log(s)) / s);

            return v1 * s;
        }

        static double AdjacentState(double roz, double temp)
        {
            return roz + Math.Sqrt(temp) * NextGauss();
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
                if((Math.Exp(-Energy(tmpRoz)-Energy(roz))/temp>random.NextDouble()))
                    return tmpRoz;
                else
                    return roz;
            }
        }
    }
}