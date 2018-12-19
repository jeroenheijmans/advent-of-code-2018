using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day20
    {
        private readonly ITestOutputHelper output;

        public Day20(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string testInput = @"";

        public const string puzzleInput = @"";

        [Fact] public void Solution_1_test_example() => Assert.Equal(-1, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(-1, Solve1(puzzleInput));

        public int Solve1(string input)
        {
            return 0;
        }
    }
}
