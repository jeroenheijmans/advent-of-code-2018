using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day25
    {
        private readonly ITestOutputHelper output;

        public Day25(ITestOutputHelper output)
        {
            this.output = output;
        }

        private const string testInput = @"";

        private const string puzzleInput = @"";

        [Fact] public void Solution_1_test_example_1() => Assert.Equal(-1, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(-1, Solve1(puzzleInput));

        public int Solve1(string input)
        {
            return 0;
        }
    }
}
