using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPULib
{

    class Program
    {
        
        public static void Main(string[] args)
        {
            Perceptron p = new Perceptron(2);
            float[] inputs = { -1.0F, 0.5F };
            int guess = p.Guess(inputs);

            var elapsedMs = Example.RunExample();

            Console.WriteLine($"Guess : {guess}");
            Console.WriteLine($"Elapsed : {elapsedMs}");
        }
    }
}
