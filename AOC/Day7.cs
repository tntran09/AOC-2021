using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    public class Day7
    {
        public static async Task Part1()
        {
            var nums = (await File.ReadAllLinesAsync("../../../Inputs/Day7.txt"))
                .First()
                .Split(",")
                .Select(int.Parse)
                .ToArray();
            var length = nums.Length;

            var sorted = nums.OrderBy(x => x).ToArray();
            var median = length % 2 == 0
                ? (sorted[length / 2] + sorted[length / 2 - 1]) / 2.0
                : sorted[length / 2];

            Console.WriteLine($"Median: {median}");
            Console.WriteLine($"Sum of deltas: {nums.Sum(x => Math.Abs(x - median))}");
        }

        public static async Task Part2()
        {
            var nums = (await File.ReadAllLinesAsync("../../../Inputs/Day7.txt"))
                .First()
                .Split(",")
                .Select(int.Parse)
                .ToArray();
            var max = nums.Max();

            var fuelCosts = new int[max + 1];
            fuelCosts[0] = 0;
            for (var i = 1; i < fuelCosts.Length; i++)
            {
                fuelCosts[i] = i + fuelCosts[i - 1];
            }

            //var sorted = nums.OrderBy(x => x).ToArray();
            //var mean = (int)Math.Round(nums.Average());
            //var median = length % 2 == 0
            //    ? (int)((sorted[length / 2] + sorted[length / 2 - 1]) / 2.0)
            //    : sorted[length / 2];
            //var midpoint = (int) (Math.Round((double) (nums.Min() + nums.Max())) / 2.0);

            var minDelta = -1;
            for (var i = 0; i < max; i++)
            {
                var costFromHere = nums.Sum(x => fuelCosts[Math.Abs(x - i)]);
                minDelta = minDelta == -1
                    ? costFromHere
                    : Math.Min(minDelta, costFromHere);
            }

            //Console.WriteLine($"Mean: {mean}");
            //Console.WriteLine($"Median: {median}");
            //Console.WriteLine($"Midpoint: {midpoint}");
            //Console.WriteLine($"Sum of deltas: {nums.Sum(x => fuelCosts[Math.Abs(x - mean)])}");
            //Console.WriteLine($"Sum of deltas: {nums.Sum(x => fuelCosts[Math.Abs(x - median)])}");
            //Console.WriteLine($"Sum of deltas: {nums.Sum(x => fuelCosts[Math.Abs(x - midpoint)])}");
            Console.WriteLine($"Lowest cost {minDelta}");
        }
    }
}
