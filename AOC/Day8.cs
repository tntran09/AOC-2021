using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    class Day8
    {
        public static async Task Part1()
        {
            var entries = (await File.ReadAllLinesAsync("../../../Inputs/Day8.txt"))
                .Select(e => e.Split('|')[1].Trim().Split(' ').Count(x => x.Length == 2 || x.Length == 3 || x.Length == 4 || x.Length == 7))
                .ToArray();

            Console.WriteLine($"Sum of [1478] digits: {entries.Sum()}");
        }

        public static async Task Part2()
        {
            var entries = (await File.ReadAllLinesAsync("../../../Inputs/Day8.txt"))
                .Select(e => e.Split('|'))
                .ToArray();

            int totalSum = 0;
            foreach (var entry in entries)
            {
                var codes = SolveEntry(entry[0].Trim().Split(' '));
                var decodedOutput = Decode(entry[1].Trim().Split(' '), codes);
                Console.WriteLine($"Decoded to: {decodedOutput}");

                totalSum += int.Parse(decodedOutput);
            }

            Console.WriteLine($"Total Sum: {totalSum}");
        }

        private static Dictionary<char, char> SolveEntry(string[] inputs)
        {
            var decodedSegments = new Dictionary<char, char>();
            // Order of solving: a, c, f, d, b, g, e
            var encoded_1 = inputs.First(x => x.Length == 2);
            var encoded_7 = inputs.First(x => x.Length == 3);

            // One diff between (1) and (7), easy to find code for (a)
            var encodedSegment_a = encoded_7.Except(encoded_1).First();
            decodedSegments[encodedSegment_a] = 'a';

            // One missing segment from (0), (6), and (9), find them all
            var encoded_069 = inputs.Where(x => x.Length == 6);
            var encoded_cde = encoded_069.Select(x => "abcdefg".Except(x).First());

            // Exactly one of them should match one of the segments from encoded_1, that is the code for (c)
            var encodedSegment_c = encoded_cde.First(x => encoded_1.Contains(x));
            decodedSegments[encodedSegment_c] = 'c';
            
            // The other segment from (1) is now easy to find (f)
            var encodedSegment_f = encoded_1.Except(encodedSegment_c.ToString()).First();
            decodedSegments[encodedSegment_f] = 'f';

            // Using the same encodings for (0), (6), (9) but removing known solutions so far
            var encoded_acf = $"{encodedSegment_a}{encodedSegment_c}{encodedSegment_f}";
            var encoded_069_remove_acf = encoded_069.Select(x => new string(x.Except(encoded_acf).ToArray()))
                .OrderBy(x => x.Length)
                .ToArray();

            // The two short ones are (0) and (9)
            var encoded_09_remove_acf = encoded_069_remove_acf.Take(2).ToArray();
            // They should have one char difference, get both of them
            var encodedSegments_de = new[]
            {
                encoded_09_remove_acf.First().Except(encoded_09_remove_acf.Last()).First(),
                encoded_09_remove_acf.Last().Except(encoded_09_remove_acf.First()).First()
            };
            // Exactly one should match a segment in (4), find (d)
            var encoded_4 = inputs.First(x => x.Length == 4);
            var encodedSegment_d = encodedSegments_de.First(x => encoded_4.Contains(x));
            decodedSegments[encodedSegment_d] = 'd';

            // Easy to find last segment for (4), which is (b)
            var encoded_acdf = $"{encodedSegment_a}{encodedSegment_c}{encodedSegment_d}{encodedSegment_f}";
            var encodedSegment_b = encoded_4.Except(encoded_acdf).First();
            decodedSegments[encodedSegment_b] = 'b';

            // Using same encodings for (0), (9) but removing two new solutions found
            var encoded_bd = $"{encodedSegment_b}{encodedSegment_d}";
            var encoded_09_remove_abcdf = encoded_09_remove_acf.Select(x => new string(x.Except(encoded_bd).ToArray()))
                .OrderBy(x => x.Length)
                .ToArray();
            // (9) is the first one, (0) is the second one, easy to find (g)
            var encodedSegment_g = encoded_09_remove_abcdf.First().First();
            decodedSegments[encodedSegment_g] = 'g';

            // Last remaining segment is found in (0)
            var encodedSegment_e = encoded_09_remove_abcdf.Last().Except(new[] {encodedSegment_g}).First();
            decodedSegments[encodedSegment_e] = 'e';

            return decodedSegments;
        }

        private static string Decode(string[] inputs, Dictionary<char, char> codes)
        {
            var segmentsToDigits = new Dictionary<string, int>()
            {
                {"abcefg", 0},
                {"cf", 1},
                {"acdeg", 2},
                {"acdfg", 3},
                {"bcdf", 4},
                {"abdfg", 5},
                {"abdefg", 6},
                {"acf", 7},
                {"abcdefg", 8},
                {"abcdfg", 9},
            };

            var output = inputs.Select(str => segmentsToDigits[new string(
                str.Select(c => codes[c]).OrderBy(x => x).ToArray()
            )]);
            return string.Join("", output);
        }
    }
}
