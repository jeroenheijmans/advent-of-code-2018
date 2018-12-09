﻿using System;
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
        public void Solution_1_test_examples(string input, long expected) => Assert.Equal(expected, Solve1(input));

        [Fact] public void Solution_1_test_real_input() => Assert.Equal(404611, Solve1(puzzleInput));

        [Fact] public void Solution_2_test_real_input() => Assert.Equal(3350093681, Solve2(puzzleInput));
        
        public long Solve1(string input)
        {
            var matches = Regex.Matches(input, @"(\d+) players; last marble is worth (\d+) points");
            long playerCount = int.Parse(matches.First().Groups[1].Value);
            long lastMarblePoints = int.Parse(matches.First().Groups[2].Value);

            return SolveInternal(playerCount, lastMarblePoints);
        }

        private static long SolveInternal(long playerCount, long lastMarblePoints)
        {
            var circle = new LinkedList<long>();

            var currentNode = circle.AddFirst(0);

            long nextMarbleValue = 0;

            long currentPlayer = 0;
            long[] scores = new long[playerCount];

            while (nextMarbleValue++ < lastMarblePoints)
            {
                if (nextMarbleValue % 23 == 0)
                {
                    var toRemove = currentNode;
                    var i = 0;
                    while (i++ < 7) toRemove = toRemove.Previous ?? circle.Last;
                    currentNode = toRemove.Next ?? circle.First;
                    
                    scores[currentPlayer] += nextMarbleValue + toRemove.Value;
                    circle.Remove(toRemove);
                }
                else
                {
                    currentNode = currentNode.Next ?? circle.First;
                    currentNode = circle.AddAfter(currentNode, nextMarbleValue);
                }

                currentPlayer = (currentPlayer + 1) % playerCount;
            }

            return scores.Max();
        }

        public long Solve2(string input)
        {
            var matches = Regex.Matches(input, @"(\d+) players; last marble is worth (\d+) points");
            long playerCount = int.Parse(matches.First().Groups[1].Value);
            long lastMarblePoints = int.Parse(matches.First().Groups[2].Value);

            return SolveInternal(playerCount, lastMarblePoints * 100);
        }
    }
}
