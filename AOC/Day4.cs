using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    public static class Day4
    {
        private static int[] drawnNumbers;
        private static List<int[][]> boards = new List<int[][]>();
        private static List<int?[][]> markedBoards = new List<int?[][]>();

        private static async Task ParseInputs()
        {
            var file = await File.ReadAllLinesAsync("../../../Inputs/Day4.txt");
            drawnNumbers = file[0].Split(',').Select(int.Parse).ToArray();

            var lineIndex = 2;
            while (lineIndex < file.Length)
            {
                var tempBoard = new int[5][];
                var tempMarkedBoard = new int?[5][];
                for (int i = 0; i < 5; i++)
                {
                    tempBoard[i] = file[lineIndex + i].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();
                    tempMarkedBoard[i] = new int?[5];
                }
                boards.Add(tempBoard);
                markedBoards.Add(tempMarkedBoard);

                lineIndex += 6;
            }
        }

        public static async Task Part1()
        {
            await ParseInputs();

            var drawIndex = 0;
            var foundWinner = false;
            while (drawIndex < drawnNumbers.Length && !foundWinner)
            {
                var num = drawnNumbers[drawIndex];
                for (int i = 0; i < boards.Count; i++)
                {
                    MarkBoard(boards[i], markedBoards[i], num);
                    if (CheckWin(markedBoards[i]))
                    {
                        CalcScore(boards[i], markedBoards[i], num);
                        foundWinner = true;
                    }
                }
                drawIndex++;
            }
        }

        public static async Task Part2()
        {
            await ParseInputs();

            var drawIndex = 0;
            var foundWinner = false;
            while (drawIndex < drawnNumbers.Length && !foundWinner)
            {
                var num = drawnNumbers[drawIndex];
                var eliminateWinners = new List<int>();

                for (int i = 0; i < boards.Count; i++)
                {
                    MarkBoard(boards[i], markedBoards[i], num);
                    if (CheckWin(markedBoards[i]))
                    {
                        if (boards.Count > 1)
                        {
                            eliminateWinners.Insert(0, i);
                        }
                        else if (boards.Count == 1)
                        {
                            CalcScore(boards[i], markedBoards[i], num);
                            foundWinner = true;
                        }
                    }
                }

                foreach (var w in eliminateWinners)
                {
                    boards.RemoveAt(w);
                    markedBoards.RemoveAt(w);
                }

                drawIndex++;
            }
        }

        private static void MarkBoard(int[][] board, int?[][] markedBoard, int num)
        {
            for(int i = 0; i < board.Length; i++)
            {
                for(int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] == num)
                    {
                        markedBoard[i][j] = num;
                        return;
                    }
                }
            }
        }

        private static bool CheckWin(int?[][] markedBoard)
        {
            for(int i = 0; i < 5; i++)
            {
                if (markedBoard[i].All(x => x.HasValue))
                {
                    return true;
                }

                if (markedBoard.Select(x => x[i]).All(x => x.HasValue))
                {
                    return true;
                }
            }

            return false;
        }

        private static void CalcScore(int[][] board, int?[][] markedBoard, int winningNumber)
        {
            var sum = 0;
            for (int i = 0; i < markedBoard.Length; i++)
            {
                for (int j = 0; j < markedBoard[i].Length; j++)
                {
                    if (!markedBoard[i][j].HasValue)
                    {
                        sum += board[i][j];
                    }
                }
            }

            Console.WriteLine($"Sum of unmarked numbers: {sum}");
            Console.WriteLine($"Winning number: {winningNumber}");
        }
    }
}
