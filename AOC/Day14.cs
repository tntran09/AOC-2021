using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    class Day14
    {
        public static async Task Part1()
        {
            var lines = (await File.ReadAllLinesAsync("../../../Inputs/Day14.txt"));
            var polymer = lines[0];
            var endPolymer = polymer;
            var rules = new Dictionary<string, string>();
            for(int i = 2; i < lines.Length; i++)
            {
                var split = lines[i].Split(" -> ");
                rules.Add(split[0], split[1]);
            }

            for(int round = 0; round < 23; round++)
            {
                polymer = endPolymer;
                var arr = new char[polymer.Length * 2 - 1];
                arr[0] = polymer[0];

                for(int i = 1; i < polymer.Length; i++)
                {
                    var sub = polymer.Substring(i - 1, 2);
                    arr[i * 2 - 1] = rules.ContainsKey(sub) ? rules[sub][0] : default(char);
                    arr[i * 2] = polymer[i];
                }

                endPolymer = string.Join("", arr);
            }

            var counts = endPolymer
                .Replace(default(char).ToString(), "")
                .GroupBy(c => c)
                .Select(g => Tuple.Create(g.Key, g.Count()))
                .OrderBy(t => t.Item2)
                .ToArray();

            Console.WriteLine(string.Join("\r\n", counts.Select(ct => $"{ct.Item1}: {ct.Item2}")));
        }

        public static async Task Part2()
        {
            var lines = (await File.ReadAllLinesAsync("../../../Inputs/Day14.txt"));
            var rules = new Dictionary<string, string>();

            for (int i = 2; i < lines.Length; i++)
            {
                var split = lines[i].Split(" -> ");
                rules.Add(split[0], split[1]);
            }

            var polymer = lines[0];
            var nextPolymerPairs = new Dictionary<string, long>();
            for (int i = 0; i < polymer.Length - 1; i++)
            {
                var sub = polymer.Substring(i, 2);
                nextPolymerPairs[sub] = nextPolymerPairs.ContainsKey(sub) ? nextPolymerPairs[sub] + 1 : 1;
            }
            var pairCounts = rules.ToDictionary(kv => kv.Key, kv => (long)0);

            for(int round = 0; round < 40; round++)
            {
                var temp = new Dictionary<string, long>();
                foreach (var pair in nextPolymerPairs)
                {
                    var valueAdded = rules[pair.Key];
                    temp[$"{pair.Key[0]}{valueAdded}"] = temp.ContainsKey($"{pair.Key[0]}{valueAdded}") ? temp[$"{pair.Key[0]}{valueAdded}"] + pair.Value : pair.Value;
                    temp[$"{valueAdded}{pair.Key[1]}"] = temp.ContainsKey($"{valueAdded}{pair.Key[1]}") ? temp[$"{valueAdded}{pair.Key[1]}"] + pair.Value : pair.Value;
                }

                nextPolymerPairs = temp;
            }

            var counts = new Dictionary<string, long>();
            string.Join("", rules.Select(kv => kv.Key))
                .Distinct()
                .ToList()
                .ForEach(x => { counts[x.ToString()] = 0; });
            foreach (var pair in nextPolymerPairs)
            {
                counts[pair.Key[0].ToString()] += pair.Value;
                counts[pair.Key[1].ToString()] += pair.Value;
            }

            Console.WriteLine(string.Join("\r\n", counts.OrderByDescending(ct => ct.Value).Select(ct => $"{ct.Key}: {ct.Value / 2.0}")));
        }
    }
}
