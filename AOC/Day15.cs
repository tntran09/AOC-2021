using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    class Day15
    {
        public static async Task Part1()
        {
            var map = await File.ReadAllLinesAsync("../../../Inputs/Day15.txt");
            var cave = string.Join("", map)
                .Select(c => int.Parse(c.ToString()))
                .ToArray();
            var nodes = new CaveNode[cave.Length];
            for (int i = 0; i < cave.Length; i++)
            {
                nodes[i] = new CaveNode
                {
                    Index = i,
                    Risk = cave[i],
                    Score = int.MaxValue,
                    ConnectedNodes = new List<CaveNode>()
                };

                if (i >= map[0].Length)
                {
                    nodes[i].ConnectedNodes.Add(nodes[i - map[0].Length]);
                    nodes[i - map[0].Length].ConnectedNodes.Add(nodes[i]);
                }

                if (i % map[0].Length != 0)
                {
                    nodes[i].ConnectedNodes.Add(nodes[i - 1]);
                    nodes[i - 1].ConnectedNodes.Add(nodes[i]);
                }
            }

            nodes[0].Score = 0;
            
            var pending = new SimpleMinHeap<CaveNode>();
            foreach(var cn in nodes) { pending.Push(cn); }

            while(pending.Count > 0)
            {
                var u = pending.Pop();

                var neighborsToExamine = u.ConnectedNodes
                    .Where(n => pending.Exists(n) )
                    .ToArray();
                foreach (var neighbor in neighborsToExamine)
                {
                    if (u.Score + neighbor.Risk < neighbor.Score)
                    {
                        // Found a good path to neighbor, reassign his score, but will affect where he is in the heap
                        pending.Remove(neighbor);
                        neighbor.Score = u.Score + neighbor.Risk;
                        //neighbor.Path = u.Path.Concat(new[] { neighbor.Index }).ToArray();
                        pending.Push(neighbor);
                    }
                }
            }

            int x = 0;

            Console.WriteLine($"Bottom right score: {nodes.Last().Score}");
            //Console.WriteLine($"Path length: {nodes.Last().Path.Length}");
            //var visitedMap = new bool[map.Length][];
            //for (int i = 0; i < visitedMap.Length; i++)
            //{
            //    visitedMap[i] = new bool[map[i].Length];
            //}

            //nodes.Last().Path
            //    .Select(index => Tuple.Create(index / map[0].Length, index % map[0].Length))
            //    .ToList()
            //    .ForEach(t => visitedMap[t.Item1][t.Item2] = true);
            //Console.WriteLine(string.Join("\r\n", visitedMap.Select(r => string.Join("", r.Select(c => c ? 'O' : '-')))));
        }

        public static async Task Part2()
        {
            var lines = await File.ReadAllLinesAsync("../../../Inputs/Day15.txt");
            var firstMap = lines
                .Select(line => line.Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();
            var bigMap = new int[firstMap.Length * 5][];

            for (int k = 0; k < firstMap.Length; k++)
            {
                var arr = firstMap[k];

                for (int m = 0; m < firstMap[k].Length; m++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (bigMap[k + (i * firstMap.Length)] == null)
                        {
                            bigMap[k + (i * firstMap.Length)] = new int[firstMap[0].Length * 5];
                        }
                        
                        for (int j = 0; j < 5; j++)
                        {
                            bigMap[k + (i * firstMap.Length)][m + (j * firstMap[0].Length)] = (arr[m] + i + j) % 10;
                            //bigMap[i] = arr
                            //    .Concat(arr.Select(x => (x + 1) % 10))
                            //    .Concat(arr.Select(x => (x + 2) % 10))
                            //    .Concat(arr.Select(x => (x + 3) % 10))
                            //    .Concat(arr.Select(x => (x + 4) % 10))
                            //    .ToArray();
                        }
                    }
                }
            }

            var cave = new int[bigMap.Length * bigMap[0].Length];
            for(int i = 0; i < bigMap.Length; i++)
            {
                for(int j = 0; j < bigMap[i].Length; j++)
                {
                    cave[i * bigMap[0].Length + j] = bigMap[i][j];
                }
            }
            var nodes = new CaveNode[cave.Length];
            for (int i = 0; i < cave.Length; i++)
            {
                nodes[i] = new CaveNode
                {
                    Index = i,
                    Risk = cave[i],
                    Score = int.MaxValue,
                    ConnectedNodes = new List<CaveNode>()
                };

                if (i >= lines[0].Length)
                {
                    nodes[i].ConnectedNodes.Add(nodes[i - lines[0].Length]);
                    nodes[i - lines[0].Length].ConnectedNodes.Add(nodes[i]);
                }

                if (i % lines[0].Length != 0)
                {
                    nodes[i].ConnectedNodes.Add(nodes[i - 1]);
                    nodes[i - 1].ConnectedNodes.Add(nodes[i]);
                }
            }

            nodes[0].Score = 0;

            // [100, 100] n=10 000 => [500, 500] n=250 000
            // 0 => 0, 100, 200, 300, 400, 50000

            var pending = new SimpleMinHeap<CaveNode>();
            foreach (var cn in nodes) { pending.Push(cn); }

            while (pending.Count > 0)
            {
                var u = pending.Pop();

                var neighborsToExamine = u.ConnectedNodes
                    .Where(n => pending.Exists(n))
                    .ToArray();
                foreach (var neighbor in neighborsToExamine)
                {
                    if (u.Score + neighbor.Risk < neighbor.Score)
                    {
                        // Found a good path to neighbor, reassign his score, but will affect where he is in the heap
                        pending.Remove(neighbor);
                        neighbor.Score = u.Score + neighbor.Risk;
                        pending.Push(neighbor);
                    }
                }
            }

            int x = 0;

            Console.WriteLine($"Bottom right score: {nodes.Last().Score}");
        }

        public class CaveNode : IComparable<CaveNode>, IEquatable<CaveNode>
        {
            public int Index;
            public int Risk;
            public int Score;
            //public bool HasBeenVisited;
            //public int[] Path = new int[0]; // try to avoid list of nodes?
            public List<CaveNode> ConnectedNodes;

            public int CompareTo([AllowNull] CaveNode other)
            {
                var comp = Score.CompareTo(other.Score);
                return comp != 0 ? comp : Index.CompareTo(other.Index);
            }

            public bool Equals([AllowNull] CaveNode other)
            {
                return Index.Equals(other.Index);
            }
        }

        public class SimpleMinHeap<T> where T : CaveNode
        {
            private List<T> Items = new List<T>();
            public int Count => Items.Count;

            public T Pop()
            {
                var c = Items.Count(x => x == null);
                var min = Items.Aggregate(
                    Items[0],
                    (accumulate, current) => current.Score < accumulate.Score || (current.Score == accumulate.Score && current.Index < accumulate.Index)
                        ? current
                        : accumulate
                );
                Items.Remove(min);
                return min;
            }

            public void Push(T obj)
            {
                Items.Add(obj);
            }

            public bool Exists(T obj)
            {
                return Items.Any(x => x.Index == obj.Index);
            }

            public T Remove(T obj)
            {
                return Items.Remove(obj) ? obj : null;
            }
        }

        public class MinHeap<T> where T : IComparable<T>, IEquatable<T>
        {
            private HeapNode<T> Root;
            private List<T> Items;
            private int Size;

            public int Count => Size;

            public T Pop()
            {
                var min = Root;
                // Children of Items[i] => 2*i + 1, 2*i + 2
                int i = 0;
                //while (i < Count)
                //{
                //    var left = 2 * i + 1 < Count ? Items[2 * i + 1] : default(T);
                //    var right = 2 * i + 2 < Count ? Items[2 * i + 2] : default(T);

                //    if (left?.CompareTo(right) <= 0)
                //    {

                //    }
                //}

                Size--;

                var prev = min;

                if (Root.Left == null)
                {
                    Root = Root.Right;
                    return min.Value;
                }
                else if (Root.Right == null)
                {
                    Root = Root.Left;
                    return min.Value;
                }
                else
                {
                    while (true)
                    {
                        var child = prev.Left.Value.CompareTo(prev.Right.Value) <= 0
                            ? prev.Left
                            : prev.Right;

                        //if (child.Left == null && child.)
                        if (prev.Right == null)
                        {
                            prev.Value = prev.Left.Value;
                            prev = prev.Left;
                        }
                        else if (prev.Left == null || prev.Left.Value.CompareTo(prev.Right.Value) > 0)
                        {
                            prev.Value = prev.Right.Value;
                            prev = prev.Right;
                        }

                        if (prev == null)
                        {
                            break;
                        }
                    }
                }

                return min.Value;
            }

            public void Push(T obj)
            {
                if (Root == null)
                {
                    Root = new HeapNode<T>
                    {
                        Value = obj
                    };
                    return;
                }

                var n = Root;
                while (true)
                {
                    if (obj.CompareTo(n.Value) <= 0)
                    {
                        if (n.Left != null)
                        {
                            n = n.Left;
                        }
                        else
                        {
                            n.Left = new HeapNode<T>
                            {
                                Value = obj
                            };
                            break;
                        }
                    }
                    else
                    {
                        if (n.Right != null)
                        {
                            n = n.Right;
                        }
                        else
                        {
                            n.Right = new HeapNode<T>
                            {
                                Value = obj
                            };
                            break;
                        }
                    }
                }

                Size++;
            }

            public bool Exists(T obj)
            {
                var n = Root;

                while (true)
                {
                    if (obj.CompareTo(n.Value) == 0)
                    {
                        return obj.Equals(n.Value);
                    }
                    else if (obj.CompareTo(n.Value) < 0)
                    {
                        if (n.Left != null)
                        {
                            n = n.Left;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (n.Right != null)
                        {
                            n = n.Right;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            public T Remove(T obj)
            {
                var n = Root;
                var parentOfRemoved = n;
                while (true)
                {
                    if (obj.CompareTo(n.Value) == 0)
                    {
                        break;
                    }
                    else if (obj.CompareTo(n.Value) < 0)
                    {
                        if (n.Left != null)
                        {
                            parentOfRemoved = n;
                            n = n.Left;
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                    else
                    {
                        if (n.Right != null)
                        {
                            parentOfRemoved = n;
                            n = n.Right;
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }

                // Assume obj was found and is assign to n
                HeapNode<T> subtreeRoot;

                if (n.Left == null) { subtreeRoot = n.Right; }
                else if (n.Right == null) { subtreeRoot = n.Left; }
                else
                {
                    // Both sides are not null
                    // Get the rightmost leaf of the left node
                    var rightmostLeaf = n.Left;
                    //var prev = rightmostLeaf;
                    while (rightmostLeaf.Right != null)
                    {
                        //prev = rightmostLeaf;
                        rightmostLeaf = rightmostLeaf.Right;
                    }
                    if (n.Left != rightmostLeaf)
                    {
                        //prev.Right = rightmostLeaf.Left;
                        rightmostLeaf.Left = n.Left;
                        rightmostLeaf.Right = n.Right;
                        subtreeRoot = rightmostLeaf;
                    }
                    else
                    {
                        rightmostLeaf.Right = n.Right;
                        subtreeRoot = rightmostLeaf.Left;
                    }
                }

                if (parentOfRemoved.Left == n) { parentOfRemoved.Left = subtreeRoot; }
                else if (parentOfRemoved.Right == n) { parentOfRemoved.Right = subtreeRoot; }

                Size--;

                //n.Left = null;
                //n.Right = null;
                return n.Value;
            }

            public class HeapNode<T>
            {
                public T Value;
                public HeapNode<T> Left;
                public HeapNode<T> Right;
            }
        }

        private static int TraverseMap(int[][] riskMap, bool[][] visitedMap, int r, int c, int currentRisk, int maxRisk)
        {
            if (r == riskMap.Length - 1 && c == riskMap[r].Length - 1)
            {
                var steps = visitedMap.Sum(r => r.Count(c => c));
                Console.WriteLine($"Found a new lowest: Risk = {currentRisk}, Steps = {steps}");
                Console.WriteLine(string.Join("\r\n", visitedMap.Select(r => string.Join("", r.Select(c => c ? 'O' : '-')))));
                return currentRisk;
            }

            visitedMap[r][c] = true;

            var potentialMoves = new[]
            {
                Tuple.Create(r, c+1),
                Tuple.Create(r+1, c),
                Tuple.Create(r, c-1),
                Tuple.Create(r-1, c),
            }
                .Where(t => t.Item1 >= 0 && t.Item1 < riskMap.Length
                    && t.Item2 >= 0 && t.Item2 < riskMap[t.Item1].Length
                    && !visitedMap[t.Item1][t.Item2])
                .OrderByDescending(t => t.Item1 > r || t.Item2 > c ? 1 : 0) // Prioritize right and down movements
                .ThenBy(t => riskMap[t.Item1][t.Item2]) // Then lowest risk score
                .ToArray();

            foreach(var move in potentialMoves)
            {
                if (currentRisk + riskMap[move.Item1][move.Item2] < maxRisk)
                {
                    maxRisk = TraverseMap(riskMap, visitedMap, move.Item1, move.Item2, currentRisk + riskMap[move.Item1][move.Item2], maxRisk);
                }
            }

            visitedMap[r][c] = false;
            return maxRisk;
        }
    }
}
