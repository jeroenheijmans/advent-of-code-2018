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
    public class Day15
    {
        private readonly ITestOutputHelper output;

        public Day15(ITestOutputHelper output)
        {
            this.output = output;
        }

        private const string exampleInput = @"";
        private const string puzzleInput = @"";

        [Fact] public void Solution_1_test_example_1() => Assert.Equal("", Solve1(exampleInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal("", Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example_1() => Assert.Equal("", Solve2(exampleInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal("", Solve2(puzzleInput));


        public string Solve1(string input)
        {
            return "";
        }

        public string Solve2(string input)
        {
            return "";
        }
    }
}
