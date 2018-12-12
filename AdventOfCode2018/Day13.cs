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
    public class Day13
    {
        private readonly ITestOutputHelper output;

        public Day13(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string testInput = @"";

        public const string puzzleInput = @"";

        [Fact] public void Solution_1_test_example() => Assert.Equal(-1, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(-1, Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example() => Assert.Equal(-1, Solve2(testInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(-1, Solve2(puzzleInput));


        public int Solve1(string input)
        {
            var data = input.SplitByNewline();
            output.WriteLine("Testing 1, 2, 3...");
            return 0;
        }

        public int Solve2(string input)
        {
            var data = input.SplitByNewline();
            output.WriteLine("Testing 1, 2, 3...");
            return 0;
        }
    }
}
