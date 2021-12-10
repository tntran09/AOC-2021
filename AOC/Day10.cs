using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    class Day10
    {
        public static async Task Part1()
        {
            var lines = (await File.ReadAllLinesAsync("../../../Inputs/Day10.txt"));

            var sum = 0;
            var completionScores = new List<long>();
            foreach (var line in lines)
            {
                var expectedClosing = "";
                var corrupted = false;
                for (var i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    switch(c)
                    {
                        case '(': expectedClosing += ")"; break;
                        case '[': expectedClosing += "]"; break;
                        case '{': expectedClosing += "}"; break;
                        case '<': expectedClosing += ">"; break;
                        case ')':
                        case ']':
                        case '}':
                        case '>':
                            if (c == expectedClosing.Last())
                            {
                                expectedClosing = expectedClosing.Substring(0, expectedClosing.Length - 1);
                            }
                            else
                            {
                                sum += GetPointsForIncorrectChar(c);
                                corrupted = true;
                            }
                            break;
                    }

                    if (corrupted)
                    {
                        break;
                    }
                }

                if (!corrupted && !string.IsNullOrEmpty(expectedClosing))
                {
                    // Reverse order of the actual string to complete
                    expectedClosing = new string(expectedClosing.Reverse().ToArray());
                    long score = 0;
                    for(var i = 0; i < expectedClosing.Length; i++)
                    {
                        score = (score * 5) + GetPointsForRemainingChar(expectedClosing[i]);
                    }

                    completionScores.Add(score);
                }
            }

            //Console.WriteLine($"Sum of errors: {sum}");

            completionScores = completionScores.OrderBy(x => x).ToList();
            Console.WriteLine($"Middle completion score: {completionScores[completionScores.Count / 2]}");
        }

        private static int GetPointsForIncorrectChar(char c)
        {
            switch (c)
            {
                case ')': return 3;
                case ']': return 57;
                case '}': return 1197;
                case '>': return 25137;
                default: return 0;
            }
        }

        private static int GetPointsForRemainingChar(char c)
        {
            switch (c)
            {
                case ')': return 1;
                case ']': return 2;
                case '}': return 3;
                case '>': return 4;
                default: return 0;
            }
        }
    }
}
