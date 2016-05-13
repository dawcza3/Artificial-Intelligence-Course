using System;

namespace Sketching
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Rozkład normalny");
            WyzarzanieRozkladNormalny.Start();

            Console.WriteLine("Rozkład jednostajny");
            WyzarzanieRozkladJednostajny.Start();
        }
    }
}
