using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    public class Day12
    {
        public static async Task Part1()
        {
            var lines = (await File.ReadAllLinesAsync("../../../Inputs/Day12.txt"));

            var nodes = new Dictionary<string, Node>();
            foreach (var line in lines)
            {
                var split = line.Split("-");
                var n1 = nodes.ContainsKey(split[0])
                    ? nodes[split[0]]
                    : new Node() {Name = split[0], IsBigCave = split[0] == split[0].ToUpper()};
                var n2 = nodes.ContainsKey(split[1])
                    ? nodes[split[1]]
                    : new Node() { Name = split[1], IsBigCave = split[1] == split[1].ToUpper() };

                n1.ConnectedNodes.Add(n2);
                n2.ConnectedNodes.Add(n1);

                nodes[n1.Name] = n1;
                nodes[n2.Name] = n2;
            }

            var pathsFound = new List<string>();
            var potentialPaths = nodes["start"].ConnectedNodes.Select(x => $"start,{x.Name}").ToList();

            while (potentialPaths.Any())
            {
                var path = potentialPaths[0];
                potentialPaths.RemoveAt(0);
                var node = nodes[path.Split(",").Last()];

                foreach (var n in node.ConnectedNodes)
                {
                    if (n.Name == "start")
                    {
                        continue;
                    }

                    if (n.Name == "end")
                    {
                        pathsFound.Add($"{path},{n.Name}");
                    }
                    else if (n.IsBigCave || !path.Contains(n.Name))
                    {
                        potentialPaths.Add($"{path},{n.Name}");
                    }
                    else if (!n.IsBigCave && !path.StartsWith("_FLAG_"))
                    {
                        potentialPaths.Add($"_FLAG_{path},{n.Name}");
                    }
                }
            }

            Console.WriteLine($"Paths found: {pathsFound.Count}");
        }


        public class Node
        {
            public string Name;
            public List<Node> ConnectedNodes = new List<Node>();
            public bool IsBigCave;
        }
    }

}
