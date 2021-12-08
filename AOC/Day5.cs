using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    public class Day5
    {
        public static async Task Part1()
        {
            var lines = (await File.ReadAllLinesAsync("../../../Inputs/Day5.txt"))
                .Select(s => s.Split(" ->,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .ToArray();

            var max_x = lines.SelectMany(points => new[] {points[0], points[2]}).Max() + 1;
            var max_y = lines.SelectMany(points => new[] {points[1], points[3]}).Max() + 1;

            var board = new int[max_y, max_x];
            foreach (var line in lines)
            {
                //if (line[0] != line[2] && line[1] != line[3])
                //{
                //    continue;
                //}

                // Traverse vertically (y)
                if (line[0] == line[2])
                {
                    for (var r = Math.Min(line[1], line[3]); r <= Math.Max(line[1], line[3]); r++)
                    {
                        board[r, line[0]]++;
                    }

                    continue;
                }

                // Traverse horizontally (x)
                if (line[1] == line[3])
                {
                    for (var c = Math.Min(line[0], line[2]); c <= Math.Max(line[0], line[2]); c++)
                    {
                        board[line[1], c]++;
                    }

                    continue;
                }

                // Diagonal
                var row = line[0] < line[2] ? line[1] : line[3]; // Row of the leftmost point
                var isDownDirection = row == line[1] ? line[1] < line[3] : line[1] > line[3]; // When the left point is the first point, then when Y increases, otherwise when Y decreases
                for (int c = Math.Min(line[0], line[2]); // Start at that leftmost point
                    c <= Math.Max(line[0], line[2]);
                    c++) // Go right
                {
                    board[row, c]++;

                    row += isDownDirection ? 1 : -1; // Go up or down
                }
            }

            Console.WriteLine($"Points >= 2: {board.Cast<int>().Count(x => x >= 2)}");
        }
    }
}
