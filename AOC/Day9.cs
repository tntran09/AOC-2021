using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    class Day9
    {
        public static async Task Part1()
        {
            var map = (await File.ReadAllLinesAsync("../../../Inputs/Day9.txt"))
                .Select(line => line.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();

            int sum = 0;
            for(int r = 0; r < map.Length; r++)
            {
                for(int c = 0; c < map[r].Length; c++)
                {
                    if (IsLowPoint(map, r, c))
                    {
                        sum += map[r][c] + 1;
                    }
                }
            }

            Console.WriteLine($"Sum of risks: {sum}");
        }

        private static bool IsLowPoint(int[][] map, int r, int c)
        {
            return (r == 0 || (r > 0 && map[r - 1][c] > map[r][c]))
                && (c == 0 || (c > 0 && map[r][c - 1] > map[r][c]))
                && (r == map.Length - 1 || (r < map.Length - 1 && map[r + 1][c] > map[r][c]))
                && (c == map[c].Length - 1 || (c < map.Length - 1 && map[r][c + 1] > map[r][c]));
        }

        public static async Task Part2()
        {
            // Mark all 9's as 'X', everything else as '-'
            var map = (await File.ReadAllLinesAsync("../../../Inputs/Day9.txt"))
                .Select(line => line.ToCharArray().Select(c => c == '9' ? 'X' : '-').ToArray())
                .ToArray();

            // Find an empty spot and recursively traverse the map, marking where we went (O)
            var basinSizes = new List<int>();
            for(int r = 0; r < map.Length; r++)
            {
                for(int c = 0; c < map[r].Length; c++)
                {
                    if (map[r][c] == '-')
                    {
                        basinSizes.Add(TraverseBasin(map, r, c, 0));
                    }
                }
            }

            //Console.WriteLine($"All basin sizes: {string.Join(",", basinSizes)}");
            Console.WriteLine($"Top 3 basin sizes: {string.Join(",", basinSizes.OrderByDescending(x => x).Take(3))}");
        }

        private static int TraverseBasin(char[][] map, int r, int c, int localBasinSize)
        {
            map[r][c] = 'O';
            localBasinSize++;

            if (c < map[r].Length - 1 && map[r][c + 1] == '-')
            {
                localBasinSize = TraverseBasin(map, r, c + 1, localBasinSize);
            }

            if (r < map.Length - 1 && map[r + 1][c] == '-')
            {
                localBasinSize = TraverseBasin(map, r + 1, c, localBasinSize);
            }

            if (c > 0 && map[r][c - 1] == '-')
            {
                localBasinSize = TraverseBasin(map, r, c - 1, localBasinSize);
            }

            if (r > 0 && map[r - 1][c] == '-')
            {
                localBasinSize = TraverseBasin(map, r - 1, c, localBasinSize);
            }

            return localBasinSize;
        }
    }
}
