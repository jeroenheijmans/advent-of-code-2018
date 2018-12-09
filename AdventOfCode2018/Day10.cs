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
    public class Day10
    {
        public const string testInput = "";
        public const string puzzleInput = "";

        [Fact] public void Solution_1_test_example() => Assert.Equal(0, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(0, Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example() => Assert.Equal(0, Solve2(testInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(0, Solve2(puzzleInput));

        public int Solve1(string input)
        {
            var data = input.Split(",");

            return -1;
        }

        public int Solve2(string input)
        {
            var data = input.Split(" ");

            return -1;
        }
    }
}
