using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    public class Day6
    {
        public static async Task Part1()
        {
            var nums = (await File.ReadAllLinesAsync("../../../Inputs/Day6.txt"))
                .First()
                .Split(",")
                .Select(int.Parse)
                .ToList();

            for (int i = 0; i < 80; i++)
            {
                var count = nums.Count;
                for (int j = 0; j < count; j++)
                {
                    if (nums[j] == 0)
                    {
                        nums[j] = 6;
                        nums.Add(8);
                    }
                    else
                    {
                        nums[j]--;
                    }
                }
                //Console.WriteLine($"Day {i + 1}: [{string.Join(",", nums)}]");
            }

            Console.WriteLine($"Total: {nums.Count}");
        }



        public static async Task Part2()
        {
            var nums = (await File.ReadAllLinesAsync("../../../Inputs/Day6.txt"))
                .First()
                .Split(",")
                .Select(int.Parse)
                .ToList();
            var totalDays = 256 + 1; // 0th day is all 1's
            var solutions = new long[9, totalDays]; // 0..8 inclusive, 

            for(int day = 0; day < totalDays; day++)
            {
                for(int num = 0; num <= 8; num++)
                {
                    if (day <= num)
                    {
                        solutions[num, day] = 1;
                    }
                    else
                    {
                        solutions[num, day] = solutions[6, day - num - 1] + solutions[8, day - num - 1];
                    }
                }
            }

            var sum = nums.Select(n => solutions[n, totalDays - 1]).Sum();
            Console.WriteLine($"Total: {sum}");
        }
    }
}
