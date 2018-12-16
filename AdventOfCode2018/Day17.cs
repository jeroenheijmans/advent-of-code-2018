using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day17
    {
        private readonly ITestOutputHelper output;

        public Day17(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string testInput = "";
        public const string puzzleInput = "";

        [Fact] public void Solution_1_test_example() => Assert.Equal(0, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(0, Solve1(puzzleInput));
        
        public int Solve1(string input)
        {
            var data = input.Split(",");

            return -1;
        }
    }
}
