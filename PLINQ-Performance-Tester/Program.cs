using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PLINQ_Performance_Tester
{
    
    class Program
    {
        private static long[] array = new long[1_000_000];
        private static Random random = new Random();
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Parallel.ForEach(
                Partitioner.Create(0, array.Length),
                (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        array[i] = random.Next(1, 100_000);
                    }
                }
            );

            var sw1 = new Stopwatch();
            sw1.Start();
            var result = SumLinq(array);
            sw1.Stop();
            Console.WriteLine("Summing with Linq took {0} ms", sw1.ElapsedMilliseconds);
            
            var sw2 = new Stopwatch();
            sw2.Start();
            var result2 = SumPLinq(array);
            sw2.Stop();
            Console.WriteLine("Summing with PLinq took {0} ms", sw2.ElapsedMilliseconds);

            var sw3 = new Stopwatch();
            sw3.Start();
            var result3 = SumSeq(array);
            sw3.Stop();
            Console.WriteLine("Summing with foreach took {0} ms", sw3.ElapsedMilliseconds);

            
            
            Console.ReadLine();

        }

        private static long SumSeq(long[] input)
        {
            long temp = 0;
            foreach (var item in input)
            {
                temp += item;
            }

            return temp;
        }
        private static long SumLinq(long[] input)
        {
            return input.Sum();
        }

        private static long SumPLinq(long[] input)
        {
            return input.AsParallel().Sum();
        }
    }
}