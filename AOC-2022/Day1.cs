using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2022
{
    public class Day1
    {
        public static async Task Part1()
        {
            var allSnacks = (await File.ReadAllLinesAsync("../../../Input/Day1.txt"));
            var currentSum = 0;
            var totalCaloriesByElf = new List<int>();
            foreach(var line in allSnacks)
            {
                if (string.IsNullOrEmpty(line))
                {
                    totalCaloriesByElf.Add(currentSum);
                    currentSum = 0;
                }
                else
                {
                    currentSum += int.Parse(line);
                }
            }

            Console.WriteLine(string.Join("\r\n", totalCaloriesByElf.OrderByDescending(x => x)));
        }



        public static async Task Part2()
        {
            var allSnacks = (await File.ReadAllLinesAsync("../../../Input/Day1.txt"));
            var currentSum = 0;
            var totalCaloriesByElf = new List<int>();
            foreach (var line in allSnacks)
            {
                if (string.IsNullOrEmpty(line))
                {
                    totalCaloriesByElf.Add(currentSum);
                    currentSum = 0;
                }
                else
                {
                    currentSum += int.Parse(line);
                }
            }

            Console.WriteLine(totalCaloriesByElf.OrderByDescending(x => x).Take(3).Sum());
        }
    }
}
