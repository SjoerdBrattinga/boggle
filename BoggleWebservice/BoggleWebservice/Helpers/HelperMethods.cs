using System;
using System.Collections.Generic;
using System.Linq;
using BoggleWebservice.Models;

namespace BoggleWebservice.Helpers
{
    public static class HelperMethods
    {
        private static readonly Random Rng = new Random();

        private const int MatrixSize = 4;
        private const int NoOfDice = MatrixSize * MatrixSize;
        private const int DieFaces = 6;

        private static readonly char[,] Dice = {
            {'R', 'I', 'F', 'O', 'B', 'X'},
            {'I', 'F', 'E', 'H', 'E', 'Y'},
            {'D', 'E', 'N', 'O', 'W', 'S'},
            {'U', 'T', 'O', 'K', 'N', 'D'},
            {'H', 'M', 'S', 'R', 'A', 'O'},
            {'L', 'U', 'P', 'E', 'T', 'S'},
            {'A', 'C', 'I', 'T', 'O', 'A'},
            {'Y', 'L', 'G', 'K', 'U', 'E'},
            {'Q', 'B', 'M', 'J', 'O', 'A'},
            {'E', 'H', 'I', 'S', 'P', 'N'},
            {'V', 'E', 'T', 'I', 'G', 'N'},
            {'B', 'A', 'L', 'I', 'Y', 'T'},
            {'E', 'Z', 'A', 'V', 'N', 'D'},
            {'R', 'A', 'L', 'E', 'S', 'C'},
            {'U', 'W', 'I', 'L', 'R', 'G'},
            {'P', 'A', 'C', 'E', 'M', 'D'}
        };

        private static readonly Dictionary<int, List<int>> ConnectedDieIndexes = new Dictionary<int, List<int>> {
            { 0, new List<int>{ 1, 4, 5 } },
            { 1, new List<int>{ 0, 2, 4, 5, 6 } },
            { 2, new List<int>{ 1, 3, 5, 6, 7 } },
            { 3, new List<int>{ 2, 6, 7 } },
            { 4, new List<int>{ 0, 1, 5, 8, 9 } },
            { 5, new List<int>{ 0, 1, 2, 4, 6, 8, 9, 10 } },
            { 6, new List<int>{ 1, 2, 3, 5, 7, 9, 10, 11 } },
            { 7, new List<int>{ 2, 3, 6, 10, 11 } },
            { 8, new List<int>{ 4, 5, 9, 12, 13 } },
            { 9, new List<int>{ 4, 5, 6, 8, 10, 12, 13, 14 } },
            { 10, new List<int>{ 5, 6, 7, 9, 11, 13, 14, 15 } },
            { 11, new List<int>{ 6, 7, 10, 14, 15 } },
            { 12, new List<int>{ 8, 9, 13 } },
            { 13, new List<int>{ 8, 9, 10, 12, 14 } },
            { 14, new List<int>{ 9, 10, 11, 13, 15 } },
            { 15, new List<int>{ 10, 11, 14 } }
        };

        public static void Roll(this List<Die> rolledDice)
        {
            for (var i = 0; i < NoOfDice; i++)
            {
                var temp = new char[DieFaces];

                for (var j = 0; j < temp.Length; j++)
                {
                    temp[j] = Dice[i, j];
                }

                var n = Rng.Next(0, temp.Length);

                var die = new Die
                {
                    Value = temp[n].ToString()
                };

                rolledDice.Add(die);
            }
        }

        // Fisher-Yates shuffle
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<List<Die>> ConvertLettersToDice(this string letters)
        {
            var diceList = letters.ConvertLettersToDieList();

            return diceList.ConvertListToMatrix();
        }

        public static List<Die> ConvertLettersToDieList(this string letters)
        {
            return letters.Select(letter => new Die { Value = letter.ToString() }).ToList();
        }

        public static List<List<Die>> ConvertListToMatrix(this List<Die> dice)
        {
            var matrix = new List<List<Die>>();
            var count = 0;

            while (count < dice.Count)
            {
                var row = new List<Die>();

                for (var j = 0; j < MatrixSize; j++)
                {
                    row.Add(dice[count]);
                    count++;
                }
                matrix.Add(row);
            }

            return matrix;
        }

        // Returns all die values as a single string
        public static string GetLetters(this List<List<Die>> dice)
        {
            return dice.SelectMany(row => row).Aggregate("", (current, die) => current + die.Value);
        }

        // TODO: simplify to improve readability
        public static bool ContainsWord(this string letters, string word)
        {
            var board = letters.ConvertLettersToDieList();
            var firstLetter = word[0].ToString();

            var checkList = new List<int>();    // FIFO
            var currentTry = new List<int>();   // used as path to find word     

            // finds all starting possibilities
            for (var i = 0; i < board.Count; i++)
            {
                if (board[i].Value == firstLetter)
                    checkList.Add(i);
            }

            while (checkList.Count > 0)
            {
                var currentDie = checkList[0];
                checkList.RemoveAt(0);
                currentTry.Add(currentDie);

                var currentWord = currentTry.Aggregate("", (current, i) => current + GetLetter(board, i));

                if (currentWord != word)
                {
                    var nextLetter = word[currentTry.Count].ToString();

                    var connected = GetConnectedMatches(currentDie, nextLetter, currentTry, board);

                    if (connected.Count == 0)    // go back and try new path                 
                    {                        
                        if (currentTry.Count == 1) 
                            currentTry.Clear();
                        else                                 
                            currentTry.RemoveRange(currentTry.Count - 2, 2);
                    }
                    else
                    {
                        foreach (var die in connected)
                        {
                            if (currentTry.Contains(die)) continue;

                            checkList.Insert(0, die);
                        }
                    }
                }
                else
                    return true;
            }

            return false;
        }

        private static string GetLetter(IReadOnlyList<Die> dieList, int index)
        {
            return dieList[index].Value;
        }

        private static List<int> GetConnectedMatches(int index, string letter, List<int> doneList, IReadOnlyList<Die> board)
        {
            var matches = new List<int>();
            ConnectedDieIndexes.TryGetValue(index, out var connected);

            if(connected != null)
                matches = connected.Where(die => GetLetter(board, die) == letter && !doneList.Contains(die)).ToList();

            return matches;
        }
    }
}