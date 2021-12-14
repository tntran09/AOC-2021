using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    class Day13
    {
        public static async Task Part1()
        {
            var lines = (await File.ReadAllLinesAsync("../../../Inputs/Day13.txt"));
            var dots = new List<Tuple<int, int>>();
            int max_x = 0, max_y = 0;
            var instructionsBegin = 0;
            for(int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    instructionsBegin = i + 1;
                    break;
                }

                var xy = lines[i].Split(",");
                var tuple = Tuple.Create(int.Parse(xy[0]), int.Parse(xy[1]));
                dots.Add(tuple);
                max_x = Math.Max(max_x, tuple.Item1);
                max_y = Math.Max(max_y, tuple.Item2);
            }

            var map = new bool[max_y + 1][];
            for (int i = 0; i < map.Length; i++)
            {
                map[i] = new bool[max_x + 1];
            }

            foreach (var d in dots)
            {
                map[d.Item2][d.Item1] = true;
            }

            const string FOLD_ALONG = "fold along ";
            for (int i = instructionsBegin; i < lines.Length; i++)
            {
                var instruction = lines[i].Substring(FOLD_ALONG.Length).Split('=');
                var mid = int.Parse(instruction[1]);

                if (instruction[0] == "x")
                {
                    var newMap = new bool[map.Length][];
                    for (int r = 0; r < newMap.Length; r++)
                    {
                        newMap[r] = new bool[mid];
                    }

                    Copy(map, newMap);
                    for (int r = 0; r < map.Length; r++)
                    {
                        for (int c = mid + 1; c < map[r].Length; c++)
                        {
                            newMap[r][mid - (c - mid)] = map[r][mid - (c - mid)] || map[r][c];
                        }
                    }

                    map = newMap;
                }
                else if (instruction[0] == "y")
                {
                    var newMap = new bool[mid][];
                    for (int r = 0; r < newMap.Length; r++)
                    {
                        newMap[r] = new bool[map[r].Length];
                    }

                    Copy(map, newMap);
                    for (int r =  mid + 1; r < map.Length; r++)
                    {
                        for (int c = 0; c < map[r].Length; c++)
                        {
                            newMap[mid - (r - mid)][c] = map[mid - (r - mid)][c] || map[r][c];
                        }
                    }

                    map = newMap;
                }
            }

            var sum = map.Sum(arr => arr.Count(c => c));
            //Console.WriteLine($"Visible dots: {sum}");
            foreach (var line in map)
            {
                Console.WriteLine(string.Join("", line.Select(x => x ? '#' : '.')));
            }
        }

        private static void Copy(bool[][] oldMap, bool[][] newMap)
        {
            // Assume newMap dimensions are equal or smaller than oldMap
            for (int i = 0; i < newMap.Length; i++)
            {
                for (int j = 0; j < newMap[i].Length; j++)
                {
                    newMap[i][j] = oldMap[i][j];
                }
            }
        }
    }
}
