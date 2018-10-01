using System;
using System.Linq;
using System.Threading;

namespace _01_EvenNumsThread
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numsData = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var thread = new Thread(() => PrintEvenNumbers(numsData[0], numsData[1]));

            thread.Start();
            thread.Join();

            Console.WriteLine("Thread finished work.");
        }

        private static void PrintEvenNumbers(int startNum, int endNum)
        {
            for (int i = startNum; i <= endNum; i++)
            {
                if(i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}
