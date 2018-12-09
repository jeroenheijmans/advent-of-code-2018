using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day09
    {
        public const string puzzleInput = "431 players; last marble is worth 70950 points";

        [Theory]
        [InlineData("9 players; last marble is worth 25 points", 32)]
        [InlineData("10 players; last marble is worth 1618 points", 8317)]
        [InlineData("13 players; last marble is worth 7999 points", 146373)]
        [InlineData("17 players; last marble is worth 1104 points", 2764)]
        [InlineData("21 players; last marble is worth 6111 points", 54718)]
        [InlineData("30 players; last marble is worth 5807 points", 37305)]
        public void Solution_1_test_examples(string input, int expected) => Assert.Equal(expected, Solve1(input));

        [Fact] public void Solution_1_test_real_input() => Assert.Equal(404611, Solve1(puzzleInput));

        //[Fact] public void Solution_2_test_example() => Assert.Equal(0, Solve2(testInput));
        //[Fact] public void Solution_2_test_real_input() => Assert.Equal(0, Solve2(puzzleInput));
        
        public int Solve1(string input)
        {
            var matches = Regex.Matches(input, @"(\d+) players; last marble is worth (\d+) points");
            int playerCount = int.Parse(matches.First().Groups[1].Value);
            int lastMarblePoints = int.Parse(matches.First().Groups[2].Value);

            var circle = new List<int> { 0 };
            int currentMarbleIndex = 0;
            int nextMarbleValue = 0;
            int currentPlayer = 0;
            int[] scores = new int[playerCount];

            while (nextMarbleValue++ < lastMarblePoints)
            {
                if (nextMarbleValue % 23 == 0)
                {
                    var removeIndex = (currentMarbleIndex + circle.Count() - 7) % circle.Count();
                    scores[currentPlayer] += nextMarbleValue + circle[removeIndex];
                    circle.RemoveAt(removeIndex);
                    currentMarbleIndex = removeIndex;
                }
                else
                {
                    currentMarbleIndex = (currentMarbleIndex + 2) % circle.Count();
                    circle.Insert(currentMarbleIndex, nextMarbleValue);
                }

                currentPlayer = (currentPlayer + 1) % playerCount;
            }

            return scores.Max();
        }

        public int Solve2(string input)
        {
            var data = input.Split(",");

            return -1;
        }
    }
}
