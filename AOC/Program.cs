using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AOC
{
    class Program
    {
        static void Main(string[] args)
        {
            Day3().GetAwaiter().GetResult();

            int x = 0;
        }

        private static async Task Day1()
        {
            var depths = (await File.ReadAllLinesAsync("../../../Inputs/Day1.txt"))
                .Select(s => int.Parse(s))
                .ToArray();

            var prev = depths[0];
            var increasedCount = 0;
            for (int i = 3; i < depths.Length; i++)
            {
                if (depths[i] > prev)
                {
                    increasedCount++;
                }

                prev = depths[i - 2];
            }

            Console.WriteLine($"Increased {increasedCount} times");
        }

        private static async Task Day2()
        {
            var instructions = (await File.ReadAllLinesAsync("../../../Inputs/Day2.txt"))
                .Select(s => s.Split(' '))
                .ToArray();

            var position = 0;
            var depth = 0;
            var aim = 0;

            foreach (var i in instructions)
            {
                var value = int.Parse(i[1]);

                switch (i[0])
                {
                    case "forward":
                        position += value;
                        depth += aim * value;
                        break;
                    case "up":
                        //depth -= value;
                        aim -= value;
                        break;
                    case "down":
                        //depth += value;
                        aim += value;
                        break;
                }
            }

            Console.WriteLine($"Position: {position}");
            Console.WriteLine($"Depth: {depth}");
            Console.WriteLine($"Aim: {aim}");
        }

        private static async Task Day3()
        {
            var reads = (await File.ReadAllLinesAsync("../../../Inputs/Day3.txt"));

            var j = 0;
            while(reads.Length > 1 && j < reads[0].Length)
            {
                var dict = new Dictionary<char, int>();

                for (int i = 0; i < reads.Length; i++)
                {
                    var digit = reads[i][j];

                    if (dict.ContainsKey(digit))
                    {
                        dict[digit]++;
                    }
                    else
                    {
                        dict[digit] = 1;
                    }
                }

                var orderedDict = dict.OrderBy(kv => kv.Value);
                var leastCommonDigit = orderedDict.First().Key;
                var mostCommonDigit = orderedDict.Last().Key;
                if (dict[leastCommonDigit] == dict[mostCommonDigit])
                {
                    //mostCommonDigit = '1';
                    leastCommonDigit = '0';
                }
                reads = reads.Where(r => r[j] == leastCommonDigit)
                    .ToArray();

                j++;
            }
            
            //Console.WriteLine($"Gamma:   {mostCommon} ({Convert.ToInt32(mostCommon, 2)})");
            //Console.WriteLine($"Epsilon: {leastCommon} ({Convert.ToInt32(leastCommon, 2)})");
            //Console.WriteLine($"Oxygen rating: {reads.First()} ({Convert.ToInt32(reads.First(), 2)})");
            Console.WriteLine($"CO2 rating: {reads.First()} ({Convert.ToInt32(reads.First(), 2)})");
        }
    }
}
