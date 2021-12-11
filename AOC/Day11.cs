using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    class Day11
    {
        public static async Task Part1()
        {
            var map = (await File.ReadAllLinesAsync("../../../Inputs/Day11.txt"))
                .Select(s => s.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();

            var numberOfFlashes = 0;

            for (var round = 0; round < 1000; round++)
            {
                var flashed = new List<Tuple<int, int>>();
                var pendingFlash = new List<Tuple<int, int>>();

                for (var i = 0; i < map.Length; i++)
                {
                    for(var j = 0; j < map[i].Length; j++)
                    {
                        map[i][j]++;
                        if (map[i][j] > 9)
                        {
                            pendingFlash.Add(Tuple.Create(i, j));
                        }
                    }
                }

                while(pendingFlash.Any())
                {
                    var position = pendingFlash[0];
                    pendingFlash.RemoveAt(0);
                    flashed.Add(position);
                    numberOfFlashes++;

                    if (position.Item1 > 0)
                    {
                        var p = Tuple.Create(position.Item1 - 1, position.Item2);
                        map[p.Item1][p.Item2]++;
                        if (map[p.Item1][p.Item2] > 9 && !flashed.Contains(p) && !pendingFlash.Contains(p)) { pendingFlash.Add(p); }
                    }

                    if (position.Item2 > 0)
                    {
                        var p = Tuple.Create(position.Item1, position.Item2 - 1);
                        map[p.Item1][p.Item2]++;
                        if (map[p.Item1][p.Item2] > 9 && !flashed.Contains(p) && !pendingFlash.Contains(p)) { pendingFlash.Add(p); }
                    }

                    if (position.Item1 < map.Length - 1)
                    {
                        var p = Tuple.Create(position.Item1 + 1, position.Item2);
                        map[p.Item1][p.Item2]++;
                        if (map[p.Item1][p.Item2] > 9 && !flashed.Contains(p) && !pendingFlash.Contains(p)) { pendingFlash.Add(p); }
                    }

                    if (position.Item2 < map[position.Item1].Length - 1)
                    {
                        var p = Tuple.Create(position.Item1, position.Item2 + 1);
                        map[p.Item1][p.Item2]++;
                        if (map[p.Item1][p.Item2] > 9 && !flashed.Contains(p) && !pendingFlash.Contains(p)) { pendingFlash.Add(p); }
                    }

                    if (position.Item1 > 0 && position.Item2 > 0)
                    {
                        var p = Tuple.Create(position.Item1 - 1, position.Item2 - 1);
                        map[p.Item1][p.Item2]++;
                        if (map[p.Item1][p.Item2] > 9 && !flashed.Contains(p) && !pendingFlash.Contains(p)) { pendingFlash.Add(p); }
                    }

                    if (position.Item1 > 0 && position.Item2 < map[position.Item1].Length - 1)
                    {
                        var p = Tuple.Create(position.Item1 - 1, position.Item2 + 1);
                        map[p.Item1][p.Item2]++;
                        if (map[p.Item1][p.Item2] > 9 && !flashed.Contains(p) && !pendingFlash.Contains(p)) { pendingFlash.Add(p); }
                    }

                    if (position.Item1 < map.Length - 1 && position.Item2 > 0)
                    {
                        var p = Tuple.Create(position.Item1 + 1, position.Item2 - 1);
                        map[p.Item1][p.Item2]++;
                        if (map[p.Item1][p.Item2] > 9 && !flashed.Contains(p) && !pendingFlash.Contains(p)) { pendingFlash.Add(p); }
                    }

                    if (position.Item1 < map.Length - 1 && position.Item2 < map[position.Item1].Length - 1)
                    {
                        var p = Tuple.Create(position.Item1 + 1, position.Item2 + 1);
                        map[p.Item1][p.Item2]++;
                        if (map[p.Item1][p.Item2] > 9 && !flashed.Contains(p) && !pendingFlash.Contains(p)) { pendingFlash.Add(p); }
                    }
                }

                foreach(var f in flashed)
                {
                    map[f.Item1][f.Item2] = 0;
                }

                if (flashed.Count == 100)
                {
                    Console.WriteLine($"Flashed everything at Round = {round}");
                    break;
                }
            }

            //Console.WriteLine($"Total flashes: {numberOfFlashes}");
        }
    }
}
