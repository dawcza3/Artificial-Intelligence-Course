using System;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Models;

namespace SieciBayes
{
    class Program
    {
        static void Main(string[] args)
        {

            Variable<bool> firstCoin = Variable.Bernoulli(0.5).Named("firstCoin");
            Variable<bool> secondCoin = Variable.Bernoulli(0.5).Named("secondCoin");

            Variable<bool> bothHeads = (firstCoin & secondCoin).Named("bothHeads");


            InferenceEngine ie = new InferenceEngine();
            if(!(ie.Algorithm is VariationalMessagePassing))
            {
                Console.WriteLine("Probability both coins are heads: " + ie.Infer(bothHeads));
                bothHeads.ObservedValue = false;
                Console.WriteLine("Probablity distribution over firstCoint"+ie.Infer(firstCoin));
            }
            else
                Console.WriteLine("This example does not run with variation Message Passing");

            Console.ReadKey();
        }
        
    }
}
